using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    [Header("Sword & Shield")]
    [SerializeField] private Transform swordShieldModel;
    [SerializeField] private int swordDamage;
    [SerializeField] private float swordAttackSpeed;
    [SerializeField] private int swordAttackCooldown;
    
    [Space]
    
    [Header("Greatsword")]
    [SerializeField] private Transform greatswordModel;
    [SerializeField] private int greatswordDamage;
    [SerializeField] private float greatswordAttackSpeed;
    [SerializeField] private int greatswordAttackCooldown;


    public static WeaponManager Instance;
    private PlayerController playerController;
    public Animator weaponAnimator;
    private int weaponDamage;
    private float weaponAttackSpeed;
    private int weaponAttackCooldown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

        if(Instance == null)
        {
            Instance = this;
        }

        // Begin with sword and shield by default
        swordShieldModel.gameObject.SetActive(true);
        weaponAnimator = swordShieldModel.gameObject.GetComponent<Animator>();
        weaponDamage = swordDamage;
        weaponAttackSpeed = swordAttackSpeed;
        weaponAttackCooldown = swordAttackCooldown;

        greatswordModel.gameObject.SetActive(false);
        
        playerController = gameObject.GetComponent<PlayerController>();
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.GetCanSwapWeapons() == true)
        {
            // Swap weapons 
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if(swordShieldModel.gameObject.activeSelf == true)
                {
                    swordShieldModel.gameObject.SetActive(false);
                    
                    greatswordModel.gameObject.SetActive(true);
                    greatswordModel.gameObject.GetComponent<Animator>().enabled = true;

                    SetGreatswordStats();

                    weaponAnimator = greatswordModel.gameObject.GetComponent<Animator>();
                }
                else if(greatswordModel.gameObject.activeSelf == true)
                {
                    greatswordModel.gameObject.SetActive(false);
                    
                    swordShieldModel.gameObject.SetActive(true);
                    swordShieldModel.gameObject.GetComponent<Animator>().enabled = true;

                    SetSwordStats();

                    weaponAnimator = swordShieldModel.gameObject.GetComponent<Animator>();
                }
            }
        }
    }

    private void SetSwordStats()
    {
        weaponDamage = swordDamage;
        weaponAttackSpeed = swordAttackSpeed;
        weaponAttackCooldown = swordAttackCooldown;
    }

    private void SetGreatswordStats()
    {
        weaponDamage = greatswordDamage;
        weaponAttackSpeed = greatswordAttackSpeed;
        weaponAttackCooldown = greatswordAttackCooldown;
    }

    public Animator GetWeaponAnimator() => weaponAnimator;
    public int GetWeaponDamage() => weaponDamage;
    public float GetWeaponAttackSpeed() => weaponAttackSpeed;
    public int GetWeaponAttackCooldown() => weaponAttackCooldown;
    public bool IsHoldingSwordShield() => swordShieldModel.gameObject.activeSelf;
    public bool IsHoldingGreatsword() => greatswordModel.gameObject.activeSelf;

}
