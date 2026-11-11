using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    private static readonly int isBlockingHash = Animator.StringToHash("isBlocking");
    private static readonly int AttackSpeedHash = Animator.StringToHash("AttackSpeed");
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int SpeedHash = Animator.StringToHash("Speed");

    //[SerializeField] private GameObject enemy;

    [Header("Movement")]
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private Transform cameraTransform;

    private WeaponManager weaponManager;
    private bool canSwapWeapons;

    /*
    [Header("Particle Systems")]
    [SerializeField] ParticleSystem hitParticleSystem;
    [SerializeField] ParticleSystem deathParticleSystem;
    */

    private bool isAttacking;
    private bool isBlocking;
    private float nextAttackTime;
    private Vector3 direction;
    private Vector3 downwardSpeed;
    //private EnemyAI enemyAI;
    private Animator animator;
    private CharacterController characterController;
    public static PlayerController Instance;
    void Awake()
    {   
        //enemyAI = enemy.GetComponent<EnemyAI>();

        if(Instance == null)
        {
            Instance = this;
        }

        characterController = gameObject.GetComponent<CharacterController>();
        weaponManager = gameObject.GetComponent<WeaponManager>();
        cameraTransform = Camera.main.transform;
        canSwapWeapons = true;
    }

    void Start()
    {
        animator = weaponManager.GetWeaponAnimator();        
    }

    // Update is called once per frame
    void Update()
    {
        animator = weaponManager.GetWeaponAnimator();        

        if (PlayerState.Instance.GetHealth() <= 0)
            return;
        
        float input_x = Input.GetAxis("Horizontal");
        float input_z = Input.GetAxis("Vertical");
        Vector3 forward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up);
        Vector3 right = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up);
        direction = (forward * input_z + right * input_x).normalized;

        if(characterController.enabled == true)
        {
            if (direction == Vector3.zero)
                animator.SetFloat(SpeedHash, 0f);
            else
                animator.SetFloat(SpeedHash, speed);

            characterController.Move(speed * Time.deltaTime * direction);
        
            if (characterController.isGrounded && downwardSpeed.y < 0)
                downwardSpeed.y = 0f;

            downwardSpeed.y -= 9.81f * Time.deltaTime;
            characterController.Move(downwardSpeed * Time.deltaTime); 
            
            if (direction != Vector3.zero)
                transform.forward = direction;
        }
        else
        {
            
        }

        isAttacking = AnimationEventHandler.Instance.GetIsAttacking();
        isBlocking = AnimationEventHandler.Instance.GetIsBlocking();

        if(Mouse.current == null)
            return;

        // Conditions to attack
        if (!isAttacking 
            && characterController.enabled == true
            && Mouse.current.leftButton.wasPressedThisFrame
            && !isBlocking)
        {
            Attack();
            return;
        }
        
        // Conditions to block/unblock
        if (!IsAttackingAnimation()
            && !isAttacking
            && Mouse.current != null
            && WeaponManager.Instance.IsHoldingSwordShield())
        {
            if (Mouse.current.rightButton.isPressed)
            {
                Block();
            }
            else if (!Mouse.current.rightButton.isPressed && isBlocking)
            {
                Unblock();
            }
        }
        
    }

    private bool IsAttackingAnimation()
    {
        if (animator.IsInTransition(0))
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")
                || animator.GetNextAnimatorStateInfo(0).IsName("Attack");
        }

        return animator.GetCurrentAnimatorStateInfo(0).IsName("Attack");
    }

    public void Attack()
    {
        Debug.Log("Attacking");
        if (isAttacking)
            return;
        
        characterController.enabled = false;

        isAttacking = true;
        isBlocking = false;
        canSwapWeapons = false;
        
        if(WeaponManager.Instance.IsHoldingSwordShield() == true)
            animator.SetBool(isBlockingHash, false);

        animator.SetFloat(SpeedHash, 0f);

        animator.SetFloat(AttackSpeedHash, WeaponManager.Instance.GetWeaponAttackSpeed());

        animator.ResetTrigger(AttackHash);
        animator.SetTrigger(AttackHash);
    }

    public void Block()
    {
        Debug.Log("Blocking");
        characterController.enabled = false;
        
        isBlocking = true;
        canSwapWeapons = false;
        
        animator.SetBool(isBlockingHash, true);

        animator.SetFloat(SpeedHash, 0f);        
    }

    public void Unblock()
    {
        Debug.Log("Stopped Blocking");
        isBlocking = false;
        canSwapWeapons = true;

        animator.SetBool(isBlockingHash, false);

        characterController.enabled = true;
    }

    public bool GetCanSwapWeapons() => canSwapWeapons;
    public void SetCanSwapWeapons(bool value)
    {
        canSwapWeapons = value;
    }
}