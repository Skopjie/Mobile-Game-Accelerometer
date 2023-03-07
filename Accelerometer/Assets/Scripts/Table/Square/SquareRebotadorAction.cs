using UnityEngine;

public class SquareRebotadorAction : MonoBehaviour, ISquareActions
{
    [Header("Componentes")]
    [SerializeField] GameObject RebotadorGO;
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
        RebotadorGO.SetActive(true);
    }

    public void ReturnSquareToNormal()
    {
        RebotadorGO.SetActive(false);
        squareController.SetNewMaterial(dataProject.normalSquareMaterial);
    }

    public Material GetMaterialSquare()
    {
        return dataProject.rebotadorSquareMaterial;
    }
}
