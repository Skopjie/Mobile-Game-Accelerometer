using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WindowToolPatron : EditorWindow
{

    GUISkin mySkin;

    Texture2D backGroundTexture;
    Texture2D headerSectionTexture;

    Color backGroundSectionColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 1f);
    Color headerSectionColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 1f);

    Rect backGroundSection;
    Rect headerSection;



    static PatronCasillasData patronCasilla;
    public static PatronCasillasData patronCasillaInfo { get { return patronCasilla; } }



    SerializedObject casillasSerializedObject;

    SerializedProperty namePatronCasillas;
    SerializedProperty casillasEliminadas;
    SerializedProperty casillasRebotador;





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
        backGroundTexture = new Texture2D(1, 1);
        backGroundTexture.SetPixel(0, 0, backGroundSectionColor);
        backGroundTexture.Apply();

        headerSectionTexture = new Texture2D(1, 1);
        headerSectionTexture.SetPixel(0, 0, headerSectionColor);
        headerSectionTexture.Apply();
    }

    void InitData()
    {
        patronCasilla = (PatronCasillasData)ScriptableObject.CreateInstance(typeof(PatronCasillasData));
        casillasSerializedObject = new SerializedObject(patronCasilla);

        namePatronCasillas = casillasSerializedObject.FindProperty("namePatron");
        casillasEliminadas = casillasSerializedObject.FindProperty("casillasEliminadas");
        casillasRebotador = casillasSerializedObject.FindProperty("casillasRebotador");
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
        backGroundSection.width = Screen.width/2;
        backGroundSection.height = Screen.height;

        GUI.DrawTexture(backGroundSection, backGroundTexture);

        ////////////////////

        headerSection.x = Screen.width / 2;
        headerSection.y = 0;
        headerSection.width = Screen.width/2;
        headerSection.height = Screen.height;

        GUI.DrawTexture(headerSection, headerSectionTexture);
    }

    void DrawTablePanel()
    {
        GUIStyle yellowBackgroundStyle = new GUIStyle(GUI.skin.button);
        yellowBackgroundStyle.normal.background = MakeBackgroundTexture(10, 10, Color.black);

        GUIStyle yellowBackgroundStyle1 = new GUIStyle(GUI.skin.button);
        yellowBackgroundStyle1.normal.background = MakeBackgroundTexture(10, 10, Color.red);

        GUILayout.BeginArea(backGroundSection);
        for (int y = 0; y < 5; y++)
        {
            GUILayout.BeginHorizontal();
            for (int i = 0; i < 5; i++)
            {
                if(i>2)
                {
                    if (GUILayout.Button("", yellowBackgroundStyle))
                    {
                        //InitializeActionData();
                        Debug.Log(y + " / " + i);
                        //patronCasilla.casillasEliminadas.Add(new Vector2Int(y, i));

                        casillasSerializedObject.Update();
                        casillasSerializedObject.ApplyModifiedProperties();
                    }
                }
                else
                {
                    if (GUILayout.Button("", yellowBackgroundStyle1))
                    {
                        //InitializeActionData();
                        Debug.Log(y + " / " + i);
                        //patronCasilla.casillasEliminadas.Add(new Vector2Int(y, i));

                        casillasSerializedObject.Update();
                        casillasSerializedObject.ApplyModifiedProperties();
                    }
                }
               
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();
    }

    void DrawPropertiesSOPanel()
    {
        GUILayout.BeginArea(headerSection);

        EditorGUILayout.PropertyField(namePatronCasillas, new GUIContent("Nombre Patron"), true);
        EditorGUILayout.PropertyField(casillasEliminadas, new GUIContent("Casillas Eliminadas"), true);
        EditorGUILayout.PropertyField(casillasRebotador, new GUIContent("Casillas Rebotador"), true);

        if (GUILayout.Button("Crear Mapa", GUILayout.Height(40)))
        {
            InitializeActionData();
        }

        GUILayout.EndArea();
    }

    private void InitializeActionData()
    {
        string dataPath = "";
        dataPath = "Assets/SO/PatronesCasillas/" + patronCasilla.namePatron + ".asset";

        casillasSerializedObject.Update();
        casillasSerializedObject.ApplyModifiedProperties();

        PatronCasillasData myType = AssetDatabase.LoadAssetAtPath(dataPath, typeof(PatronCasillasData)) as PatronCasillasData;
        AssetDatabase.CreateAsset(WindowToolPatron.patronCasillaInfo, dataPath);

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
