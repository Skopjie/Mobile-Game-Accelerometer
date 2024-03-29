using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerometerInput : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] Rigidbody rgbd;
    [SerializeField] MeshRenderer meshRender;

    [Header("Variables")]
    [SerializeField] float forceSpeed;

    void Start() {
        InitComponents();

        GameStateManager.Instance.OnRoundIsOver += SetPosition;
        GameStateManager.Instance.OnAllPlayerConnected += SetPosition;
        GameStateManager.Instance.OnRoundStart += ActiveMovement;
    }

    void FixedUpdate() {
        GetAccelerometer();
    }

    void InitComponents() {
        if (rgbd == null)
            rgbd = GetComponent<Rigidbody>();

        if (meshRender == null)
            meshRender = GetComponent<MeshRenderer>();
    }

    void ActiveMovement(object sender, GameStateManager.GameStateEventArgs e) {
        rgbd.isKinematic = false;
    }

    void DesactiveMovement() {
        rgbd.isKinematic = true;
    }

    void SetPosition(object sender, GameStateManager.GameStateEventArgs e) {
        rgbd.velocity = new Vector3(0, 0, 0);
        transform.position = new Vector3(2.1f, 0.6f, 5.3f);
        DesactiveMovement();
    }

    void GetAccelerometer() {
        Vector3 tilt = Input.acceleration * forceSpeed;
        tilt = Quaternion.Euler(90, 0, 0) * tilt;

        rgbd.AddForce(tilt);
    }
}
