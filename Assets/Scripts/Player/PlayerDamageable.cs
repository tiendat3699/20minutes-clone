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
    private int maxHp;
    private int hp;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();
    }

    private void Start() {
        maxHp = playerStats.maxHp;
        hp = maxHp;
    }

    private void OnEnable() {
        playerStats.OnUpgrade += UpdateMaxHealth;
    }

    public void UpdateMaxHealth(PlayerStats stats) {
        int diffHp = stats.maxHp - maxHp;
        hp += diffHp > 1 ? diffHp : 0; 
        maxHp = stats.maxHp;
    }

    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        if(hp <= 0) return;
        if(!immortal) {
                OnBegin?.Invoke();
                hitEffect.Play();
                hp--;
                OnHit?.Invoke(hp);
                if(hp > 0) {
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
