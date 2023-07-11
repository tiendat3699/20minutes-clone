using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private Slider sliderExp;
    [SerializeField] private Text levelText;
    [SerializeField] private Text timeText;
    [SerializeField] private Text ammoText;
    [SerializeField] private Transform healthHolder;
    [SerializeField] private Transform healthDisableHolder;
    [SerializeField] private GameObject healthPrefab;
    [SerializeField] private GameObject healthDisablePrefab;
    [SerializeField] private GameObject GameOverPopUp;
    [SerializeField] private GameObject WinGamePopUp;
    private List<GameObject> healthList;
    private Queue <GameObject> healthActiveQueue;
    private Queue<GameObject> healthDisableQueue;
    private GameManager gameManager;
    private PlayerDamageable playerDamageable;
    private PlayerStats playerStats;

    private void Awake() {
        gameManager = GameManager.Instance;
        healthList = new List<GameObject>();
        healthActiveQueue = new Queue<GameObject>();
        healthDisableQueue = new Queue<GameObject>();
    }

    private void Start() {        
        UpdateMaxHealth(playerStats.maxHp);
    }

    private void OnEnable() {
        playerDamageable = gameManager.player.GetComponent<PlayerDamageable>();
        playerStats = gameManager.player.GetComponent<PlayerStats>();

        gameManager.OnIncreaseExp += HandleSliderExp;
        gameManager.OnUpLevel += HandleLevelUp;
        gameManager.OnWin += HandleWinGame;
        playerDamageable.OnHit += UpdateHealth;
        playerStats.OnUpgrade += UpdateMaxHealth;
        playerDamageable.OnDead += HandlePlayeDead;
        WaveManager.Instance.OnCountDown += HandleUpdateTime;
        PlayerAttackHandler.OnAmmoUpdate += HandleAmmoChange;
    }

    private void HandleAmmoChange(int ammo)
    {
        ammoText.text = $"{ammo}/{playerStats.ammo}";
    }

    private void OnDisable() {
        gameManager.OnIncreaseExp -= HandleSliderExp;
        gameManager.OnUpLevel -= HandleLevelUp;
        gameManager.OnWin -= HandleWinGame;
        PlayerAttackHandler.OnAmmoUpdate -= HandleAmmoChange;
    }

    private void HandleSliderExp(int exp) {
        sliderExp.value = exp;
    }

    private void HandleLevelUp(int lv, int maxExp) {
        sliderExp.value = 0;
        sliderExp.maxValue = maxExp;
        levelText.text = "Lv." + lv.ToString();
    }

    private void UpdateHealth(int hp) {
        for(int i = 0; i < healthList.Count; i++) {
            bool active = i < hp;
            healthList[i].SetActive(active);
            healthActiveQueue.Clear();
            if(active) {
                healthActiveQueue.Enqueue(healthList[i]);
            }
        }
    }

    private void UpdateMaxHealth(int maxHp) {
        while(healthDisableQueue.Count < maxHp) {
            GameObject health = Instantiate(healthPrefab, healthHolder, false);
            healthList.Add(health);
            GameObject healthDisable = Instantiate(healthDisablePrefab, healthDisableHolder, false);
            healthDisableQueue.Enqueue(healthDisable);
        }

        while(healthDisableQueue.Count > maxHp) {
            GameObject obj = healthDisableQueue.Dequeue();
            Destroy(obj);
            healthList.Remove(obj);
        }

        while(healthActiveQueue.Count > maxHp) {
            Debug.Log(healthActiveQueue.Count);
            Destroy(healthActiveQueue.Dequeue());
        }
    }

    
    private void UpdateMaxHealth(PlayerStats stats) {
        UpdateMaxHealth(stats.maxHp);
    }

    private void HandlePlayeDead() {
        Invoke(nameof(ShowGameOver), 1);
    }

    private void ShowGameOver() {
        gameManager.PauseGame();
        GameOverPopUp.SetActive(true);
    }

    private void HandleWinGame() {
        Invoke(nameof(ShowWinGame), 1.5f);
    }

    private void ShowWinGame() {
        gameManager.PauseGame();
        WinGamePopUp.SetActive(true);
    }

    private void HandleUpdateTime(float time) {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        timeText.text = DateTime.Today.Add(timeSpan).ToString("mm:ss");
    }

    public void ResetGame() {
        gameManager.ResetGame();
    }
}
