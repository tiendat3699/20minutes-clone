using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DebugLogText : MonoBehaviour
{
    private Text textlog;
    private GameManager gameManager;
    private void Awake() {
        gameManager = GameManager.Instance;
        textlog = GetComponent<Text>();
    }

    private void OnEnable() {
        gameManager.OnDebugLog += SetLog;
    }

    private void OnDisable() {
        gameManager.OnDebugLog -= SetLog;
    }


    private void SetLog(string content) {
        textlog.text = content;
    }
}
