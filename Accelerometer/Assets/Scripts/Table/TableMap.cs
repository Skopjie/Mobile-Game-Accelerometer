using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableMap : MonoBehaviour
{
    public Casilla[,] casillas;

    [Header("Componentes")]
    [SerializeField] GameObject casillaPrefab;
    [SerializeField] PatronCasillasData patronData;

    [Header("Datos Tablero")]
    [SerializeField] int filas;
    [SerializeField] int columnas;
    [SerializeField] bool isPlaying = false;

    Vector3 scalePrefab;


    // Start is called before the first frame update
    void Start()
    {
        InitTable();

        //InvokeRepeating("DisableRandomCasilla", 2, 2);

        if(patronData != null && isPlaying == true)
            BuildPatronMap(patronData);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void InitTable()
    {
        casillas = new Casilla[filas, columnas];
        scalePrefab = casillaPrefab.GetComponent<Transform>().localScale;

        for(int i = 0; i<filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                InstantiateCasilla(i, j);
            }
        }
    }

    void InstantiateCasilla(int fila, int columna)
    {
        Vector3 positionCasilla = new Vector3(transform.position.x + (fila * scalePrefab.x), 0, transform.position.y + (columna * scalePrefab.z));
        Casilla casilla = Instantiate(casillaPrefab, positionCasilla, Quaternion.identity, transform).GetComponent<Casilla>();
        casilla.id = new Vector2(fila, columna);
        casillas[fila, columna] = casilla;
    }

    void DisableRandomCasilla()
    {
        DisableCasilla(GetRandomCasilla());
    }

    void BuildPatronMap(PatronCasillasData patron)
    {
        DisablePatronCasillas(patron);
        AddRebotadorPatronCasillas(patron);
    }

    void DisablePatronCasillas(PatronCasillasData patron)
    {
        foreach(Vector2Int id in patron.casillasEliminadas)
        {
            DisableCasilla(id);
        }
    }

    void AddRebotadorPatronCasillas(PatronCasillasData patron)
    {
        foreach (Vector2Int id in patron.casillasRebotador)
        {
            AddRebotadorCasilla(id);
        }
    }

    Vector2Int GetRandomCasilla()
    {
        int randomfila = Random.Range(0, filas);
        int randomColumna = Random.Range(0, columnas);
        return new Vector2Int(randomfila, randomColumna);
    }

    void DisableCasilla(Vector2Int idCasilla)
    {
        casillas[idCasilla.x, idCasilla.y].SelectCasillaDisactive();
    }

    void AddRebotadorCasilla(Vector2Int idCasilla)
    {
        casillas[idCasilla.x, idCasilla.y].AddRebotadorCasilla();
    }

}
