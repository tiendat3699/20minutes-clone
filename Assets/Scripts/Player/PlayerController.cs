using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField, Range(0, 5)] private float speed;
    [SerializeField] private Transform hand;
    [SerializeField] SpriteRenderer weapon;
    private CharacterController controller;
    private PlayerAnimationHandler animationHandler;
    private PlayerAttackHandler attackHandler;
    private SpriteRenderer sprite;
    private PlayerInput inputs;
    private bool attackPress;
    private Vector2 moveDirection, aimDirection;

    private void Awake() {
        inputs = new PlayerInput();
        controller = GetComponent<CharacterController>();
        animationHandler = GetComponent<PlayerAnimationHandler>();
        attackHandler = GetComponent<PlayerAttackHandler>();
        sprite = GetComponent<SpriteRenderer>();
    }
    
    private void OnEnable() {
        inputs.PlayerAction.Enable();
        
        //move input
        inputs.PlayerAction.Move.performed += GetMoveDirection;
        inputs.PlayerAction.Move.canceled += GetMoveDirection;
        //aim input
        inputs.PlayerAction.Aim.performed += AimHandler;
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
        inputs.PlayerAction.Aim.performed -= AimHandler;
        inputs.PlayerAction.Attack.performed -= GetAttackInput;
        inputs.PlayerAction.Reload.performed += ReloadHandler;
    }

    private void Update() {
        MoveHandler();
        Attack();
    }

    private void GetMoveDirection(InputAction.CallbackContext ctx) {
        moveDirection = ctx.ReadValue<Vector2>();
    }

    private void MoveHandler() {
        if(moveDirection.magnitude > 0.2) {
            controller.Move(moveDirection * speed * Time.deltaTime);
        }
        animationHandler.PlayRun(moveDirection.magnitude);
        if (moveDirection.x != 0) {
                sprite.flipX = moveDirection.x < 0;
        }
    }

    private void GetAttackInput(InputAction.CallbackContext ctx) {
        attackPress = ctx.ReadValueAsButton();
    }

    private void Attack() {
        if(attackPress) {
            attackHandler.Attack(aimDirection);
        }
    }

    private void AimHandler(InputAction.CallbackContext ctx) {
        Vector2 pos = Camera.main.ScreenToWorldPoint(ctx.ReadValue<Vector2>());
        aimDirection = (pos - new Vector2(transform.position.x, transform.position.y)).normalized;
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
