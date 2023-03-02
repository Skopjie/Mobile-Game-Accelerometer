using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerometerInput : MonoBehaviour
{
    Rigidbody rgbd;

    [SerializeField] float forceSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rgbd = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GetAccelerometer();
    }

    void GetAccelerometer()
    {
        Vector3 tilt = Input.acceleration * forceSpeed;
        tilt = Quaternion.Euler(90, 0, 0) * tilt;

        rgbd.AddForce(tilt);
    }
}
