using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareRebotadorAction : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] GameObject RebotadorGO;

    private void Start()
    {
        InitComponents();
    }

    void InitComponents()
    {
    }

    public void CallActionSquare()
    {
        RebotadorGO.SetActive(true);
    }

    public void ReturnSquareToNormal()
    {
        RebotadorGO.SetActive(false);
    }
}
