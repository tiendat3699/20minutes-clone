using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TalentSelector : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Text descText;
    [SerializeField] private Image image;
    [SerializeField] private Sprite defaultImage;
    private Button button;
    private TalentBase talent;
    public static event Action<TalentBase> OnClick;

    private void Awake() {
        button =  GetComponent<Button>();
    }

    private void OnEnable() {
        button.onClick.AddListener(HandleClick);
    }

    private void HandleClick() {
        OnClick?.Invoke(talent);
    }

    private void OnDisable() {
        button.onClick.RemoveListener(HandleClick);
        nameText.text = "Not Avaiable";
        descText.text = "Not Avaiable";
        image.sprite = defaultImage;
        talent = null;
    }

    public void SetData(TalentBase talent) {
        this.talent = talent;
        nameText.text = talent.name;
        descText.text = talent.description;
        image.sprite = talent.sprite != null ? talent.sprite : defaultImage;
    }
}
