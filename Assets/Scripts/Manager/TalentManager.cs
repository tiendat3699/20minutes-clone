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
    }

    private void OnEnable() {
        gameManager.OnSetPlayer += Init;
        gameManager.OnUpLevel += ShowUpgrade;
        TalentSelector.OnClick += SelectTalent;
    }

    private void OnDisable() {
        gameManager.OnSetPlayer -= Init;
        gameManager.OnUpLevel -= ShowUpgrade;
        TalentSelector.OnClick -= SelectTalent;
    }

    private void Init(Transform player) {
        this.player = player;
        playerStats = player.GetComponent<PlayerStats>();
    }

    private void ShowUpgrade(int playerExp, int lv, int maxExp) {
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
        ResetTalentList();
        if(talent != null) {
            talentActived.Add(talent);
            talentAvaiable.Remove(talent);
            switch(talent.type) {
                case TalentType.AddTalent:
                    Instantiate(talent.talentPrefabs, player, false);
                    break;
                case TalentType.StatsUpgrade:
                    playerStats.Upgrade(talent);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"talent type {talent.type} is invalid ");
            }
        }
        
        gameManager.ResumeGame();
        upgradePopUp.SetActive(false);
    }

    private void ResetTalentList() {
        for(int i = 0; i < talentDisplayed.Count; i ++) {
            talentAvaiable.Add(talentDisplayed[i]);
        }
        talentDisplayed.Clear();
    }
}
