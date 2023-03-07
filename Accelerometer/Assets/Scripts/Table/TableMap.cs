using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableMap : MonoBehaviour
{
    public SquareController[,] allSquares;

    [Header("Listas Temporales Casillas")]
    [SerializeField] List<SquareController> sqaureFallList;
    [SerializeField] List<SquareController> squareRebotadorList;

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

    [Header("Datos Temporales")]
    [SerializeField] GameObject player;
    [SerializeField] Vector3 playerPos;


    // Start is called before the first frame update
    void Start()
    {
        InitTable();
        playerPos = player.transform.position;

        //InvokeRepeating("DisableRandomCasilla", 2, 2);

        if(patronesData != null && isPlaying == true)
            StartCoroutine(NextRound(GetRandomPatron()));
    }


    void InitTable()
    {
        allSquares = new SquareController[rows, columns];
        scaleSquarePrefab = squarePrefab.transform.GetChild(0).GetComponent<Transform>().localScale;

        for(int i = 0; i<rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                InstantiateCasilla(i, j);
            }
        }
    }

    void InstantiateCasilla(int fila, int columna)
    {
        Vector3 positionCasilla = new Vector3(transform.position.x + (fila * scaleSquarePrefab.x), 0, transform.position.y + (columna * scaleSquarePrefab.z));
        SquareController casilla = Instantiate(squarePrefab, positionCasilla, Quaternion.identity, transform).transform.GetChild(0).GetComponent<SquareController>();
        casilla.InitDataSquare(new Vector2(fila, columna), positionCasilla);

        allSquares[fila, columna] = casilla;
        allSquaresEnables.Add(casilla);
    }



    IEnumerator NextRound(PatronCasillasData patronData)
    {
        yield return new WaitForSeconds(timeBtwRound);
        BuildPatronMap(patronData);

        yield return new WaitForSeconds(timePerRound);
        ReturnMapNormal();
        StartCoroutine(NextRound(GetRandomPatron()));
    }



    public void BuildPatronMap(PatronCasillasData patron)
    {
        AddSquareFallPatron(patron);
        AddSquareRebotadorPatron(patron);
    }

    public void ReturnMapNormal()
    {
        foreach (SquareController casilla in sqaureFallList)
        {
            casilla.ReturnCasillaInitialPosition();
            allSquaresEnables.Add(casilla);
        }

        foreach (SquareController casilla in squareRebotadorList)
        {
            casilla.ReturnCasillaInitialPosition();
            allSquaresEnables.Add(casilla);
        }

        sqaureFallList.Clear();
        squareRebotadorList.Clear();
    }



    void AddSquareFallPatron(PatronCasillasData patron)
    {
        foreach(Vector2Int id in patron.casillasEliminadas)
        {
            AddSquareFall(id);
        }
    }

    void AddSquareFall(Vector2Int idCasilla)
    {
        SquareController casillaSeleccionada = allSquares[idCasilla.x, idCasilla.y];
        casillaSeleccionada.CallAndChangeTypeSquareAction(TypeSquare.fall);

        sqaureFallList.Add(casillaSeleccionada);
        allSquaresEnables.Remove(casillaSeleccionada);
    }



    void AddSquareRebotadorPatron(PatronCasillasData patron)
    {
        foreach (Vector2Int id in patron.casillasRebotador)
        {
            AddSquareRebotador(id);
        }
    }

    void AddSquareRebotador(Vector2Int idCasilla)
    {
        SquareController casillaSeleccionada = allSquares[idCasilla.x, idCasilla.y];
        casillaSeleccionada.CallAndChangeTypeSquareAction(TypeSquare.rebotador);

        squareRebotadorList.Add(casillaSeleccionada);
        allSquaresEnables.Remove(casillaSeleccionada);
    }





    void DisableRandomCasilla()
    {
        AddSquareFall(GetRandomCasilla());
    }

    Vector2Int GetRandomCasilla()
    {
        int randomfila = Random.Range(0, rows);
        int randomColumna = Random.Range(0, columns);
        return new Vector2Int(randomfila, randomColumna);
    }

    PatronCasillasData GetRandomPatron()
    {
        int randomPatron = Random.Range(0, patronesData.Count);
        return patronesData[randomPatron];
    }



    public void ReturnPlayer()
    {
        player.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        player.transform.position = playerPos;
    }

}
