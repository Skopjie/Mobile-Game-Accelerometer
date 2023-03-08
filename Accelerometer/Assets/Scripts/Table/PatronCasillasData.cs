using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Patrones", menuName = "ScriptableObjects/Patrones")]
public class PatronCasillasData : ScriptableObject
{
    public string namePatron;

    public List<Vector2Int> casillasEliminadas;
    public List<Vector2Int> casillasRebotador;
    public List<Vector2Int> squaresRepeller;
}
