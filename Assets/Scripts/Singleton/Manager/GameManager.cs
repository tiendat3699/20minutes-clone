using System;
using UnityEngine;
using MyCustomAttribute;

public class GameManager : Singleton<GameManager>
{
    [ReadOnly] public Transform player;
    [ReadOnly] public Vector2 playerMoveDirection;
    [SerializeField, ReadOnly] private int playerLevel = 1;
    [SerializeField, ReadOnly] private int playerExp;
    [SerializeField, ReadOnly] private int ExpToUpLevel = 10;
    public event Action<int> OnUpLevel;
    public event Action<int> OnIncreaseExp;

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
}
