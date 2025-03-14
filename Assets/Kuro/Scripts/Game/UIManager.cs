using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Weapon;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject PlayerComp;
    [SerializeField] private GameObject dotUI;
    [SerializeField] private bool hideUI = false;

    [Header("HPBar")]
    [SerializeField] private Slider hpBar;
    [SerializeField] private Image hpBarFill;
    [SerializeField] private Color onHealtyColor;
    [SerializeField] private Color onNotGoodColor;
    [SerializeField] private Color onDangerColor;
    [SerializeField] private Color onVeryDeadColor;

    [Header("Weapon Info")]
    [SerializeField] private TMP_Text bulletCountText;
    [SerializeField] private Image weaponIcon;
    [SerializeField] private GameObject weaponInfoPanel;
    [SerializeField] private Sprite shotgunIcon;
    [SerializeField] private Sprite pistolIcon;
    [SerializeField] private Sprite rifleIcon;
    [SerializeField] private Sprite smgIcon;

    [Header("Heal Info")]
    [SerializeField] private TMP_Text healCountText;
    [SerializeField] private Image healIcon;
    [SerializeField] private GameObject healInfoPanel;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("Game End")]
    [SerializeField] private GameObject gameEndPanel;

    [Header("Reload Text")]
    [SerializeField] private GameObject reloadText;

    private PlayerManager playerManager;
    private PlayerWeaponHandler playerWeapon;

    void Start()
    {
        PlayerComp = GameObject.FindGameObjectWithTag("Player");
        playerManager = PlayerComp.GetComponent<PlayerManager>();
        playerWeapon = PlayerComp.GetComponent<PlayerWeaponHandler>();
        hpBar.maxValue = playerManager.maxHealth;
    }

    private void HideUI()
    {
        if (hideUI)
        {
            weaponInfoPanel.SetActive(false);
            dotUI.SetActive(false);
            hpBar.gameObject.SetActive(false);
            healInfoPanel.SetActive(false);
        }
        else
        {
            weaponInfoPanel.SetActive(true);
            dotUI.SetActive(true);
            hpBar.gameObject.SetActive(true);
            healInfoPanel.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            hideUI = !hideUI;
            HideUI();
        }

        gameOverPanel.SetActive(GameManager.Instance.IsGameOver);
        gameEndPanel.SetActive(GameManager.Instance.IsGameEnd);

        if (playerWeapon.HasWeapon && !hideUI)
        {
            weaponInfoPanel.SetActive(true);
            reloadText.SetActive(playerWeapon.CurrentWeapon.IsReloading());
        }
        else if (!playerWeapon.HasWeapon && !hideUI)
        {
            weaponIcon.sprite = null;
        }

        dotUI.SetActive(playerWeapon.HasWeapon && !hideUI);
        UpdateHPBar();
        UpdateWeaponInfo();
        UpdateHealInfo();
    }

    private void GameOver()
    {
        gameOverPanel.SetActive(GameManager.Instance.IsGameOver);
    }

    private void UpdateHPBar()
    {
        hpBar.value = playerManager.GetHealth();

        if (hpBar.value >= playerManager.maxHealth * 0.75f)
        {
            hpBarFill.color = onHealtyColor;
        }
        else if (hpBar.value >= playerManager.maxHealth * 0.5f)
        {
            hpBarFill.color = onNotGoodColor;
        }
        else if (hpBar.value >= playerManager.maxHealth * 0.25f)
        {
            hpBarFill.color = onDangerColor;
        }
        else
        {
            hpBarFill.color = onVeryDeadColor;
        }
    }

    private void UpdateWeaponInfo()
    {
        if (!playerWeapon.HasWeapon)
        {
            bulletCountText.text = "No weapon";
            weaponIcon.sprite = null;
            weaponIcon.color = Color.clear;
            return;
        }
        else
        {
            weaponIcon.color = Color.white;
        }

        bulletCountText.text = playerWeapon.CurrentWeapon.CurrentAmmo + " / " + playerWeapon.CurrentWeapon.weaponData.magazineSize;

        switch (playerWeapon.CurrentWeapon.weaponData.weaponType)
        {
            case WeaponType.Shotgun:
                weaponIcon.sprite = shotgunIcon;
                break;
            case WeaponType.Pistol:
                weaponIcon.sprite = pistolIcon;
                break;
            case WeaponType.AR:
                weaponIcon.sprite = rifleIcon;
                break;
            case WeaponType.SMG:
                weaponIcon.sprite = smgIcon;
                break;
        }
    }

    private void UpdateHealInfo()
    {
        healCountText.text = "x" + playerManager.healItem;
    }
}
