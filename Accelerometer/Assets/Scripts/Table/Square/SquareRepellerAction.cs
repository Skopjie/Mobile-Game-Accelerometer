using Unity.Netcode;
using UnityEngine;

public class SquareRepellerAction : MonoBehaviour, ISquareActions
{
    [Header("Componentes")]
    [SerializeField] RepellersController repellerController;
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
        repellerController.EnableObject();
    }

    public void ReturnSquareToNormal()
    {
        repellerController.DisableObject();
        squareController.SetNewMaterial(dataProject.normalSquareMaterial);
    }

    public Material GetMaterialSquare()
    {
        return dataProject.repellerSquareMaterial;
    }
}
