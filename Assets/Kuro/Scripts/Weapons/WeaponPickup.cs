using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponData weaponData;
    public GameObject weaponPrefab;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerWeaponHandler>(out var weaponHandler))
            {
                weaponHandler.PickupWeapon(weaponPrefab, weaponData);
                Destroy(gameObject);
            }
        }
    }
}
