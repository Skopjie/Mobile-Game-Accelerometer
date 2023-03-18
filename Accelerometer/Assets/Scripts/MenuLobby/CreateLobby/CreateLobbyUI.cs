using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreateLobbyUI : MonoBehaviour
{

    const string PRIVATETEXT = "Private";
    const string PUBLICTEXT = "Public";
    int numberPlayer = 2;

    [Header("Textos")]
    [SerializeField] TextMeshProUGUI protectionLobbyText;
    [SerializeField] TextMeshProUGUI numPlayerText;
    [SerializeField] TMP_InputField inputNameLobbyText;

    public void ChangeTypeProtectionLobby()
    {
        if (protectionLobbyText.text == PUBLICTEXT){
            protectionLobbyText.text = PRIVATETEXT;
            LobbyController.Instance.SetProtectionLobby(true);
        }
        else{
            protectionLobbyText.text = PUBLICTEXT;
            LobbyController.Instance.SetProtectionLobby(false);
        }
    }

    public void ChangeNumPlayerLobby()
    {
        numberPlayer++;
        if (numberPlayer == 5)
            numberPlayer = 2;

        LobbyController.Instance.SetNumerPlayerLobby(numberPlayer);
        numPlayerText.text = ""+numberPlayer;
    }

    public void ChangeNameLobby(string newNameLobby)
    {
        LobbyController.Instance.SetNameLobby(newNameLobby);
    }

    public void ResetLobbyUIData()
    {
        inputNameLobbyText.text = null;
        LobbyController.Instance.SetNameLobby(null);

        numberPlayer = 2;
        LobbyController.Instance.SetNumerPlayerLobby(numberPlayer);
        numPlayerText.text = "" + numberPlayer;

        protectionLobbyText.text = PUBLICTEXT;
        LobbyController.Instance.SetProtectionLobby(false);
    }
}
