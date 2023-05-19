using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TableMap : NetworkBehaviour
{
    SquareController[,] allSquares;

    [Header("Listas Temporales Casillas")]
    [SerializeField] List<SquareController> squaresDisable;
    [SerializeField] List<SquareController> allSquaresEnables;

    [Header("Componentes")]
    [SerializeField] GameObject squarePrefab;
    [SerializeField] GameObject squareSinglePlayerPrefab;
    [SerializeField] List<PatronCasillasData> patronesData;

    [Header("Datos Tablero")]
    [SerializeField] int rows;
    [SerializeField] int columns;
    [SerializeField] float timePerRound;
    [SerializeField] float timeBtwRound;
    [SerializeField] bool isPlaying = false;

    Vector3 scaleSquarePrefab;
    IEnumerator roundGame;

    public static TableMap Instance { get { return instace; } }
    private static TableMap instace;

    void Start() {
        roundGame = NextRound(GetRandomPatron());
        instace = this;
    }

    public void StartRoundGameInvoke() {
        roundGame = NextRound(GetRandomPatron());
        StartCoroutine(roundGame);
        LobbyController.Instance.DeleteLobby();
    }

    public void InitTable() {
        allSquares = new SquareController[rows, columns];
        scaleSquarePrefab = squarePrefab.transform.GetComponent<Transform>().localScale;

        for (int i = 0; i < rows; i++)
            for (int j = 0; j < columns; j++)
                InstantiateSquare(i, j);
    }

    void InstantiateSquare(int fila, int columna) {
        Vector3 squarePosition = new Vector3(transform.position.x + (fila * scaleSquarePrefab.x), -2, transform.position.y + (columna * scaleSquarePrefab.z));
        Transform newSquareTransform;
        if (GameStateManager.Instance.isInMultiplayer) {
            newSquareTransform = Instantiate(squarePrefab, squarePosition, Quaternion.identity, transform).GetComponent<Transform>();
            newSquareTransform.GetComponent<NetworkObject>().Spawn(true);
        }
        else {
            newSquareTransform = Instantiate(squareSinglePlayerPrefab, squarePosition, Quaternion.identity, transform).GetComponent<Transform>();
            newSquareTransform = newSquareTransform.transform;
        }
        newSquareTransform.parent = transform;
        SquareController newSquare = newSquareTransform.GetComponent<SquareController>();
        newSquare.InitDataSquare(new Vector2(fila, columna), squarePosition);

        allSquares[fila, columna] = newSquare;
        allSquaresEnables.Add(newSquare);
    }

    public void AddSquareToData() {
        allSquares = new SquareController[rows, columns];
        int childrenTableCount = transform.childCount;
        for(int i = 0; i<childrenTableCount;i++) {
            SquareController newSquare = transform.GetChild(i).GetComponent<SquareController>();
            Vector2 newIdSquare = newSquare.GetIdSquare();
            allSquares[(int)newIdSquare.x, (int)newIdSquare.y] = newSquare;
            allSquaresEnables.Add(newSquare);
        }
    }

    IEnumerator NextRound(PatronCasillasData patronData)
    {
        Debug.Log(patronData.namePatron);
        yield return new WaitForSeconds(timeBtwRound);

        if (GameStateManager.Instance.isInMultiplayer)
            BuildPatronMapClientRpc(patronData);
        else
            BuildPatronMap(patronData);

        yield return new WaitForSeconds(timePerRound);
        if (GameStateManager.Instance.isInMultiplayer)
            ReturnMapNormalClientRpc();
        else
            ReturnMapNormal();

        roundGame = NextRound(GetRandomPatron());
        StartCoroutine(roundGame);
    }

    PatronCasillasData GetRandomPatron() {
        int randomPatron = Random.Range(0, patronesData.Count);
        return patronesData[randomPatron];
    }

    public void StopGame() {
        StopCoroutine(roundGame);
        if (GameStateManager.Instance.isInMultiplayer)
            ReturnMapNormalClientRpc();
        else 
            ReturnMapNormal();
    }

    public void DeleteAllDataTable() {
        StopCoroutine(roundGame);
        squaresDisable.Clear();
        allSquaresEnables.Clear();
        foreach (SquareController square in allSquares) {
            square.GetComponent<NetworkObject>().Despawn();
            Destroy(square.gameObject);
        }
    }

    [ClientRpc]
    public void BuildPatronMapClientRpc(PatronCasillasData patron) {
        SetSquareListType(patron.squareFallArray, TypeSquare.fall);
        SetSquareListType(patron.squareRebotadorArray, TypeSquare.rebotador);
        SetSquareListType(patron.squaresRepellerArray, TypeSquare.repellers);
        SetSquareListType(patron.squaresAttractorArray, TypeSquare.attractor);
    }
    public void BuildPatronMap(PatronCasillasData patron) {
        SetSquareListType(patron.squareFallArray, TypeSquare.fall);
        SetSquareListType(patron.squareRebotadorArray, TypeSquare.rebotador);
        SetSquareListType(patron.squaresRepellerArray, TypeSquare.repellers);
        SetSquareListType(patron.squaresAttractorArray, TypeSquare.attractor);
    }

    void SetSquareListType(Vector2Int[] squareArray, TypeSquare typeSquare) {
        foreach (Vector2Int id in squareArray)
            SetSquareType(id, typeSquare);
    }

    void SetSquareType(Vector2Int idCasilla, TypeSquare typeSquare) {
        SquareController casillaSeleccionada = allSquares[idCasilla.x, idCasilla.y];
        casillaSeleccionada.CallAndChangeTypeSquareAction(typeSquare);

        squaresDisable.Add(casillaSeleccionada);
        allSquaresEnables.Remove(casillaSeleccionada);
    }

    [ClientRpc]
    public void ReturnMapNormalClientRpc() {
        foreach (SquareController casilla in squaresDisable) {
            casilla.ReturnSquareToNormalState();
            allSquaresEnables.Add(casilla);
        }
        squaresDisable.Clear();
    }
    public void ReturnMapNormal() {
        foreach (SquareController casilla in squaresDisable) {
            casilla.ReturnSquareToNormalState();
            allSquaresEnables.Add(casilla);
        }
        squaresDisable.Clear();
    }
}
