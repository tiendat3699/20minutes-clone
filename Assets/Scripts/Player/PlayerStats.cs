using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private PlayerSO playerSO;
    public int projectile {get; private set;}
    public int maxHp {get; private set;}
    public int ammo {get; private set;}
    public int speed {get; private set;}
    public int bulletSpeed {get; private set;}
    public float reloadTime {get; private set;}
    public float fireRateTime {get; private set;}
    public int damage {get; private set;}
    public event Action<PlayerStats> OnUpgrade;

    private void Awake() {
        projectile = playerSO.projectile;
        maxHp = playerSO.maxHp;
        ammo = playerSO.ammo;
        speed = playerSO.speed;
        bulletSpeed = playerSO.bulletSpeed;
        reloadTime = playerSO.reloadTime;
        fireRateTime = playerSO.fireRateTime;
        damage = playerSO.damage;
    }

    private void Start() {
        DebugLog();
    }


    public void Upgrade(TalentBase talent) {
        projectile += talent.projectilePlus;
        maxHp += talent.maxHpPlus;
        ammo += (int)(talent.ammoRate * playerSO.ammo) / 100;
        speed += (int)(talent.speedRate * playerSO.speed) / 100;
        bulletSpeed += (int)(talent.bulletSpeedRate * playerSO.bulletSpeed) / 100;
        reloadTime -= (talent.reloadRate * playerSO.reloadTime) / 100;
        fireRateTime -= (talent.fireRate * playerSO.fireRateTime) / 100;
        damage += (int)(talent.damageRate * playerSO.damage) / 100;
        OnUpgrade?.Invoke(this);
        DebugLog();
    }


    private void DebugLog(){
        GameManager.Instance.SetDebugLog(@$"  
        projectile: {projectile}
        maxHp: {maxHp}
        ammo: {ammo}
        speed: {speed}
        bulletSpeed: {bulletSpeed}
        reloadTime: {reloadTime}s 
        fireRateTime: {fireRateTime}s
        damage: {damage}
        ");
    }
}
