using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeSquare
{
    normal,
    fall,
    rebotador,
    repellers
}

public class SquareController : MonoBehaviour
{
    [Header("Datos Importantes")]
    [SerializeField]Vector2 idSquare;
    Vector3 squareInitialPosition;

    [SerializeField]float timeToChangeTypeSquare;
    TypeSquare typeSquare;

    [Header("Materiales")]
    [SerializeField] Material normalSquareMaterial;
    [SerializeField] Material fallSquareMaterial;
    [SerializeField] Material rebotadorSquareMaterial;
    [SerializeField] Material repellerSquareMaterial;

    [Header("Componentes")]
    [SerializeField] MeshRenderer meshRender;
    [SerializeField] Rigidbody rgbd;
    [SerializeField] GameObject RebotadorGO;
    [SerializeField] GameObject RepellerGO;

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

        /*if (RebotadorGO == null)
            RebotadorGO = this.gameObject.GetComponent<Rigidbody>();*/
    }

    public void InitDataSquare(Vector2 newId, Vector3 newSquareInitialPosition)
    {
        idSquare = newId;
        squareInitialPosition = newSquareInitialPosition;
    }



    public void SetTimeSquareCallAction(float newTime)
    {
        timeToChangeTypeSquare = newTime;
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
        //meshRender.material = materialNormal;
    }

    Material GetActualTypeSquareMaterial()
    {
        switch (typeSquare)
        {
            case TypeSquare.fall:
                return fallSquareMaterial;

            case TypeSquare.rebotador:
                return rebotadorSquareMaterial;

            case TypeSquare.repellers:
                return repellerSquareMaterial;

            default:
                return normalSquareMaterial;
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
        typeSquare = TypeSquare.normal;
        meshRender.material = normalSquareMaterial;

        rgbd.isKinematic = true;
        rgbd.useGravity = false;
        gameObject.transform.position = squareInitialPosition;

        RebotadorGO.SetActive(false);
    }
}
