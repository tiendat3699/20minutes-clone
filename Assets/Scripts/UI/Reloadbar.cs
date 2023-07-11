using UnityEngine;
using UnityEngine.UI;

public class Reloadbar : MonoBehaviour
{
    [SerializeField] private Slider slider;


    private void Start() {
        slider.gameObject.SetActive(false);
    }

    private void OnEnable() {
        PlayerAttackHandler.OnReload += UpdateSlider;
    }

    private void OnDisable() {
        PlayerAttackHandler.OnReload -= UpdateSlider;
    }

    private void UpdateSlider(float value) {
        slider.gameObject.SetActive(value > 0 && value < 1);
        slider.value = value;
    }
}
