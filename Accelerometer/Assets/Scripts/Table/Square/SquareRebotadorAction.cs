using UnityEngine;

public class SquareRebotadorAction : MonoBehaviour, ISquareActions
{
    [Header("Componentes")]
    [SerializeField] RebotadorContoller RebotadorGO;
    [SerializeField] SquareController squareController;

    [Header("Variables Importantes")]
    [SerializeField] DataProjectSO dataProject;

    private void Start()
    {
        InitSquareData();
    }

    public void InitSquareData()
    {
        if (squareController == null)
            squareController = gameObject.GetComponent<SquareController>();
    }

    public void CallActionSquare()
    {
        RebotadorGO.EnableObject();
    }

    public void ReturnSquareToNormal()
    {
        RebotadorGO.DisableObject();
        squareController.SetNewMaterial(dataProject.normalSquareMaterial);
    }

    public Material GetMaterialSquare()
    {
        return dataProject.rebotadorSquareMaterial;
    }
}
