using UnityEngine;

public class SquareAttractorAction : MonoBehaviour, ISquareActions
{
    [Header("Componentes")]
    [SerializeField] AttractorController attractorController;
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
        attractorController.EnableObject();
    }

    public void ReturnSquareToNormal()
    {
        attractorController.DisableObject();
        squareController.SetNewMaterial(dataProject.normalSquareMaterial);
    }

    public Material GetMaterialSquare()
    {
        return dataProject.attractorSquareMaterial;
    }
}


