using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class SquareController : NetworkBehaviour {
    [Header("Datos Importantes")]
    public NetworkVariable<Vector2> idSquare =
    new NetworkVariable<Vector2>(new Vector2(0,0),
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    public NetworkVariable<Vector3> squareInitialPosition =
    new NetworkVariable<Vector3>(new Vector3(0, 0,0),
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

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

    public Vector2 GetIdSquare() {
        return idSquare.Value;
    }
    public void SetPositionSquare(Vector3 newSquareInitialPosition) {
        squareInitialPosition.Value = newSquareInitialPosition;
    }
    public void InitDataSquare(Vector2 newId, Vector3 newSquareInitialPosition)
    {
        idSquare.Value = newId;
        squareInitialPosition.Value = newSquareInitialPosition;
    }
    public void InitIdSquare(Vector2 newId) {
        idSquare.Value = newId;
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
                squareActions = GetComponent<SquareRebotadorAction>();
                break;

            case TypeSquare.repellers:
                squareActions = GetComponent<SquareRepellerAction>();
                break;

            case TypeSquare.attractor:
                squareActions = GetComponent<SquareAttractorAction>();
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
