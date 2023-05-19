using System.Collections;
using UnityEngine;

public class SquareController : MonoBehaviour {
    [Header("Datos Importantes")]
    [SerializeField] Vector2 idSquare;
    [SerializeField] Vector3 squareInitialPosition;
    [SerializeField] ISquareActions squareActions;

    [SerializeField]float timeToChangeTypeSquare;
    TypeSquare typeSquare;

    [Header("Componentes")]
    [SerializeField] MeshRenderer meshRender;
    [SerializeField] SquareNetwork squareNetwork;
    public Rigidbody rgbd;

    private void Start()
    {
        InitComponents();
    }

    void InitComponents()
    {
        if (squareNetwork == null)
            squareNetwork = gameObject.GetComponent<SquareNetwork>();

        if (meshRender == null)
            meshRender = gameObject.GetComponent<MeshRenderer>();

        if(rgbd == null)
            rgbd = gameObject.GetComponent<Rigidbody>();
    }

    public Vector2 GetIdSquare() {
        if (GameStateManager.Instance.isInMultiplayer)
            return squareNetwork.GetIdSquare();
        else
            return idSquare;
    }
    public Vector3 GetInitialPositionSquare() {
        if (GameStateManager.Instance.isInMultiplayer) {
            return squareNetwork.GetInitialPositionSquare();
        }
        else
            return squareInitialPosition;
    }
    public void SetPositionSquare(Vector3 newSquareInitialPosition) {
        if (GameStateManager.Instance.isInMultiplayer)
            squareNetwork.SetPositionSquare(newSquareInitialPosition);
        else
            squareInitialPosition = newSquareInitialPosition;
    }
    public void SetIdSquare(Vector2 newId) {
        if (GameStateManager.Instance.isInMultiplayer)
            squareNetwork.SetIdSquare(newId);
        else
            idSquare = newId;
    }
    public void InitDataSquare(Vector2 newId, Vector3 newSquareInitialPosition) {
        if (GameStateManager.Instance.isInMultiplayer) {
            squareInitialPosition = newSquareInitialPosition;
            idSquare = newId;
            squareNetwork.InitDataSquare(newId, newSquareInitialPosition);
        }
        else {
            SetPositionSquare(newSquareInitialPosition);
            SetIdSquare(newId);
        }
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
