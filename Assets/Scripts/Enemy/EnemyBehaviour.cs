using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] public EnemyScriptableObject enemyScriptable;
    private Transform target;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private EnemyDamageable damageable;
    private bool hitting;

    private void Awake() {
        target = GameManager.Instance.player;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        damageable = GetComponent<EnemyDamageable>();
    }

    private void OnEnable() {
        damageable.OnBegin += () => {
            hitting = true;
            rb.velocity = Vector2.zero;
        };

        damageable.OnDone += () => {
            hitting = false;
        };
    }



    private void FixedUpdate() {
        if(hitting) return;
        Vector2 moveDirection = (target.position - transform.position).normalized;
        if (moveDirection.x != 0) {
                sprite.flipX = moveDirection.x < 0;
        }
        rb.velocity = enemyScriptable.speed * moveDirection;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        hitting = true;
    }

    private void OnCollisionExit2D(Collision2D other) {
        hitting = false;
    }
}
