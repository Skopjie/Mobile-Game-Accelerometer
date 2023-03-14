using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractorController : MonoBehaviour, IObject
{
    [SerializeField] float forceFactor;
    [SerializeField] List<Rigidbody> bodies = new List<Rigidbody>();
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<AccelerometerInput>())
            bodies.Add(other.GetComponent<Rigidbody>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<AccelerometerInput>())
            bodies.Remove(other.GetComponent<Rigidbody>());
    }

    private void FixedUpdate()
    {
        CallAction();
    }

    public void CallAction()
    {
        foreach (Rigidbody rgbd in bodies)
        {
            rgbd.AddForce((transform.position - rgbd.position) * forceFactor * Time.fixedDeltaTime);
        }
    }

    public void EnableObject()
    {
        gameObject.SetActive(true);
    }

    public void DisableObject()
    {
        gameObject.SetActive(false);
        bodies.Clear();
    }
}
