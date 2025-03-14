using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;
    public float speed = 30f;
    public float lifetime = 3f;
    
    private void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
        Destroy(gameObject, lifetime); // Destroy bullet after lifetime seconds
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IEnemy enemy))
        {
            enemy.TakeDamage(damage);
        }
        
        // Destroy the bullet on any collision
        Destroy(gameObject);
    }
}
