using Unity.Netcode;
using UnityEngine;

public class AccelerometerInput : NetworkBehaviour
{
    Rigidbody rgbd;

    [SerializeField] float forceSpeed;

    [Header("Datos Temporales")]
    [SerializeField] Vector3 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        rgbd = GetComponent<Rigidbody>();

        playerPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsOwner) return;
        GetAccelerometer();
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
