using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareFallAction : MonoBehaviour, ISquareActions
{
    [SerializeField] Rigidbody rgbd;
    [SerializeField] Vector3 squareInitialPosition;

    private void Start()
    {
        InitComponents();
    }

    void InitComponents()
    {
        if (rgbd == null)
            rgbd = GetComponent<Rigidbody>();
    }

    public void CallActionSquare()
    {
        rgbd.isKinematic = false;
        rgbd.useGravity = true;
    }

    public void ReturnSquareToNormal()
    {
        rgbd.isKinematic = true;
        rgbd.useGravity = false;
        gameObject.transform.position = squareInitialPosition;
    }
}
