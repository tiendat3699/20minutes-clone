using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField, Range(0, 10)] private float speed;
    [SerializeField] private Transform hand;
    [SerializeField] SpriteRenderer weapon;
    private Rigidbody2D rb;
    private PlayerAnimationHandler animationHandler;
    private PlayerAttackHandler attackHandler;
    private SpriteRenderer sprite;
    private PlayerInput inputs;
    private PlayerDamageable damageable;
    private bool attackPress;
    private Vector2 moveDirection;
    private bool hitting, dead;
    private GameManager gameManager;
    public Vector2 AimDirection {get; private set;}

    private void Awake() {
        inputs = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
        animationHandler = GetComponent<PlayerAnimationHandler>();
        attackHandler = GetComponent<PlayerAttackHandler>();
        sprite = GetComponent<SpriteRenderer>();
        damageable = GetComponent<PlayerDamageable>();
        gameManager = GameManager.Instance;
        gameManager.player = transform;
    }


    private void OnEnable() {
        inputs.PlayerAction.Enable();

        //move input
        inputs.PlayerAction.Move.performed += GetMoveDirection;
        inputs.PlayerAction.Move.canceled += GetMoveDirection;
        //aim input
        inputs.PlayerAction.Aim.performed += GetAimInput;
        //attack input
        inputs.PlayerAction.Attack.started += GetAttackInput;
        inputs.PlayerAction.Attack.canceled += GetAttackInput;
        //reload input
        inputs.PlayerAction.Reload.performed += ReloadHandler;

        damageable.OnBegin += () => {
            hitting = true;
            rb.velocity = Vector2.zero;
        };

        damageable.OnDone += () => {
            hitting = false;
        };

        gameManager.OnPlayerDead += PlayerDead;
    }

    private void OnDisable() {
        inputs.PlayerAction.Disable();
        inputs.PlayerAction.Move.performed -= GetMoveDirection;
        inputs.PlayerAction.Move.canceled -= GetMoveDirection;
        inputs.PlayerAction.Aim.performed -= GetAimInput;
        inputs.PlayerAction.Attack.performed -= GetAttackInput;
        inputs.PlayerAction.Reload.performed += ReloadHandler;
        gameManager.OnPlayerDead -= PlayerDead;

    }

    private void Update() {
        if(!dead) {
            AimHandler();
            Attack();
        }
    }

    private void FixedUpdate() {
        if(!dead) {
            MoveHandler();
        }
    }

    private void LateUpdate() {
        GameManager.Instance.playerMoveDirection = rb.velocity.normalized;
    }

    private void PlayerDead() {
        dead = true;
    }

    private void GetMoveDirection(InputAction.CallbackContext ctx) {
        moveDirection = ctx.ReadValue<Vector2>();
    }

    private void MoveHandler() {
        if(hitting) return;
        if(moveDirection.magnitude > 0.2) {
            rb.velocity = speed * moveDirection;
        } else {
            rb.velocity = Vector2.zero;
        }
        animationHandler.PlayRun(moveDirection.magnitude);
        if (moveDirection.x != 0) {
            sprite.flipX = moveDirection.x < 0;
        }



    }

    private void GetAttackInput(InputAction.CallbackContext ctx) {
        attackPress = ctx.ReadValueAsButton();
    }

    private void GetAimInput(InputAction.CallbackContext ctx) {
        Vector2 pos = Camera.main.ScreenToWorldPoint(ctx.ReadValue<Vector2>());
        AimDirection = (pos - new Vector2(transform.position.x, transform.position.y)).normalized;
    }

    private void Attack() {
        if(attackPress) {
            attackHandler.Attack(AimDirection);
        }
    }

    private void AimHandler() {
        if(AimDirection != Vector2.zero && !attackHandler.reloading) {
            hand.eulerAngles = new Vector3(0,0, Utilities.Direction2Angle(AimDirection));
        }

        if (AimDirection.x != 0) {
            weapon.flipY = AimDirection.x < 0;
        }
    }

    private void ReloadHandler(InputAction.CallbackContext ctx) {
        bool press = ctx.ReadValueAsButton();
        if(press) {
            attackHandler.Reload();
        }
    }
}
