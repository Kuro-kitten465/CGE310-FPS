using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
    [SerializeField] private Transform weaponSocket; // Where to attach weapons on the player
    [SerializeField] private LayerMask groundLayer; // For dropping weapons

    public bool HasWeapon => currentWeapon != null;
    public Weapon CurrentWeapon => currentWeapon;
    
    private GameObject currentWeaponObject;
    private Weapon currentWeapon;
    
    private void Update()
    {
        if (GameManager.Instance.IsGameOver || GameManager.Instance.IsGameEnd)
            return;

        if (currentWeapon == null)
            return;
            
        // Example inputs - you can modify these as needed
        if (Input.GetMouseButton(0) && currentWeapon.weaponData.isAutomatic)
        {
            currentWeapon.Shoot();
        }
        else if (Input.GetMouseButtonDown(0) && !currentWeapon.weaponData.isAutomatic)
        {
            currentWeapon.Shoot();
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            currentWeapon.StartReload();
        }
        
        // Drop weapon
        if (Input.GetKeyDown(KeyCode.G))
        {
            DropWeapon();
        }
    }

    public void PickupWeapon(GameObject weaponPrefab, WeaponData weaponData)
    {
        // Drop current weapon if we have one
        if (currentWeaponObject != null)
        {
            DropWeapon();
        }
        
        // Instantiate and setup new weapon
        currentWeaponObject = Instantiate(weaponPrefab, weaponSocket);
        currentWeaponObject.transform.localPosition = weaponData.offset;
        currentWeaponObject.transform.localRotation = Quaternion.identity;
        
        // Get and setup weapon component
        currentWeapon = currentWeaponObject.GetComponent<Weapon>();
        if (currentWeapon == null)
        {
            currentWeapon = currentWeaponObject.AddComponent<Weapon>();
        }
    }
    
    public void DropWeapon()
    {
        if (currentWeaponObject == null)
            return;
            
        // Create a pickup for the dropped weapon
        GameObject pickup = new(currentWeapon.weaponData.weaponName + " Pickup");
        pickup.AddComponent<BoxCollider>().isTrigger = true;
        WeaponPickup pickupComponent = pickup.AddComponent<WeaponPickup>();
        pickupComponent.weaponData = currentWeapon.weaponData;
        pickupComponent.weaponPrefab = currentWeaponObject;
        
        // Position the pickup in front of the player
        Ray ray = new(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f, groundLayer))
        {
            pickup.transform.position = hit.point + Vector3.up * 0.5f;
        }
        else
        {
            pickup.transform.position = transform.position + transform.forward * 2f;
        }
        
        // Add weapon model to pickup
        currentWeaponObject.transform.parent = pickup.transform;
        currentWeaponObject.transform.localPosition = Vector3.zero;
        
        // Clear current weapon references
        currentWeaponObject = null;
        currentWeapon = null;
    }
}
