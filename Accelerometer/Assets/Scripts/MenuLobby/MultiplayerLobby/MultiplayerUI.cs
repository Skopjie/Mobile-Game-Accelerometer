using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MultiplayerUI: MonoBehaviour
{
    public static MultiplayerUI Instance { get; private set; }


    [Header("Botones")]
    [SerializeField] Button createLobbyButton;
    [SerializeField] Button joinPublicButton;
    [SerializeField] Button joinPrivateButton;

    private void Awake() {
        Instance = this;

        createLobbyButton.onClick.AddListener(() => {
            HideCanvas();
        });

        joinPublicButton.onClick.AddListener(() => {
            HideCanvas();
        });

        joinPrivateButton.onClick.AddListener(() => {
            HideCanvas();
        });
    }

    public void HideCanvas() {
        gameObject.SetActive(false);
    }

    public void ShowCanvas() {
        gameObject.SetActive(true);
    }

}
