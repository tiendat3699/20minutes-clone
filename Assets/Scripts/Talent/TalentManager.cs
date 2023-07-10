using System;
using System.Collections.Generic;
using UnityEngine;
using MyCustomAttribute;
using Random = UnityEngine.Random;

public class TalentManager : MonoBehaviour
{
    public TalentBase[] talents;
    [SerializeField] private GameObject upgradePopUp;
    [SerializeField] private Transform upgradeContainer;
    [SerializeField, ReadOnly] private List<TalentBase> talentDisplayed;
    [SerializeField, ReadOnly] private List<TalentBase> talentActived;
    [SerializeField, ReadOnly] private List<TalentBase> talentAvaiable;
    private Transform player;
    private PlayerStats playerStats;
    private GameManager gameManager;

    private void Awake() {
        gameManager =  GameManager.Instance;
        talentDisplayed = new List<TalentBase>();
        talentActived = new List<TalentBase>();
        talentAvaiable = new List<TalentBase>(talents);
    }

    private void Start() {
        player = gameManager.player;
        playerStats = player.GetComponent<PlayerStats>();
    }

    private void OnEnable() {
        gameManager.OnUpLevel += ShowUpgrade;
        TalentSelector.OnClick += SelectTalent;
    }

    private void OnDisable() {
        gameManager.OnUpLevel -= ShowUpgrade;
        TalentSelector.OnClick -= SelectTalent;
    }

    private void ShowUpgrade(int lv, int maxExp) {
        if(lv > 1) {
            gameManager.PauseGame();
            //display random talent
            for(int i = 0; i < 3; i++) {
                if(talentAvaiable.Count > 0) {
                    int random = Random.Range(0, talentAvaiable.Count);
                    TalentBase talent = talentAvaiable[random];
                    talentDisplayed.Add(talent);
                    talentAvaiable.Remove(talent);
                    upgradeContainer.GetChild(i).GetComponent<TalentSelector>().SetData(talent);
                }
            }

            upgradePopUp.SetActive(true);
        }

    }

    private void SelectTalent(TalentBase talent) {
        if(talent == null) return;
        ResetTalentList();
        talentActived.Add(talent);
        talentAvaiable.Remove(talent);
        upgradePopUp.SetActive(false);
        gameManager.ResumeGame();
        switch(talent.type) {
            case TalentType.AddWeapon:
                
                break;
            case TalentType.StatsUpgrade:
                playerStats.Upgrade(talent);
                break;
            default:
                throw new ArgumentOutOfRangeException($"talent type {talent.type} is invalid ");
        }
    }

    private void ResetTalentList() {
        for(int i = 0; i < talentDisplayed.Count; i ++) {
            talentAvaiable.Add(talentDisplayed[i]);
        }
        talentDisplayed.Clear();
    }
}
