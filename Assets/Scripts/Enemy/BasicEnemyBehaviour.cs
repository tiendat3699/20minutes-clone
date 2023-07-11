using System;
using UnityEngine;

public enum EnemyState {
    Chase,
    Attack
}

[RequireComponent(typeof(Rigidbody2D))]
public class BasicEnemyBehaviour : MonoBehaviour
{
    public EnemyScriptableObject enemyScriptable;
    protected Transform target;
    protected EnemyState state;
    protected Rigidbody2D rb;
    protected SpriteRenderer sprite;
    protected EnemyDamageable damageable;
    protected Animator animator;
    private bool hitting, pause;
    private int hitHash;
    protected GameManager gameManager;

    private void Awake() {
        gameManager = GameManager.Instance;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        damageable = GetComponent<EnemyDamageable>();
        animator =  GetComponent<Animator>();
        hitHash = Animator.StringToHash("Hit");
    }

    private void OnEnable() {

        gameManager.OnPause += HandleOnPauseGame;
        gameManager.OnResume += HandleOnResumeGame;

        damageable.OnBegin += () => {
            animator.SetTrigger(hitHash);
            hitting = true;
            rb.velocity = Vector2.zero;
        };

        damageable.OnDone += () => {
            hitting = false;
            rb.velocity = Vector2.zero;
        };
    }

    private void OnDisable() {
        gameManager.OnPause -= HandleOnPauseGame;
        gameManager.OnResume -= HandleOnResumeGame;
    }

    private void Start() {
        target = GameManager.Instance.player;
    }

    protected virtual void FixedUpdate() {
        if(hitting || pause) return;
        switch(state) {
            case EnemyState.Chase:
                ChasePlayer();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            default:
                throw new ArgumentOutOfRangeException($"state {state} is invalid");
        }
    }

    private void HandleOnPauseGame() {
        pause = true;
        rb.velocity = Vector2.zero;
    }

    private void HandleOnResumeGame() {
        Invoke(nameof(Resume), 0.3f);
    }

    private void Resume() {
        pause = false;
    }

    protected virtual void ChasePlayer() {
        Vector2 moveDirection = (target.position - transform.position).normalized;
        if (moveDirection.x != 0) {
                sprite.flipX = moveDirection.x < 0;
        }
        rb.velocity = enemyScriptable.speed * moveDirection;
    }

    protected virtual void Attack() {

    }
}
