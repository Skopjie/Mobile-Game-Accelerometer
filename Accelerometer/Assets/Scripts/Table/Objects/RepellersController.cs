using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepellersController : MonoBehaviour
{
    [SerializeField] float forceFactor;
    [SerializeField] List<Rigidbody> bodies = new List<Rigidbody>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        ApplyForce();
    }

    void ApplyForce()
    {
        foreach(Rigidbody rgbd in bodies)
        {
            rgbd.AddForce((this.transform.position - rgbd.position) * forceFactor * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<AccelerometerInput>())
            bodies.Add(other.GetComponent<Rigidbody>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<AccelerometerInput>())
            bodies.Remove(other.GetComponent<Rigidbody>());
    }
}
