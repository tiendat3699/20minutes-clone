using System;
using UnityEngine;
using MyCustomAttribute;

public class PlayerDamageable : MonoBehaviour, IDamageable
{

    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField, ReadOnly] private bool immortal;
    public event Action<int> OnHit;
    public event Action OnBegin;
    public event Action OnDone;
    public event Action OnDead;
    private Rigidbody2D rb;
    private PlayerStats playerStats;
    private int hp;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();
        hp = playerStats.maxHp;
    }

    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        if(!immortal) {
            if(hp > 0) {
                OnBegin?.Invoke();
                hitEffect.Play();
                hp--;
                OnHit?.Invoke(hp);
                rb.AddForce(hitDirection * 8f, ForceMode2D.Impulse);
                Invoke(nameof(ResetKnockBack), 0.2f);
                BeImmortal(0.5f);
            } else {
                GetComponent<PlayerAnimationHandler>().PlayDead();
                OnDead?.Invoke();
            }
        }
    }

    public void BeImmortal(float time) {
        immortal =  true;
        Invoke(nameof(CancelImmortal), time);
    }

    private void ResetKnockBack() {
        OnDone?.Invoke();
    }

    private void CancelImmortal() {
        immortal = false;
    }
}
