#if UNITY_EDITOR
using UnityEditor;
#endif

public class SerializableEnumComponent : UnityEngine.MonoBehaviour
{
    public Direction Value;
}

#if UNITY_EDITOR
[CustomEditor(typeof(SerializableEnumComponent))]
public class SerializableEnumComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawPropertiesExcluding(serializedObject, "m_Script");
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
