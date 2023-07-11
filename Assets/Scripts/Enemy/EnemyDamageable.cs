using System;
using UnityEngine;

public class EnemyDamageable : MonoBehaviour, IDamageable
{
    private int HP;
    private Rigidbody2D rb;
    private BasicEnemyBehaviour basicEnemyBehaviour;
    private GameManager gameManager;
    public event Action OnBegin, OnDone;
    private bool endGame;

    private void Awake() {
        basicEnemyBehaviour = GetComponent<BasicEnemyBehaviour>();
        HP = basicEnemyBehaviour.enemyScriptable.maxHP;
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameManager.Instance;
    }

    private void OnEnable() {
        gameManager.OnWin += HandleOnWin;
    }

    private void OnDisable() {
        rb.velocity = Vector2.zero;
        HP = basicEnemyBehaviour.enemyScriptable.maxHP;
        gameManager.OnWin -= HandleOnWin;
    }

    private void HandleOnWin() {
        endGame = true;
        Vector2 dir = (gameManager.player.position - transform.position).normalized;
        TakeDamage(basicEnemyBehaviour.enemyScriptable.maxHP, dir);
    }

    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        if(HP > 0) {
            CancelInvoke(nameof(ResetKnockBack));
            OnBegin?.Invoke();
            HP -= damage;
            rb.AddForce(hitDirection * 3f, ForceMode2D.Impulse);
            Invoke(nameof(ResetKnockBack), 0.3f);
        } else {
            PoolManager poolManager = PoolManager.Instance;
            poolManager.enemyPooler.Release(basicEnemyBehaviour);
            poolManager.hitImpactPooler.Spawn(transform.position, Quaternion.identity);
            if(!endGame) {
                poolManager.expPooler.Spawn(transform.position, Quaternion.identity);
                GameManager.Instance.IncreaseKill();
            }
        }
    }

    private void ResetKnockBack() {
        OnDone?.Invoke();
    }
}
