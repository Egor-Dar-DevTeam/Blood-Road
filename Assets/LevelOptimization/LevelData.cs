using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelData : ScriptableObject
{
    public List<LevelElementInfo> _levelElements = new List<LevelElementInfo>();

    private Dictionary<Vector3, string> _idProvider;
    public Dictionary<int, List<Transformable>> _transformablesInSector;

    public void Init()
    {
        _idProvider = new Dictionary<Vector3, string>();
        _transformablesInSector = new Dictionary<int, List<Transformable>>();


        foreach (var element in _levelElements)
        {
            if(_idProvider.ContainsKey(element.transformable.position) == false)
                _idProvider.Add(element.transformable.position, element.id);

            if (_transformablesInSector.ContainsKey(element.sector))
            {
                _transformablesInSector[element.sector].Add(element.transformable);
            }
            else
                _transformablesInSector.Add(element.sector, new List<Transformable>() { element.transformable });
        }
    }

    public string GetID(Transformable transformable)
    {
        return _idProvider[transformable.position];
    }

    public bool TryGetTransformables(int sector, out Transformable[] transformables)
    {
        transformables = null;
        if (_transformablesInSector.ContainsKey(sector) == false) { return false; }

        transformables = _transformablesInSector[sector].ToArray();
        return true;
    }


    [System.Serializable]
    public struct LevelElementInfo
    {
        public string id;
        public Transformable transformable;
        public int sector;
    }

    [System.Serializable]
    public struct Transformable
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 localScale;
    }
}
