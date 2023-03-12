#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

public class LevelStaticBuilderEditorWindow : EditorWindow
{
    private SerializedObject _serializedObject;
    private SerializedProperty _groupsDataProperty;

    private LevelStaticBuilder _levelBuilder;
    private Dictionary<string, List<Transform>> _levelObjects = new Dictionary<string, List<Transform>>();

    private Vector2 scrollPos;
    public static void Open(LevelStaticBuilder dataObject)
    {
        var window = GetWindow<LevelStaticBuilderEditorWindow>();
        window._serializedObject = new SerializedObject(dataObject);
        window._levelBuilder = dataObject;
        window.Show();
    }

    private void OnGUI()
    {
       // _serializedObject.Update();
        _groupsDataProperty = _serializedObject.FindProperty("_levelGroupInfos");

        scrollPos = GUILayout.BeginScrollView(scrollPos, false, true);

        DrawProperty(_groupsDataProperty, true);
        EditorGUILayout.PropertyField(_serializedObject.FindProperty("_root"), false);
        EditorGUILayout.PropertyField(_serializedObject.FindProperty("poolFactory"), false);
        EditorGUILayout.PropertyField(_serializedObject.FindProperty("_layermask"), false);
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Add Group"))
            _groupsDataProperty.arraySize++;
        if (GUILayout.Button("Remove Group"))
            _groupsDataProperty.arraySize--;
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("FindObjects"))
        {
            FindObjectsByMesh(_levelBuilder._root, _levelBuilder._layermask);
            Repaint();
        }


        if (GUILayout.Button("Build"))
        {
            Build();
        }

        EditorGUILayout.EndScrollView();

        _serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(_levelBuilder);
    }

    private void FindObjectsByMesh(Transform root, LayerMask mask)
    {
        var children = root.GetComponentsInChildren<MeshFilter>().Where(child => ((1 << child.gameObject.layer) & mask) != 0);
        _levelObjects.Clear();

        foreach(var child in children)
        {
            if (child.sharedMesh == null) { continue; }
            var name = child.sharedMesh.name;
            if (_levelObjects.ContainsKey(name))
                _levelObjects[name].Add(child.transform);
            else
                _levelObjects.Add(name, new List<Transform>() { child.transform });
        }

        InitializeDataWindow();
    }

    private void InitializeDataWindow()
    {
        var groups = _levelBuilder._levelGroupInfos = new LevelStaticBuilder.LevelBuilderInfo[_levelObjects.Count];

        int i = 0;
        foreach (var pair in _levelObjects)
        {
            var group = groups[i];
            group.id = pair.Key;
            group.maxNumberInCamera = 5;
            group.prefab = PrefabUtility.GetCorrespondingObjectFromSource(pair.Value[0].gameObject);
            group.segmentsToReplace = pair.Value.ToArray();

            groups[i] = group;
            i++;
        }
    }

    private void Build()
    {
        LevelData asset = ScriptableObject.CreateInstance<LevelData>();
        AssetDatabase.CreateAsset(asset, "Assets/MyLevel.asset");
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;

        var data = _levelBuilder.poolFactory.poolDatas = new List<LevelPoolManager.PoolData>();
        foreach (var group in _levelBuilder._levelGroupInfos)
        {

            data.Add(new LevelPoolManager.PoolData
            {
                prefab = group.prefab.transform,
                amount = group.maxNumberInCamera,
                id = group.id,
                autoExpand = true
            });

            var segments = group.segmentsToReplace;
            if (segments == null) { continue; }
            for (int i = 0; i < segments.Length; i++)
            {
                if (segments[i] == null) { continue; }

                asset._levelElements.Add(new LevelData.LevelElementInfo
                {
                    id = group.id,
                    transformable = segments[i].transform.ToTransformable(),
                    sector = FlatSectorDefinition.GetSectorPosition(segments[i].position)
                });
            } 
        }
        EditorUtility.SetDirty(asset);
        AssetDatabase.SaveAssets();
    }

    private void DrawProperty(SerializedProperty prop, bool drawChildren)
    {
        string lastPropPath = string.Empty;
        foreach(SerializedProperty p in prop)
        {
            if(p.isArray && p.propertyType == SerializedPropertyType.Generic)
            {
                EditorGUILayout.BeginHorizontal();
                p.isExpanded = EditorGUILayout.Foldout(p.isExpanded, p.displayName);
                EditorGUILayout.EndHorizontal();

                if (p.isExpanded)
                {
                    EditorGUI.indentLevel++;
                    DrawProperty(p, drawChildren);
                    EditorGUI.indentLevel--;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(lastPropPath) && p.propertyPath.Contains(lastPropPath)){ continue; };
                lastPropPath = p.propertyPath;
                EditorGUILayout.PropertyField(p, drawChildren);
            }
        }
    }

}
#endif
