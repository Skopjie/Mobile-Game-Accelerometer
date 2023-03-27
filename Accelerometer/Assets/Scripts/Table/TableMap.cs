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
    [SerializeField] List<PatronCasillasData> patronesData;

    [Header("Datos Tablero")]
    [SerializeField] int rows;
    [SerializeField] int columns;
    [SerializeField] float timePerRound;
    [SerializeField] float timeBtwRound;
    [SerializeField] bool isPlaying = false;

    Vector3 scaleSquarePrefab;

    public NetworkVariable<int> numberPlayer =
    new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    public static TableMap Instance { get { return instace; } }
    private static TableMap instace;

    void Start()
    {
        //InitTable();
        instace = this;

        if (patronesData != null && isPlaying == true)
            StartCoroutine(NextRound(GetRandomPatron()));
    }

    public void AddNewPlayer() {
        numberPlayer.Value++;
        Debug.Log(numberPlayer.Value);

        if (IsHost)
            InitTable();
        else
            AddSquareToData();
        if (numberPlayer.Value == 2) {
            StartGame();
            if(IsHost)
                StartRoundGameInvoke();
        }
    }

    public void StartGame() {
        Debug.Log("hola");
    }

    public void StartRoundGameInvoke() {
        StartCoroutine(NextRound(GetRandomPatron()));
    }

    public void InitTable()
    {
        allSquares = new SquareController[rows, columns];
        scaleSquarePrefab = squarePrefab.transform.GetComponent<Transform>().localScale;

        for(int i = 0; i<rows; i++)
            for (int j = 0; j < columns; j++)
                InstantiateCasilla(i, j);
    }

    void InstantiateCasilla(int fila, int columna)
    {
        Vector3 squarePosition = new Vector3(transform.position.x + (fila * scaleSquarePrefab.x), -2, transform.position.y + (columna * scaleSquarePrefab.z));
        Transform newSquareTransform = Instantiate(squarePrefab, squarePosition, Quaternion.identity, transform).GetComponent<Transform>();
        newSquareTransform.GetComponent<NetworkObject>().Spawn(true);
        newSquareTransform.parent = transform;
        SquareController newSquare = newSquareTransform.GetComponent<SquareController>();
        newSquare.InitDataSquare(new Vector2(fila, columna), squarePosition);

        allSquares[fila, columna] = newSquare;
        allSquaresEnables.Add(newSquare);
    }

    public void AddQuareListCount() {
        Invoke("AddSquareToData", 1);
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
        BuildPatronMapClientRpc(patronData);

        yield return new WaitForSeconds(timePerRound);
        ReturnMapNormalClientRpc();
        StartCoroutine(NextRound(GetRandomPatron()));
    }

    PatronCasillasData GetRandomPatron()
    {
        int randomPatron = Random.Range(0, patronesData.Count);
        return patronesData[randomPatron];
    }


    [ClientRpc]
    public void BuildPatronMapClientRpc(PatronCasillasData patron)
    {
        //patron.InitArrayData();
        SetSquareListType(patron.squareFallArray, TypeSquare.fall);
        SetSquareListType(patron.squareRebotadorArray, TypeSquare.rebotador);
        SetSquareListType(patron.squaresRepellerArray, TypeSquare.repellers);
        SetSquareListType(patron.squaresAttractorArray, TypeSquare.attractor);
    }

    void SetSquareListType(Vector2Int[] squareArray, TypeSquare typeSquare)
    {
        foreach (Vector2Int id in squareArray)
            SetSquareType(id, typeSquare);
    }

    void SetSquareType(Vector2Int idCasilla, TypeSquare typeSquare)
    {
        SquareController casillaSeleccionada = allSquares[idCasilla.x, idCasilla.y];
        casillaSeleccionada.CallAndChangeTypeSquareAction(typeSquare);

        squaresDisable.Add(casillaSeleccionada);
        allSquaresEnables.Remove(casillaSeleccionada);
    }

    [ClientRpc]
    public void ReturnMapNormalClientRpc()
    {
        foreach (SquareController casilla in squaresDisable)
        {
            casilla.ReturnSquareToNormalState();
            allSquaresEnables.Add(casilla);
        }
        squaresDisable.Clear();
    }
}
