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
    private bool attackPress;
    private Vector2 moveDirection, aimDirection;

    private void Awake() {
        inputs = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
        animationHandler = GetComponent<PlayerAnimationHandler>();
        attackHandler = GetComponent<PlayerAttackHandler>();
        sprite = GetComponent<SpriteRenderer>();
        GameManager.Instance.player = transform;
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
    }

    private void OnDisable() {
        inputs.PlayerAction.Disable();
        inputs.PlayerAction.Move.performed -= GetMoveDirection;
        inputs.PlayerAction.Move.canceled -= GetMoveDirection;
        inputs.PlayerAction.Aim.performed -= GetAimInput;
        inputs.PlayerAction.Attack.performed -= GetAttackInput;
        inputs.PlayerAction.Reload.performed += ReloadHandler;
    }

    private void Update() {
        AimHandler();
        Attack();
    }

    private void FixedUpdate() {
        MoveHandler();
    }

    private void GetMoveDirection(InputAction.CallbackContext ctx) {
        moveDirection = ctx.ReadValue<Vector2>();
        GameManager.Instance.playerMoveDirection = moveDirection;
    }

    private void MoveHandler() {
        if(moveDirection.magnitude > 0.2) {
            // rb.AddForce(moveDirection * speed, ForceMode2D.Force);
            rb.MovePosition(rb.position + (0.02f * speed * moveDirection));
            // controller.Move(speed * Time.deltaTime * moveDirection);
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
        aimDirection = (pos - new Vector2(transform.position.x, transform.position.y)).normalized;
    }

    private void Attack() {
        if(attackPress) {
            attackHandler.Attack(aimDirection);
        }
    }

    private void AimHandler() {
        if(aimDirection != Vector2.zero && !attackHandler.reloading) {
            hand.eulerAngles = new Vector3(0,0, Utilities.Direction2Angle(aimDirection));
        }

        if (aimDirection.x != 0) {
            weapon.flipY = aimDirection.x < 0;
        }
    }

    private void ReloadHandler(InputAction.CallbackContext ctx) {
        bool press = ctx.ReadValueAsButton();
        if(press) {
            attackHandler.Reload();
        }
    }
}
