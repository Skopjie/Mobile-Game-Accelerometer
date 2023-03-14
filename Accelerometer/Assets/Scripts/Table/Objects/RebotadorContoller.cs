using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RebotadorContoller : MonoBehaviour, IObject
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
            AplyForceToObject(collision.gameObject.GetComponent<Rigidbody>());
    }

    public void EnableObject()
    {
        gameObject.SetActive(true);
    }

    public void DisableObject()
    {
        gameObject.SetActive(false);
    }

    void AplyForceToObject(Rigidbody objectRgbd)
    {
        objectRgbd.AddExplosionForce(explosionForce, transform.position, explosionRadioForce, 0, ForceMode.Impulse);
    }

}
