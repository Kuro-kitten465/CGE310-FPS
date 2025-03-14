using UnityEngine;

public interface IEnemy
{
    public void TakeDamage(float damage);
    public bool IsAlive();
}
