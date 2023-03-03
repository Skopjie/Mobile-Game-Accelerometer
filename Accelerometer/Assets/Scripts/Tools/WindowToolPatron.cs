using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WindowToolPatron : EditorWindow
{
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
        InitData();
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
        casillasSerializedObject.Update();


        for(int y =0;y<5;y++)
        {
            GUILayout.BeginHorizontal();
            for (int i = 0; i < 5; i++)
            {
                if (GUILayout.Button("", GUILayout.Height(20), GUILayout.Width(20)))
                {
                    //InitializeActionData();
                    patronCasilla.casillasEliminadas.Add(new Vector2Int(y, i));

                    casillasSerializedObject.Update();
                    casillasSerializedObject.ApplyModifiedProperties();
                }
            }
            GUILayout.EndHorizontal();
        }



        EditorGUILayout.PropertyField(namePatronCasillas, new GUIContent("Nombre Patron"), true);
        EditorGUILayout.PropertyField(casillasEliminadas, new GUIContent("Casillas Eliminadas"), true);
        EditorGUILayout.PropertyField(casillasRebotador, new GUIContent("Casillas Rebotador"), true);

        if (GUILayout.Button("Crear Mapa", GUILayout.Height(40)))
        {
            InitializeActionData();
        }

        casillasSerializedObject.ApplyModifiedProperties();
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
}
