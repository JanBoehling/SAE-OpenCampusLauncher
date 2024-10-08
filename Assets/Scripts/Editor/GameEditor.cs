using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Game))]
public class GameEditor : Editor
{
    private Game game;

    private void OnEnable()
    {
        
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.BeginHorizontal();
        game.GamePath = EditorGUILayout.TextField("Game Path", game.GamePath);
        if (GUILayout.Button("Select file"))
        {
            string filePath = EditorUtility.OpenFilePanel("Select game exe", "", "exe");
            game.GamePath = string.IsNullOrWhiteSpace(filePath) ? game.GamePath : filePath;
        }
        EditorGUILayout.EndHorizontal();
        
        DrawPropertiesExcluding(serializedObject, "<GamePath>k__BackingField", "m_Script");

        serializedObject.ApplyModifiedProperties();
    }
}
