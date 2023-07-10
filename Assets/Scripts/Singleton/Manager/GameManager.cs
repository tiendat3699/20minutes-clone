using System;
using UnityEngine;
using MyCustomAttribute;

public class GameManager : Singleton<GameManager>
{
    public float countdownTime;
    [ReadOnly] public Transform player;
    [ReadOnly] public Vector2 playerMoveDirection;
    private int playerLevel = 1;
    private int ExpToUpLevel = 10;
    private int playerExp;
    private int killCount;
    private float timer;
    [ReadOnly] public bool isDead;
    private bool pause;
    public event Action<int, int> OnUpLevel;
    public event Action<int> OnIncreaseExp;
    public event Action<int> OnIncreaseKill;
    public event Action<float> OnUpdateTime;
    public event Action OnPause;
    public event Action OnResume;

    private void Start() {
        Init();
    }

    private void Update() {
        if(!pause) {
            timer -= Time.deltaTime;
            OnUpdateTime?.Invoke(timer);
        }
    }

    private void Init() {
        timer = countdownTime;
        OnUpLevel?.Invoke(playerLevel, ExpToUpLevel);
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

    public void PauseGame() {
        Time.timeScale = 0;
        OnPause?.Invoke();
    }


    public void ResumeGame() {
        Time.timeScale = 1;
        OnResume?.Invoke();
    }

}
