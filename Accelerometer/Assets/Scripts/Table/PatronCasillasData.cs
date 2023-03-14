using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Patrones", menuName = "ScriptableObjects/Patrones")]
public class PatronCasillasData : ScriptableObject
{
    public string namePatron;

    public List<Vector2Int> squareFallList;
    public List<Vector2Int> squareRebotadorList;
    public List<Vector2Int> squaresRepellerList;
    public List<Vector2Int> squaresAttractorList;
}
