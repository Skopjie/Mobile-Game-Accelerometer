using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeSquare
{
    normal,
    fall,
    rebotador
}

public class Casilla : MonoBehaviour
{
    [Header("Datos Importantes")]
    public Vector2 id;
    public Vector3 casillaPosition;
    public float timeToChangeTypeSquare;
    TypeSquare typeSquare;

    [Header("Materiales")]
    [SerializeField] Material materialNormal;
    [SerializeField] Material materialFall;
    [SerializeField] Material materialRebotador;

    [Header("Componentes")]
    [SerializeField] MeshRenderer meshRender;
    [SerializeField] Rigidbody rgbd;
    [SerializeField] GameObject RebotadorGO;

    private void Start()
    {
        InitComponents();
    }

    void InitComponents()
    {
        if(meshRender == null)
            meshRender = this.gameObject.GetComponent<MeshRenderer>();

        if(rgbd == null)
            rgbd = this.gameObject.GetComponent<Rigidbody>();
    }


    public void CallAndChangeTypeSquareAction(TypeSquare newType)
    {
        SetTypeSquare(newType);
        CallSquareIsChangingTypeAction();
    }

    public void SetTypeSquare(TypeSquare newType)
    {
        typeSquare = newType;
    }

    public void CallSquareIsChangingTypeAction()
    {
        StartCoroutine(SquareIsChangingType());
    }

    IEnumerator SquareIsChangingType()
    {
        meshRender.material = GetActualTypeSquareMaterial();
        yield return new WaitForSeconds(timeToChangeTypeSquare);
        CallSquareTypeAction();
    }

    Material GetActualTypeSquareMaterial()
    {
        switch (typeSquare)
        {
            case TypeSquare.fall:
                return materialFall;

            case TypeSquare.rebotador:
                return materialRebotador;

            default:
                return materialNormal;
        }
    }

    void CallSquareTypeAction()
    {
        switch (typeSquare)
        {
            case TypeSquare.fall:
                FallOfCasilla();
                break;

            case TypeSquare.rebotador:
                AddRebotadorCasilla();
                break;
        }
    }



    //interfaz?
    public void DisableCasilla()
    {
        this.gameObject.SetActive(false);
    }

    public void AddRebotadorCasilla()
    {
        RebotadorGO.SetActive(true);
    }

    public void FallOfCasilla()
    {
        rgbd.isKinematic = false;
        rgbd.useGravity = true;
    }

    public void ReturnCasillaInitialPosition()
    {
        meshRender.material = materialNormal;
        rgbd.isKinematic = true;
        rgbd.useGravity = false;
        gameObject.transform.position = casillaPosition;
        RebotadorGO.SetActive(false);
    }
}
