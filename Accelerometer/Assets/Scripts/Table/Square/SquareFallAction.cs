using UnityEngine;

public class SquareFallAction : MonoBehaviour, ISquareActions
{
    [Header("Componentes")]
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
        squareController.rgbd.isKinematic = false;
        squareController.rgbd.useGravity = true;
    }

    public void ReturnSquareToNormal()
    {
        squareController.rgbd.isKinematic = true;
        squareController.rgbd.useGravity = false;
        gameObject.transform.position = squareController.squareInitialPosition.Value;
        squareController.SetNewMaterial(dataProject.normalSquareMaterial);
    }

    public Material GetMaterialSquare()
    {
        return dataProject.fallSquareMaterial;
    }
}
