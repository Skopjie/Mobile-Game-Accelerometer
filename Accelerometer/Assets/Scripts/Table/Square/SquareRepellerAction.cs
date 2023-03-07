using UnityEngine;

public class SquareRepellerAction : MonoBehaviour, ISquareActions
{
    [Header("Componentes")]
    [SerializeField] GameObject RepellerGO;
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
        RepellerGO.SetActive(true);
    }

    public void ReturnSquareToNormal()
    {
        RepellerGO.SetActive(false);
        squareController.SetNewMaterial(dataProject.normalSquareMaterial);
    }

    public Material GetMaterialSquare()
    {
        return dataProject.repellerSquareMaterial;
    }
}
