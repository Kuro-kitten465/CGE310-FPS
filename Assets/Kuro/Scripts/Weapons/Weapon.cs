using System;
using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WeaponType
    {
        Pistol,
        Shotgun,
        AR,
        SMG,
        Sniper
    }

    public WeaponData weaponData;
    [SerializeField] private Transform shootPoint;
    public int CurrentAmmo => currentAmmo;
    
    private int currentAmmo;
    private bool isReloading = false;
    private float lastShotTime;
    private AudioSource audioSource;
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    
    private void Start()
    {
        currentAmmo = weaponData.magazineSize;
    }
    
    public void Shoot()
    {
        if (isReloading || Time.time - lastShotTime < weaponData.fireRate)
            return;
            
        if (currentAmmo <= 0)
        {
            StartReload();
            return;
        }
        
        // Create bullet
        GameObject bullet = Instantiate(weaponData.bulletPrefab, shootPoint.position, shootPoint.rotation);
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        if (bulletComponent != null)
        {
            bulletComponent.damage = weaponData.damage;
        }
        
        // Play effects
        if (weaponData.muzzleFlash != null)
        {
            Instantiate(weaponData.muzzleFlash, shootPoint.position, shootPoint.rotation, transform);
        }
        
        if (weaponData.shootSound != null)
        {
            audioSource.PlayOneShot(weaponData.shootSound);
        }
        
        currentAmmo--;
        lastShotTime = Time.time;
    }
    
    public void StartReload()
    {
        if (!isReloading && currentAmmo < weaponData.magazineSize)
        {
            StartCoroutine(ReloadRoutine());
        }
    }
    
    private IEnumerator ReloadRoutine()
    {
        isReloading = true;
        
        if (weaponData.reloadSound != null)
        {
            audioSource.PlayOneShot(weaponData.reloadSound);
        }
        
        // Wait for reload time
        yield return new WaitForSeconds(weaponData.reloadTime);
        
        // Refill magazine
        currentAmmo = weaponData.magazineSize;
        isReloading = false;
    }
    
    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }
    
    public int GetMaxAmmo()
    {
        return weaponData.magazineSize;
    }
    
    public bool IsReloading()
    {
        return isReloading;
    }
}
