using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GPManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI statusText;

    [SerializeField] Button hostBtn;
    [SerializeField] Button clientBtn;
    [SerializeField] bool inityRelay;



    private void Awake()
    {
        hostBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
        });

        clientBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
        });

        if(inityRelay)
            InitGame();
    }

    public void Start()
    {
        //PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    public void InitGame() {
        RelayController.Instance.StartRelayHostClient();
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        statusText.text = "Wait...";
        if (status == SignInStatus.Success)
        {
            statusText.text = "Good";
            nameText.text = Social.localUser.userName + " / " + Social.localUser.id;
            // Continue with Play Games Services
        }
        else
        {
            statusText.text = "Fail: " + status;
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
        }
    }


}
