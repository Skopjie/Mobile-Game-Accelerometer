using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [Header("Botones")]
    [SerializeField] Button playOneButton;


    private void Awake() {
        playOneButton.onClick.AddListener(() => {
            LobbyUIController.Instance.StartGameCamera();
        });
    }

    public void HideCanvas() {
        gameObject.SetActive(false);
    }
    public void ShowCanvas() {
        gameObject.SetActive(true);
    }
}
