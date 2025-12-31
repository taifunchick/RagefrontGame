using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Цель")]
    public Transform player;

    [Header("Передвижение")]
    public float moveSpeed = 3f;
    public bool useRigidbody = true;

    [Header("Здоровье")]
    public float maxHealth = 50f;
    private float currentHealth;

    [Header("Урон игроку")]
    public float damage = 10f;
    public float damageInterval = 1f;

    private float lastDamageTime;
    private bool isPlayerInTrigger = false;
    private Rigidbody2D rb;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
            else
                Debug.LogError("Player не найден! Установите тег 'Player' на игрока.");
        }
    }

    void Update()
    {
        if (player != null && currentHealth > 0)
        {
            Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;

            // === Поворот в сторону игрока (в 2D) ===
            // Предполагается, что "лицо" врага направлено вправо (вдоль оси X)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // Передвижение
            if (useRigidbody && rb != null)
            {
                rb.velocity = direction * moveSpeed;
            }
            else
            {
                transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
            }
        }

        if (isPlayerInTrigger && currentHealth > 0 && Time.time >= lastDamageTime + damageInterval)
        {
            PlayerHealth playerHealth = player?.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                lastDamageTime = Time.time;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}