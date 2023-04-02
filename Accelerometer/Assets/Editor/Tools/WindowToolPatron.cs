using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WindowToolPatron : EditorWindow
{

    GUISkin mySkin;

    Texture2D backGroundTexture;
    Texture2D headerSectionTexture;

    Color backGroundSectionColor = new Color(60f / 255f, 60f / 255f, 60f / 255f, 1f);
    Color headerSectionColor = new Color(40f / 255f, 40f / 255f, 40f / 255f, 1f);

    Rect backGroundSection;
    Rect headerSection;

    Vector2 scrollPos;
    Vector2 scrollPosDescription;

    static PatronCasillasData patronCasilla;
    public static PatronCasillasData patronCasillaInfo { get { return patronCasilla; } }



    SerializedObject casillasSerializedObject;

    SerializedProperty namePatronCasillas;
    SerializedProperty casillasEliminadas;
    SerializedProperty casillasRebotador;
    SerializedProperty squareRepellerProperty;
    SerializedProperty squareAttractorProperty;

    int filaSize = 8;
    int columnaSize = 8;


    List<Vector2Int> squaresFallList = new List<Vector2Int>();
    List<Vector2Int> squaresRebotadorList = new List<Vector2Int>();
    List<Vector2Int> squareRepellerList = new List<Vector2Int>();
    List<Vector2Int> squareAttractorList = new List<Vector2Int>();


    idSelector idCasillaSeleccionada;

    GUIStyle redBackgroundStyle;
    GUIStyle whiteBackgroundStyle;
    GUIStyle greenBackgroundStyle;
    GUIStyle purpleBackgroundStyle;
    GUIStyle blueBackgroundStyle;


    [MenuItem("Window/Tool")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<WindowToolPatron>("Tool");
    }

    private void OnEnable()
    {
        InitTextures();
        InitData();

        mySkin = Resources.Load<GUISkin>("GameDesingWindow");
    }

    void InitTextures()
    {
        InitTexture(backGroundTexture, backGroundSectionColor);
        InitTexture(headerSectionTexture, headerSectionColor);
    }

    void InitTexture(Texture2D texture, Color colorTexture)
    {
        texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, colorTexture);
        texture.Apply();
    }

    void InitData()
    {
        patronCasilla = (PatronCasillasData)ScriptableObject.CreateInstance(typeof(PatronCasillasData));
        casillasSerializedObject = new SerializedObject(patronCasilla);

        namePatronCasillas = casillasSerializedObject.FindProperty("namePatron");
        casillasEliminadas = casillasSerializedObject.FindProperty("squareFallList");
        casillasRebotador = casillasSerializedObject.FindProperty("squareRebotadorList");
        squareRepellerProperty = casillasSerializedObject.FindProperty("squaresRepellerList");
        squareAttractorProperty = casillasSerializedObject.FindProperty("squaresAttractorList");
    }

    void SetButtonsColor()
    {
        whiteBackgroundStyle = new GUIStyle(GUI.skin.button);
        whiteBackgroundStyle.normal.background = MakeBackgroundTexture(10, 10, Color.white);
        whiteBackgroundStyle.normal.textColor = Color.black;
        whiteBackgroundStyle.richText = true;

        redBackgroundStyle = new GUIStyle(GUI.skin.button);
        redBackgroundStyle.normal.background = MakeBackgroundTexture(10, 10, Color.red);
        redBackgroundStyle.normal.textColor = Color.black;
        redBackgroundStyle.richText = true;

        greenBackgroundStyle = new GUIStyle(GUI.skin.button);
        greenBackgroundStyle.normal.background = MakeBackgroundTexture(10, 10, Color.green);
        greenBackgroundStyle.normal.textColor = Color.black;
        greenBackgroundStyle.richText = true;

        purpleBackgroundStyle = new GUIStyle(GUI.skin.button);
        purpleBackgroundStyle.normal.background = MakeBackgroundTexture(10, 10, Color.magenta);
        purpleBackgroundStyle.normal.textColor = Color.black;
        purpleBackgroundStyle.richText = true;

        blueBackgroundStyle = new GUIStyle(GUI.skin.button);
        blueBackgroundStyle.normal.background = MakeBackgroundTexture(10, 10, Color.blue);
        blueBackgroundStyle.normal.textColor = Color.black;
        blueBackgroundStyle.richText = true;

        SetButtonColor(redBackgroundStyle, Color.red);
        SetButtonColor(whiteBackgroundStyle, Color.white);
        SetButtonColor(greenBackgroundStyle, Color.green);
        SetButtonColor(purpleBackgroundStyle, Color.magenta);
        SetButtonColor(blueBackgroundStyle, Color.blue);
    }

    void SetButtonColor(GUIStyle style, Color colorBackground)
    {
        style = new GUIStyle(GUI.skin.button);
        style.normal.background = MakeBackgroundTexture(10, 10, colorBackground);
        style.normal.textColor = Color.black;
        style.richText = true;
    }

    private void OnGUI()
    {
        GUI.skin = mySkin;
        casillasSerializedObject.Update();


        DrawLayouts();
        DrawTablePanel();
        DrawPropertiesSOPanel();

        casillasSerializedObject.ApplyModifiedProperties();
    }

    void DrawLayouts()
    {
        backGroundSection.x = 0;
        backGroundSection.y = 0;
        backGroundSection.width = Screen.width / 2;
        backGroundSection.height = Screen.height;

        GUI.DrawTexture(backGroundSection, backGroundTexture);

        ////////////////////

        headerSection.x = Screen.width / 2;
        headerSection.y = 0;
        headerSection.width = Screen.width / 2;
        headerSection.height = Screen.height;

        GUI.DrawTexture(headerSection, headerSectionTexture);
    }

    void DrawTablePanel()
    {
        SetButtonsColor();

        GUILayout.BeginArea(backGroundSection);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        GUILayout.Label("Modificador Mapa:");

        for (int fila = 0; fila < filaSize; fila++)
        {
            GUILayout.BeginHorizontal();
            for (int columna = 0; columna < columnaSize; columna++)
            {
                PrintButton(fila, columna);
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(20);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Fall", redBackgroundStyle))
        {
            idCasillaSeleccionada = idSelector.eliminado;
        }

        if (GUILayout.Button("Rebo", greenBackgroundStyle))
        {
            idCasillaSeleccionada = idSelector.rebotador;
        }

        if (GUILayout.Button("Repe", purpleBackgroundStyle))
        {
            idCasillaSeleccionada = idSelector.repeller;
        }

        if (GUILayout.Button("Atra", blueBackgroundStyle))
        {
            idCasillaSeleccionada = idSelector.attractor;
        }

        if (GUILayout.Button("Delete", whiteBackgroundStyle))
        {
            idCasillaSeleccionada = idSelector.borrar;
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Delete All", GUILayout.Height(50)))
        {
            DeleteAllCasillas();
        }

        if (GUILayout.Button("Deselecionar", GUILayout.Height(50)))
        {
            DeselecionarSO();
        }

        GUILayout.EndHorizontal();


        Event evt = Event.current;
        Rect drop_area = GUILayoutUtility.GetRect(0.0f, 35.0f, GUILayout.ExpandWidth(true));
        GUI.Box(drop_area, "Drag Action To Copy");

        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!drop_area.Contains(evt.mousePosition))
                    return;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    foreach (Object dragged_object in DragAndDrop.objectReferences)
                    {
                        PatronCasillasData obj = dragged_object as PatronCasillasData;
                        CopyDataFromPatron(obj);

                        casillasSerializedObject.Update();
                        casillasSerializedObject.ApplyModifiedProperties();
                        /*bool check = EditorUtility.DisplayDialog("Copy Action Data", "You have dragged an action file, do you want to copy its data to the window?", "Yes", "No");
                        if (check)
                        {
                            patronCasilla = obj;
                            //copyActionData(obj);
                        }
                        else
                        {
                            Debug.Log("NO");
                        }*/
                    }
                }
                break;
        }

        GUILayout.EndArea();

        EditorGUILayout.EndScrollView();
    }

    private void DeselecionarSO()
    {
        //patronCasilla = (PatronCasillasData)ScriptableObject.CreateInstance(typeof(PatronCasillasData));
        //casillasSerializedObject = new SerializedObject(patronCasilla);
    }

    void PrintButton(int fila, int columna)
    {
        if (GUILayout.Button("", ChoseColorButton(fila, columna), GUILayout.Width(30), GUILayout.Height(30)))
        {
            SelectCasilla(fila, columna);

            casillasSerializedObject.Update();
            casillasSerializedObject.ApplyModifiedProperties();
        }
    }

    GUIStyle ChoseColorButton(int fila, int columna)
    {
        if (squaresFallList.Contains(new Vector2Int(fila, columna)))
            return redBackgroundStyle;

        else if (squaresRebotadorList.Contains(new Vector2Int(fila, columna)))
            return greenBackgroundStyle;

        else if (squareRepellerList.Contains(new Vector2Int(fila, columna)))
            return purpleBackgroundStyle;

        else if (squareAttractorList.Contains(new Vector2Int(fila, columna)))
            return blueBackgroundStyle;

        else
            return whiteBackgroundStyle;
    }

    void SelectCasilla(int fila, int columna)
    {
        switch (idCasillaSeleccionada)
        {
            case idSelector.eliminado:
                DeleteCasilla(fila, columna);
                if (!squaresFallList.Contains(new Vector2Int(fila, columna)))
                {
                    squaresFallList.Add(new Vector2Int(fila, columna));
                    patronCasillaInfo.squareFallList = squaresFallList;
                }
                break;

            case idSelector.rebotador:
                DeleteCasilla(fila, columna);
                if (!squaresRebotadorList.Contains(new Vector2Int(fila, columna)))
                {
                    squaresRebotadorList.Add(new Vector2Int(fila, columna));
                    patronCasillaInfo.squareRebotadorList = squaresRebotadorList;
                }
                break;

            case idSelector.repeller:
                DeleteCasilla(fila, columna);
                if (!squareRepellerList.Contains(new Vector2Int(fila, columna)))
                {
                    squareRepellerList.Add(new Vector2Int(fila, columna));
                    patronCasillaInfo.squaresRepellerList = squareRepellerList;
                }
                break;

            case idSelector.attractor:
                DeleteCasilla(fila, columna);
                if (!squareAttractorList.Contains(new Vector2Int(fila, columna)))
                {
                    squareAttractorList.Add(new Vector2Int(fila, columna));
                    patronCasillaInfo.squaresAttractorList = squareAttractorList;
                }
                break;

            case idSelector.borrar:
                DeleteCasilla(fila, columna);
                break;
        }
    }

    void CopyDataFromPatron(PatronCasillasData data)
    {
        InitData();

        patronCasillaInfo.squareFallList = data.squareFallList;
        squaresFallList = data.squareFallList;

        patronCasillaInfo.squareRebotadorList = data.squareRebotadorList;
        squaresRebotadorList = data.squareRebotadorList;

        patronCasillaInfo.squaresRepellerList = data.squaresRepellerList;
        squareRepellerList = data.squaresRepellerList;

        patronCasillaInfo.squaresAttractorList = data.squaresAttractorList;
        squareAttractorList = data.squaresAttractorList;

        patronCasillaInfo.namePatron = data.namePatron;

        casillasSerializedObject.Update();
        casillasSerializedObject.ApplyModifiedProperties();
    }

    void DeleteAllCasillas()
    {
        for (int fila = 0; fila < filaSize; fila++)
            for (int columna = 0; columna < columnaSize; columna++)
                DeleteCasilla(fila, columna);
    }

    void DeleteCasilla(int fila, int columna)
    {
        if (squaresFallList.Contains(new Vector2Int(fila, columna)))
            squaresFallList.Remove(new Vector2Int(fila, columna));

        if (squaresRebotadorList.Contains(new Vector2Int(fila, columna)))
            squaresRebotadorList.Remove(new Vector2Int(fila, columna));

        if (squareRepellerList.Contains(new Vector2Int(fila, columna)))
            squareRepellerList.Remove(new Vector2Int(fila, columna));

        if (squareAttractorList.Contains(new Vector2Int(fila, columna)))
            squareAttractorList.Remove(new Vector2Int(fila, columna));
    }

    void DrawPropertiesSOPanel()
    {
        GUILayout.BeginArea(headerSection);

        scrollPosDescription = EditorGUILayout.BeginScrollView(scrollPosDescription);

        GUILayout.Label("Datos del Patron:");

        EditorGUILayout.PropertyField(namePatronCasillas, new GUIContent("Nombre Patron"), true);
        EditorGUILayout.PropertyField(casillasEliminadas, new GUIContent("Casillas Eliminadas"), true);
        EditorGUILayout.PropertyField(casillasRebotador, new GUIContent("Casillas Rebotador"), true);
        EditorGUILayout.PropertyField(squareRepellerProperty, new GUIContent("Squares Repeller"), true);
        EditorGUILayout.PropertyField(squareAttractorProperty, new GUIContent("Squares Attractor"), true);

        GUILayout.Space(20);

        filaSize = EditorGUILayout.IntField("Filas:", filaSize);
        columnaSize = EditorGUILayout.IntField("Columnas:", columnaSize);

        GUILayout.Space(20);

        if (GUILayout.Button("Crear Mapa", GUILayout.Height(40)))
        {
            CreateMapDesing();
        }

        GUILayout.EndArea();

        EditorGUILayout.EndScrollView();
    }

    private void CreateMapDesing()
    {
        string dataPath = "";
        dataPath = "Assets/SO/PatronesCasillas/" + patronCasilla.namePatron + ".asset";

        casillasSerializedObject.Update();
        casillasSerializedObject.ApplyModifiedProperties();

        PatronCasillasData myType = AssetDatabase.LoadAssetAtPath(dataPath, typeof(PatronCasillasData)) as PatronCasillasData;
        AssetDatabase.CreateAsset(WindowToolPatron.patronCasillaInfo, dataPath);

        patronCasillaInfo.squareFallList = squaresFallList;
        patronCasillaInfo.squareRebotadorList = squaresRebotadorList;
        patronCasillaInfo.squaresRepellerList = squareRepellerList;
        patronCasillaInfo.squaresAttractorList = squareAttractorList;
        patronCasilla.InitArrayData();

        //InitData();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }

    private Texture2D MakeBackgroundTexture(int width, int height, Color color)
    {
        Color[] pixels = new Color[width * height];

        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = color;
        }

        Texture2D backgroundTexture = new Texture2D(width, height);

        backgroundTexture.SetPixels(pixels);
        backgroundTexture.Apply();

        return backgroundTexture;
    }
}