#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelStaticBuilder))]
public class LevelStaticBuilderCustomEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open Editor"))
        {
            LevelStaticBuilderEditorWindow.Open((LevelStaticBuilder)target);
        }
    }
}
#endif
