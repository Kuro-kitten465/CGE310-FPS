using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float health = 100f;

    public int healItem = 0;

    public float maxHealth = 0f;
    public float GetHealth() => health;

    void Awake()
    {
        maxHealth = health;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver || GameManager.Instance.IsGameEnd) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (health >= maxHealth) return;

            if (healItem > 0)
            {
                healItem--;
                health = Mathf.Clamp(999, 0, maxHealth);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Heal"))
        {
            healItem++;
            Destroy(other.gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    private void Die()
    {
        GameManager.Instance.IsGameOver = true;
    }
}
