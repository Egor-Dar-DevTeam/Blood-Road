using System;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class EventsDeleteInFBX : AssetPostprocessor
    {
        [SerializeField] private GameObject fbx;

        private void OnPreprocessModel()
        {
            
        }
    }
}