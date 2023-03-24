using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "Patrones", menuName = "ScriptableObjects/Patrones")]
public class PatronCasillasData : ScriptableObject, INetworkSerializable {
    public string namePatron;
    public Vector2Int[] squareFallArray;
    public Vector2Int[] squareRebotadorArray;
    public Vector2Int[] squaresRepellerArray;
    public Vector2Int[] squaresAttractorArray;

    public List<Vector2Int> squareFallList;
    public List<Vector2Int> squareRebotadorList;
    public List<Vector2Int> squaresRepellerList;
    public List<Vector2Int> squaresAttractorList;

    /*public void InitArrayData() {
        if(squareFallList.Count > 0) {
            squareFallArray = new Vector2Int[squareFallList.Count];
            for (int fallSquare = 0; fallSquare < squareFallList.Count; fallSquare++)
                squareFallArray[fallSquare] = squareFallList[fallSquare];
        }

        if (squareRebotadorList.Count > 0) {
            squareRebotadorArray = new Vector2Int[squareRebotadorList.Count];
            for (int fallSquare = 0; fallSquare < squareRebotadorList.Count; fallSquare++)
                squareRebotadorArray[fallSquare] = squareRebotadorList[fallSquare];
        }

        if (squaresRepellerList.Count > 0) {
            squaresRepellerArray = new Vector2Int[squaresRepellerList.Count];
            for (int fallSquare = 0; fallSquare < squaresRepellerList.Count; fallSquare++)
                squaresRepellerArray[fallSquare] = squaresRepellerList[fallSquare];
        }

        if (squaresAttractorList.Count > 0) {
            squaresAttractorArray = new Vector2Int[squaresAttractorList.Count];
            for (int fallSquare = 0; fallSquare < squaresAttractorList.Count; fallSquare++)
                squaresAttractorArray[fallSquare] = squaresAttractorList[fallSquare];
        }
    }*/

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
        serializer.SerializeValue(ref squareFallArray);
        serializer.SerializeValue(ref squareRebotadorArray);
        serializer.SerializeValue(ref squaresRepellerArray);
        serializer.SerializeValue(ref squaresAttractorArray);
    }
}
/*
public struct PatronCasillasDataNoSO: INetworkSerializable {
    public string namePatron;

    public Vector2[] squareFallList;
    public List<Vector2Int> squareRebotadorList;
    public List<Vector2Int> squaresRepellerList;
    public List<Vector2Int> squaresAttractorList;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
        serializer.SerializeValue(ref namePatron);
        serializer.SerializeValue(ref squareFallList);
    }
}*/