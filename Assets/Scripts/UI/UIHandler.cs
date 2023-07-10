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
    [SerializeField] private Transform healthHolder;
    [SerializeField] private Transform healthDisableHolder;
    [SerializeField] private GameObject healthPrefab;
    [SerializeField] private GameObject healthDisablePrefab;
    [SerializeField] private GameObject GameOverPopUp;
    private List<GameObject> healthList;
    private List<GameObject> healthDisableList;
    private GameManager gameManager;
    private PlayerDamageable playerDamageable;
    private PlayerStats playerStats;

    private void Awake() {
        gameManager = GameManager.Instance;
        healthList = new List<GameObject>();
        healthDisableList = new List<GameObject>();
    }

    private void Start() {        
        UpdateMaxHealth(playerStats.maxHp);
        UpdateHealth(playerStats.maxHp);
    }

    private void OnEnable() {
        playerDamageable = gameManager.player.GetComponent<PlayerDamageable>();
        playerStats = gameManager.player.GetComponent<PlayerStats>();

        gameManager.OnIncreaseExp += HandleSliderExp;
        gameManager.OnUpLevel += HandleLevelUp;
        playerDamageable.OnHit += UpdateHealth;
        playerStats.OnUpgrade += UpdateMaxHealth;
        playerDamageable.OnDead += HandlePlayeDead;
        gameManager.OnUpdateTime += HandleUpdateTime;
    }

    private void OnDisable() {
        gameManager.OnIncreaseExp -= HandleSliderExp;
        gameManager.OnUpLevel -= HandleLevelUp;
        playerDamageable.OnHit -= UpdateHealth;
        playerStats.OnUpgrade -= UpdateMaxHealth;
        playerDamageable.OnDead -= HandlePlayeDead;
        gameManager.OnUpdateTime -= HandleUpdateTime;
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
        }
    }

    private void UpdateMaxHealth(int maxHp) {
        while(healthDisableList.Count < maxHp) {
            GameObject health = Instantiate(healthPrefab, healthHolder, false);
            health.SetActive(false);
            healthList.Add(health);
            GameObject healthDisable = Instantiate(healthDisablePrefab, healthDisableHolder, false);
            healthDisableList.Add(healthDisable);
        }
    }

    
    private void UpdateMaxHealth(PlayerStats playerStats) {
        UpdateMaxHealth(playerStats.maxHp);
    }

    private void HandlePlayeDead() {
        Invoke(nameof(ShowGameOver), 1);
    }

    private void ShowGameOver() {
        gameManager.PauseGame();
        GameOverPopUp.SetActive(true);
    }

    private void HandleUpdateTime(float time) {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        timeText.text = DateTime.Today.Add(timeSpan).ToString("mm:ss");
    }
}
