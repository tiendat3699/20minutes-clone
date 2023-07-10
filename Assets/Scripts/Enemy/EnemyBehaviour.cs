using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public EnemyScriptableObject enemyScriptable;
    private Transform target;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private EnemyDamageable damageable;
    private Animator animator;
    private bool hitting, pause;
    private int hitHash;
    private GameManager gameManager;

    private void Awake() {
        gameManager = GameManager.Instance;
        target = GameManager.Instance.player;
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
        };
    }

    private void OnDisable() {
        gameManager.OnPause -= HandleOnPauseGame;
        gameManager.OnResume -= HandleOnResumeGame;
    }



    private void FixedUpdate() {
        if(hitting || pause) return;
        Vector2 moveDirection = (target.position - transform.position).normalized;
        if (moveDirection.x != 0) {
                sprite.flipX = moveDirection.x < 0;
        }
        rb.velocity = enemyScriptable.speed * moveDirection;
    }

    private void HandleOnPauseGame() {
        pause = true;
        rb.velocity = Vector2.zero;
    }

    private void HandleOnResumeGame() {
        Invoke(nameof(Resume), 0.5f);
    }

    private void Resume() {
        pause = false;
    }
}
