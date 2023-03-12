using System.Collections;
using System.Collections.Generic;
using Characters;
using Characters.Enemy;
using Scripts;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(DefaultEnemy))]

public class BaseCharacterEditor : UnityEditor.Editor
{
    private BaseCharacter _baseCharacter;

    private void OnEnable()
    {
        _baseCharacter = target as BaseCharacter;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Set CharacterController")) _baseCharacter.SetCharacterController();
    }

}
