using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableMap : MonoBehaviour
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

    void Start()
    {
        InitTable();

        if(patronesData != null && isPlaying == true)
            StartCoroutine(NextRound(GetRandomPatron()));
    }

    void InitTable()
    {
        allSquares = new SquareController[rows, columns];
        scaleSquarePrefab = squarePrefab.transform.GetChild(0).GetComponent<Transform>().localScale;

        for(int i = 0; i<rows; i++)
            for (int j = 0; j < columns; j++)
                InstantiateCasilla(i, j);
    }

    void InstantiateCasilla(int fila, int columna)
    {
        Vector3 squarePosition = new Vector3(transform.position.x + (fila * scaleSquarePrefab.x), -2, transform.position.y + (columna * scaleSquarePrefab.z));
        SquareController newSquare = Instantiate(squarePrefab, squarePosition, Quaternion.identity, transform).transform.GetChild(0).GetComponent<SquareController>();
        newSquare.InitDataSquare(new Vector2(fila, columna), squarePosition);

        allSquares[fila, columna] = newSquare;
        allSquaresEnables.Add(newSquare);
    }



    IEnumerator NextRound(PatronCasillasData patronData)
    {
        yield return new WaitForSeconds(timeBtwRound);
        BuildPatronMap(patronData);

        yield return new WaitForSeconds(timePerRound);
        ReturnMapNormal();
        StartCoroutine(NextRound(GetRandomPatron()));
    }

    PatronCasillasData GetRandomPatron()
    {
        int randomPatron = Random.Range(0, patronesData.Count);
        return patronesData[randomPatron];
    }



    public void BuildPatronMap(PatronCasillasData patron)
    {
        SetSquareListType(patron.squareFallList, TypeSquare.fall);
        SetSquareListType(patron.squareRebotadorList, TypeSquare.rebotador);
        SetSquareListType(patron.squaresRepellerList, TypeSquare.repellers);
        SetSquareListType(patron.squaresAttractorList, TypeSquare.attractor);
    }

    void SetSquareListType(List<Vector2Int> squareList, TypeSquare typeSquare)
    {
        foreach (Vector2Int id in squareList)
            SetSquareType(id, typeSquare);
    }

    void SetSquareType(Vector2Int idCasilla, TypeSquare typeSquare)
    {
        SquareController casillaSeleccionada = allSquares[idCasilla.x, idCasilla.y];
        casillaSeleccionada.CallAndChangeTypeSquareAction(typeSquare);

        squaresDisable.Add(casillaSeleccionada);
        allSquaresEnables.Remove(casillaSeleccionada);
    }

    public void ReturnMapNormal()
    {
        foreach (SquareController casilla in squaresDisable)
        {
            casilla.ReturnSquareToNormalState();
            allSquaresEnables.Add(casilla);
        }
        squaresDisable.Clear();
    }
}
