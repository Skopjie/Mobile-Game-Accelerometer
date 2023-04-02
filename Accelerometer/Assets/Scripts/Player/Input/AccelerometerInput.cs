using Unity.Netcode;
using UnityEngine;

public class AccelerometerInput : NetworkBehaviour
{
    [Header("Componentes")]
    [SerializeField] Rigidbody rgbd;
    [SerializeField] PlayerDataNetwork playerData;
    [SerializeField] MeshRenderer meshRender;

    [Header("Variables")]
    [SerializeField] float forceSpeed;
    [SerializeField] bool canMove;

    [Header("Datos Temporales")]
    [SerializeField] Vector3 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        InitComponents();

        GameStateManager.Instance.OnRoundIsOver += SetPosition;
        GameStateManager.Instance.OnAllPlayerConnected += SetPosition;
        GameStateManager.Instance.OnRoundStart += ActiveMovement;

        playerPos = transform.position;
    }

    void InitComponents() {
        if (rgbd == null)
            rgbd.GetComponent<Rigidbody>();

        if (meshRender == null)
            meshRender.GetComponent<MeshRenderer>();

        if (playerData == null)
            playerData.GetComponent<PlayerDataNetwork>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("gameOver")) {
            Debug.Log("Perdio el jugador: " + OwnerClientId);

            if (IsHost)
                GameStateManager.Instance.PlayerEliminated();

            Invoke("CheckFinalGame", 0.5f);
            //Interfaz indicando jugador caido
        }
    }

    void CheckFinalGame() {
        GameStateManager.Instance.CheckNumberOfPlayersClientRpc();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsOwner) return;
        GetAccelerometer();
    }

    void ActiveMovement(object sender, GameStateManager.GameStateEventArgs e) {
        rgbd.isKinematic = false;
    }

    void DesactiveMovement() {
        rgbd.isKinematic = true;
    }

    void SetPosition(object sender, GameStateManager.GameStateEventArgs e) {
        if (IsOwner) {
            rgbd.velocity = new Vector3(0, 0, 0);
            transform.position = playerData.GetPositionSpawn();
            DesactiveMovement();
        }
    }

    void GetAccelerometer()
    {
        Vector3 tilt = Input.acceleration * forceSpeed;
        tilt = Quaternion.Euler(90, 0, 0) * tilt;

        rgbd.AddForce(tilt);
    }

    public void ReturnPlayer()
    {
        rgbd.velocity = new Vector3(0, 0, 0);
        transform.position = playerPos;
    }
}
