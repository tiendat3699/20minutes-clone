using System;
using UnityEngine;

public class EnemyDamageable : MonoBehaviour, IDamageable
{
    private int HP;
    private Rigidbody2D rb;
    private EnemyBehaviour enemyBehaviour;
    public event Action OnBegin, OnDone;

    private void Awake() {
        enemyBehaviour = GetComponent<EnemyBehaviour>();
        HP = enemyBehaviour.enemyScriptable.maxHP;
        rb = GetComponent<Rigidbody2D>();
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
            poolManager.enemyPooler.Release(enemyBehaviour);
            poolManager.expPooler.Spawn(transform.position, Quaternion.identity);
        }
    }

    private void ResetKnockBack() {
        OnDone?.Invoke();
    }

    private void OnDisable() {
        rb.velocity = Vector2.zero;
        HP = enemyBehaviour.enemyScriptable.maxHP;
    }
}
