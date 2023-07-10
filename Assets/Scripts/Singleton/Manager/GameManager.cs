using System;
using UnityEngine;
using MyCustomAttribute;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int maxPlayerHP;
    public float countdownTime;
    [ReadOnly] public Transform player;
    [ReadOnly] public Vector2 playerMoveDirection;
    private int playerLevel = 1;
    private int ExpToUpLevel = 10;
    private int playerExp;
    private int playerHP;
    private int killCount;
    private float timer;
    [ReadOnly] public bool isDead;
    public event Action<int, int> OnUpLevel;
    public event Action<int> OnIncreaseExp;
    public event Action<int> OnIncreaseKill;
    public event Action<int> OnUpdatePlayerHP;
    public event Action<int> OnUpdateMaxPlayerHP;
    public event Action<float> OnUpdateTime;
    public event Action OnPlayerDead;

    private void Start() {
        Init();
    }

    private void Update() {
        timer -= Time.deltaTime;
        OnUpdateTime?.Invoke(timer);
    }

    private void Init() {
        timer = countdownTime;
        playerHP = maxPlayerHP;
        OnUpLevel?.Invoke(playerLevel, ExpToUpLevel);
        OnUpdateMaxPlayerHP?.Invoke(maxPlayerHP);
        OnUpdatePlayerHP?.Invoke(playerHP);
    }

    public void UpLevel() {
        playerLevel++;
        playerExp = 0;
        ExpToUpLevel = (int)(ExpToUpLevel * 1.5f);
        OnUpLevel?.Invoke(playerLevel, ExpToUpLevel);
    }

    public void IncreaseExp(int exp) {
        playerExp += exp;
        OnIncreaseExp?.Invoke(playerExp);
        if(playerExp >= ExpToUpLevel) {
            UpLevel();
        }
    }

    public void IncreaseKill() {
        killCount++;
        OnIncreaseKill?.Invoke(killCount);
    }

    public void UpdatePlayerHP(int hp) {
        if(!isDead) {
            playerHP += hp;
            OnUpdatePlayerHP?.Invoke(playerHP);
            if(playerHP <= 0) {
                isDead = true;
                OnPlayerDead?.Invoke();
            }
        }
    }

    public void UpdateMaxHealth(int maxHP) {
        maxPlayerHP = maxHP;
        OnUpdateMaxPlayerHP?.Invoke(maxPlayerHP);
    }


}
