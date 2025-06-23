using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class RemovalPrompt : MonoBehaviour {
    public static RemovalPrompt Instance;
    
    [Header("Prompt UI Elements")]
    public GameObject promptPanel; // The panel containing the prompt.
    public TMP_Text promptMessage;
    public Button confirmButton;
    public Button cancelButton;

    private Action confirmCallback;
    private Action cancelCallback;

    void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (promptPanel != null)
            promptPanel.SetActive(false);
    }

    public void ShowPrompt(string message, Action onConfirm, Action onCancel) {
        if (promptPanel == null)
            return;
        promptMessage.text = message;
        confirmCallback = onConfirm;
        cancelCallback = onCancel;
        promptPanel.SetActive(true);

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(Confirm);
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(Cancel);
    }

    public void Confirm() {
        promptPanel.SetActive(false);
        confirmCallback?.Invoke();
    }

    public void Cancel() {
        promptPanel.SetActive(false);
        cancelCallback?.Invoke();
    }
}
