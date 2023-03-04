using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableMap : MonoBehaviour
{
    public Casilla[,] casillas;
    [SerializeField] List<Casilla> casillasEliminadas;
    [SerializeField] List<Casilla> casillasRebotador;

    [Header("Componentes")]
    [SerializeField] GameObject casillaPrefab;

    [SerializeField] List<PatronCasillasData> patronesData;

    [Header("Datos Tablero")]
    [SerializeField] int filas;
    [SerializeField] int columnas;
    [SerializeField] float timePerRound;
    [SerializeField] float timeBtwRound;
    [SerializeField] bool isPlaying = false;

    Vector3 scalePrefab;
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

    // Update is called once per frame
    void Update()
    {
    }

    void InitTable()
    {
        casillas = new Casilla[filas, columnas];
        scalePrefab = casillaPrefab.transform.GetChild(0).GetComponent<Transform>().localScale;

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
        Casilla casilla = Instantiate(casillaPrefab, positionCasilla, Quaternion.identity, transform).transform.GetChild(0).GetComponent<Casilla>();
        casilla.id = new Vector2(fila, columna);
        casilla.casillaPosition = positionCasilla;
        casillas[fila, columna] = casilla;
    }





    IEnumerator NextRound(PatronCasillasData patronData)
    {
        yield return new WaitForSeconds(timeBtwRound);
        BuildPatronMap(patronData);

        yield return new WaitForSeconds(timePerRound);
        ReturnMapNormal();
        StartCoroutine(NextRound(GetRandomPatron()));
    }



    void BuildPatronMap(PatronCasillasData patron)
    {
        DisablePatronCasillas(patron);
        AddRebotadorPatronCasillas(patron);
    }

    void ReturnMapNormal()
    {
        foreach (Casilla casilla in casillasEliminadas)
        {
            casilla.ReturnCasillaInitialPosition();
        }

        foreach (Casilla casilla in casillasRebotador)
        {
            casilla.ReturnCasillaInitialPosition();
        }
        casillasEliminadas.Clear();
        casillasRebotador.Clear();
    }

    void DisablePatronCasillas(PatronCasillasData patron)
    {
        foreach(Vector2Int id in patron.casillasEliminadas)
        {
            DisableCasilla(id);
        }
    }

    void DisableCasilla(Vector2Int idCasilla)
    {
        Casilla casillaSeleccionada = casillas[idCasilla.x, idCasilla.y];
        casillaSeleccionada.SelectCasillaDisactive();
        casillasEliminadas.Add(casillaSeleccionada);
    }



    void AddRebotadorPatronCasillas(PatronCasillasData patron)
    {
        foreach (Vector2Int id in patron.casillasRebotador)
        {
            AddRebotadorCasilla(id);
        }
    }

    void AddRebotadorCasilla(Vector2Int idCasilla)
    {
        casillas[idCasilla.x, idCasilla.y].AddRebotadorCasilla();
        casillasRebotador.Add(casillas[idCasilla.x, idCasilla.y]);
    }





    void DisableRandomCasilla()
    {
        DisableCasilla(GetRandomCasilla());
    }

    Vector2Int GetRandomCasilla()
    {
        int randomfila = Random.Range(0, filas);
        int randomColumna = Random.Range(0, columnas);
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
