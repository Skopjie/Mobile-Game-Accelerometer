using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rebotador : MonoBehaviour
{
    [SerializeField] float explosionForce;
    [SerializeField] float explosionRadioForce;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<AccelerometerInput>())
        {
            Debug.Log("Choque");
            AplyForceToObject(collision.gameObject.GetComponent<Rigidbody>());
        }
    }

    void AplyForceToObject(Rigidbody objectRgbd)
    {
        objectRgbd.AddExplosionForce(explosionForce, transform.position, explosionRadioForce);
    }
}
