using System;
using UnityEngine;
using MyCustomAttribute;

public class GameManager : Singleton<GameManager>
{
    public int maxPlayerHP;
    [ReadOnly] public Transform player;
    [ReadOnly] public Vector2 playerMoveDirection;
    [SerializeField, ReadOnly] private int playerLevel = 1;
    [SerializeField, ReadOnly] private int playerExp;
    [SerializeField, ReadOnly] private int ExpToUpLevel = 10;
    [SerializeField, ReadOnly] private int playerHP;
    public bool isDead;
    public event Action<int> OnUpLevel;
    public event Action<int> OnIncreaseExp;
    public event Action<int> OnUpdatePlayerHP;
    public event Action OnPlayerDead;

    private void Start() {
        playerHP = maxPlayerHP;
    }

    public void UpLevel() {
        playerLevel++;
        OnUpLevel?.Invoke(playerLevel);
    }

    public void IncreaseExp(int exp) {
        playerExp += exp;
        OnIncreaseExp?.Invoke(playerExp);
        if(playerExp >= ExpToUpLevel) {
            UpLevel();
            ExpToUpLevel = (int)(ExpToUpLevel * 1.5f);
        }
    }

    public void UpdatePlayerHP(int hp) {
        if(playerHP > 0) {
            playerHP += hp;
            OnUpdatePlayerHP?.Invoke(playerHP);
        } else {
            isDead = true;
            OnPlayerDead?.Invoke();
        }
    }
}
