using System;
using System.Collections;
using UnityEngine;
using MyCustomAttribute;

public class PlayerAttackHandler : MonoBehaviour
{
    public int ammoLimit;
    public float speedBullet;
    public int damage;
    [SerializeField] private Transform shootPoin1;
    [SerializeField] private Transform shootPoin2;
    private Transform shootPoin;
    [SerializeField, ReadOnly] private int bulletAmount;
    public float timeReload;
    public float timeFireRate;
    [ReadOnly] public bool reloading, ready = true;
    private PlayerAnimationHandler animationHandler;
    private static event Action<float> OnReload;
    private PlayerController playerController;

    private void Awake() {
        shootPoin = shootPoin1;
        animationHandler = GetComponent<PlayerAnimationHandler>();
        playerController = GetComponent<PlayerController>();
        bulletAmount = ammoLimit;
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
            Bullet bullet = PoolManager.Instance.bulletPooler.Spawn(shootPoin.position, Quaternion.identity);
            bullet.Fire(dirAttack, speedBullet, damage);
            bulletAmount--;
            if(bulletAmount ==0) {
                Reload();
                return;
            }
            ready = false;
            Invoke(nameof(WaitForNextAttack), timeFireRate);
        }
    }

    public void Reload() {
        if(bulletAmount < ammoLimit && !reloading) {
            StartCoroutine(ReloadCoroutine());
        }
    }

    private void WaitForNextAttack() {
        ready = true;
    }

    private IEnumerator ReloadCoroutine() {
        float timer = 0;
        animationHandler.SetPlayReload(true);
        while(timer < timeReload) {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
            OnReload?.Invoke(timer);
        }
        animationHandler.SetPlayReload(false);
        bulletAmount = ammoLimit;

    }

    private void OnDrawGizmosSelected() {
        if(shootPoin != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(shootPoin1.position, 0.1f);
            Gizmos.DrawSphere(shootPoin2.position, 0.1f);
        }
    }
}
