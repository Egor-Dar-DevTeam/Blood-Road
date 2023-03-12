using System.Collections.Generic;
using UnityEngine;

public class LevelPoolManager : MonoBehaviour
{
    public List<PoolData> poolDatas;

    [System.Serializable]
    public struct PoolData
    {
        public Transform prefab;
        public bool autoExpand;
        public int amount;
        public string id;
    }

    private Dictionary<string, PoolMono<Transform>> _pools = new Dictionary<string, PoolMono<Transform>>();
    private Dictionary<LevelData.Transformable, Transform> _culledObjects = new Dictionary<LevelData.Transformable, Transform>();

    public static LevelPoolManager Instance { get; private set; }

    private void Awake()
    {
        foreach(var data in poolDatas)
        {
            var pool = new PoolMono<Transform>(data.prefab, data.amount, transform);
            pool.AutoExpand = data.autoExpand;
            _pools.Add(data.id, pool);    
        }
        Instance = this;
    }

    public void Append(LevelData.Transformable transformable, string id)
    {
        var element = _pools[id].FreeElement;
        element.CopyData(transformable);

        if (IsCulled(transformable)) { return; }
        _culledObjects.Add(transformable, element);
    }

    public void Remove(LevelData.Transformable transformable)
    {
        if (IsCulled(transformable))
        {
            _culledObjects[transformable].gameObject.SetActive(false);
            _culledObjects.Remove(transformable);
        }
    }

    public bool IsCulled(LevelData.Transformable transformable)
    {
        return _culledObjects.ContainsKey(transformable);
    }
}
