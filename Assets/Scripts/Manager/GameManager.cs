using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using MyCustomAttribute;

public class GameManager : PersistentSingleton<GameManager>
{
    public Transform player {get; private set;}
    [ReadOnly] public Vector2 playerMoveDirection;
    private int playerLevel = 1;
    private int ExpToUpLevel = 10;
    private int playerExp;
    private int killCount;
    public event Action<Transform> OnSetPlayer;
    public event Action<int, int> OnUpLevel;
    public event Action<int> OnIncreaseExp;
    public event Action<int> OnIncreaseKill;
    public event Action OnPause;
    public event Action OnResume;
    public event Action OnWin;
    public event Action OnReset;

    //use for display log on game screen
    public event Action<string> OnDebugLog;

    private void Start() {
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += Init;
    }

    private void Init(Scene scene, LoadSceneMode loadSceneMode) {
        OnUpLevel?.Invoke(playerLevel, ExpToUpLevel);
    }

    public void SetPlayer(Transform player) {
        this.player = player;
        OnSetPlayer?.Invoke(player);
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
    
    public void Win() {
        OnWin?.Invoke();
    }

    public void SetDebugLog(string content) {
        OnDebugLog?.Invoke(content);
    }

    public void ResetGame() {
        playerExp = 0;
        ExpToUpLevel = 10;
        playerLevel = 0;
        killCount = 0;
        Time.timeScale = 1;
        OnReset?.Invoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
