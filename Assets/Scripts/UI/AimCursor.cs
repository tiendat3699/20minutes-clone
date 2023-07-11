using UnityEngine;
using UnityEngine.UI;

public class AimCursor : MonoBehaviour
{
    [SerializeField] private Text ammoText;

    private void Start() {
        Cursor.visible = false;
    }

    private void OnEnable() {
        PlayerAttackHandler.OnAmmoUpdate += UpdateAmmoText;
    }

    private void OnDisable() {
        PlayerAttackHandler.OnAmmoUpdate -= UpdateAmmoText;
    }

    private void UpdateAmmoText(int ammo) {
        ammoText.text = ammo.ToString();
    }

    private void Update() {
        transform.position = Input.mousePosition;
        if(Input.GetMouseButtonDown(0)) {
            Cursor.visible = false;
        }

        if(Input.GetButton("Cancel")) {
            Cursor.visible = true;
        }
    }
}
