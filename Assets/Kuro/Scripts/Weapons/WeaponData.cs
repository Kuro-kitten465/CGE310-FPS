using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public Weapon.WeaponType weaponType;
    public GameObject model;
    public GameObject bulletPrefab;
    public Vector3 offset;
    
    [Header("Weapon Stats")]
    public float damage = 10f;
    public float fireRate = 0.5f; // Time between shots
    public int magazineSize = 10;
    public float reloadTime = 1.5f;
    public bool isAutomatic = false;
    
    [Header("Weapon Effects")]
    public AudioClip shootSound;
    public AudioClip reloadSound;
    public ParticleSystem muzzleFlash;
}
