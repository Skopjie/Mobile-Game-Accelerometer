using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casilla : MonoBehaviour
{
    [Header("Datos Importantes")]
    public Vector2 id;

    [Header("Componentes")]
    [SerializeField] Material materialSelected;
    [SerializeField] MeshRenderer meshRender;
    [SerializeField] Rigidbody rgbd;
    [SerializeField] GameObject RebotadorGO;

    private void Start()
    {
        meshRender = this.gameObject.GetComponent<MeshRenderer>();
        rgbd = this.gameObject.GetComponent<Rigidbody>();
    }

    public void SelectCasillaDisactive()
    {
        StartCoroutine(CasillaFall());
    }

    public void AddRebotadorCasilla()
    {
        RebotadorGO.SetActive(true);
    }

    IEnumerator CasillaFall()
    {
        meshRender.material = materialSelected;
        yield return new WaitForSeconds(2);
        FallOfCasilla();
    }

    //interfaz?
    public void DisableCasilla()
    {
        this.gameObject.SetActive(false);
    }

    public void FallOfCasilla()
    {
        rgbd.isKinematic = false;
        rgbd.useGravity = true;
    }
}
