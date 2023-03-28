using Unity.Netcode;
using UnityEngine;

public class AccelerometerInput : NetworkBehaviour
{
    [Header("Componentes")]
    [SerializeField] Rigidbody rgbd;
    [SerializeField] PlayerDataNetwork playerData;

    [Header("Variables")]
    [SerializeField] float forceSpeed;

    [Header("Datos Temporales")]
    [SerializeField] Vector3 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        if (IsOwner && IsClient)
            TableMap.Instance.AddSquareToData();
        InitComponents();
        if(IsOwner)
        TableMap.Instance.AddNewPlayer();

        playerPos = transform.position;
    }

    void InitComponents() {
        if (rgbd == null)
            rgbd.GetComponent<Rigidbody>();
        if (playerData == null)
            playerData.GetComponent<PlayerDataNetwork>();
    }

    public override void OnNetworkSpawn() {
        /*if(IsClient) {
            TableMap.Instance.AddSquareToData();
        }*/
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("gameOver")) {
            Debug.Log("Perdio el jugador: " + OwnerClientId);
            gameObject.SetActive(false);
            //Interfaz indicando jugador caido
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsOwner) return;
        GetAccelerometer();
    }

    [ServerRpc(RequireOwnership = false)]
    void SetPositionServerRpc() {
        transform.position = playerData.GetPositionSpawn();
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
