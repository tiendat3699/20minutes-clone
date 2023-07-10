using System;
using System.Collections;
using UnityEngine;
using MyCustomAttribute;

public class PlayerAttackHandler : MonoBehaviour
{

    [SerializeField] private Transform shootPoin1;
    [SerializeField] private Transform shootPoin2;
    private Transform shootPoin;
    [SerializeField, ReadOnly] private int bulletAmount;
    [ReadOnly] public bool reloading, ready = true;
    private PlayerAnimationHandler animationHandler;
    private PlayerStats playerStats;
    private static event Action<float> OnReload;
    private PlayerController playerController;

    private void Awake() {
        shootPoin = shootPoin1;
        animationHandler = GetComponent<PlayerAnimationHandler>();
        playerController = GetComponent<PlayerController>();
        playerStats = GetComponent<PlayerStats>();
        bulletAmount = playerStats.ammo;
    }

    private void Update() {
        if (playerController.AimDirection.x != 0) {
            shootPoin = playerController.AimDirection.x > 0 ? shootPoin1 : shootPoin2;
        }

    }

    private void LateUpdate() {
        reloading = animationHandler.IsAnimPlaying("Player_reload", 1);
    }

    public void Attack(Vector2 dirAttack) {
        if(!reloading && ready && bulletAmount > 0) {
            float angle = 0;
            for(int i = 0; i < playerStats.projectile; i++) {
                angle = i % 2 == 0 ? angle + i * 10f : angle - i * 10f; 
                Vector3 dir = Quaternion.Euler(0, 0, angle) * dirAttack;
                Bullet bullet = PoolManager.Instance.bulletPooler.Spawn(shootPoin.position, Quaternion.identity);
                bullet.Fire(dir, playerStats.bulletSpeed, playerStats.damage);
            }
            bulletAmount--;
            if(bulletAmount ==0) {
                Reload();
                return;
            }
            ready = false;
            Invoke(nameof(WaitForNextAttack), playerStats.fireRateTime);
        }
    }

    public void Reload() {
        if(bulletAmount < playerStats.ammo && !reloading) {
            StartCoroutine(ReloadCoroutine());
        }
    }

    private void WaitForNextAttack() {
        ready = true;
    }

    private IEnumerator ReloadCoroutine() {
        float timer = 0;
        animationHandler.SetPlayReload(true);
        while(timer < playerStats.reloadTime) {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
            OnReload?.Invoke(timer);
        }
        animationHandler.SetPlayReload(false);
        bulletAmount = playerStats.ammo;

    }

    private void OnDrawGizmosSelected() {
        if(shootPoin != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(shootPoin1.position, 0.1f);
            Gizmos.DrawSphere(shootPoin2.position, 0.1f);
        }
    }
}
