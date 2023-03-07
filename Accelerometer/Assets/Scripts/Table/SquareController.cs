using System.Collections;
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
    public Vector3 squareInitialPosition;
    [SerializeField] ISquareActions squareActions;

    [SerializeField]float timeToChangeTypeSquare;
    TypeSquare typeSquare;

    [Header("Componentes")]
    [SerializeField] MeshRenderer meshRender;
    public Rigidbody rgbd;

    private void Start()
    {
        InitComponents();
    }

    void InitComponents()
    {
        if(meshRender == null)
            meshRender = gameObject.GetComponent<MeshRenderer>();

        if(rgbd == null)
            rgbd = gameObject.GetComponent<Rigidbody>();
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

    public void SetNewMaterial(Material newMaterial)
    {
        meshRender.material = newMaterial;
    }

    public void CallAndChangeTypeSquareAction(TypeSquare newType)
    {
        SetTypeSquare(newType);
        CallSquareIsChangingTypeAction();
    }

    public void SetTypeSquare(TypeSquare newType)
    {
        typeSquare = newType;

        switch(typeSquare)
        {
            case TypeSquare.fall:
                squareActions = GetComponent<SquareFallAction>();
                break;

            case TypeSquare.rebotador:
                squareActions = GetComponent<SquareRepellerAction>();
                break;

            case TypeSquare.repellers:
                squareActions = GetComponent<SquareRepellerAction>();
                break;
        }
    }

    public void CallSquareIsChangingTypeAction()
    {
        StartCoroutine(SquareIsChangingType());
    }



    IEnumerator SquareIsChangingType()
    {
        meshRender.material = squareActions.GetMaterialSquare();
        yield return new WaitForSeconds(timeToChangeTypeSquare);
        squareActions.CallActionSquare();
    }

    public void ReturnSquareToNormalState()
    {
        squareActions.ReturnSquareToNormal();
        squareActions = null;
    }
}
