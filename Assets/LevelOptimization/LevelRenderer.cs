using System.Collections;
using UnityEngine;

public class LevelRenderer : MonoBehaviour
{
    [SerializeField]
    private LevelData _data;

    [SerializeField]
    private int _neighboursCount;

    [SerializeField]
    private Transform _target;

    private LevelPoolManager Manager => LevelPoolManager.Instance;
    private int _cachedSec;

    private int Current => FlatSectorDefinition.GetSectorPosition(_target.TransformPoint(Vector3.zero));

    private void OnEnable()
    {
        _data.Init();
    }

    private void Start()
    {
       
        StartCoroutine(Draw());
    }

    private IEnumerator Draw()
    {
        _cachedSec = Current;
        EnableSectors(Current - _neighboursCount, Current + _neighboursCount);
        while (true)
        {
            RenderInSectors();
            yield return new WaitForSeconds(0.56f);
        }
    }

    private void RenderInSectors()
    {
        if (Current == _cachedSec) { return; }
        //if(sec != _currentSector)
        //{
        //    DisableSector(_currentSector + (_currentSector - sec) * _neighboursCount);
        //    _currentSector = sec;
        ////}
        //Debug.Log("ASDASd");

        //_currentSector = sec;
        //DisableSectors(_currentSector - _neighboursCount, _currentSector + _neighboursCount);

        //LevelData.Transformable[] transformables;
        OnSectorChanged(Current - _cachedSec);

        _cachedSec = Current;
    }

    private void OnSectorChanged(int offset)
    {
        DisableSector(_cachedSec - offset * _neighboursCount);
        EnableSector(_cachedSec + offset * _neighboursCount);
       // DisableSectors(_currentSector - _neighboursCount, _currentSector + _neighboursCount);
    }




    private void EnableSector(int sector)
    {
        var has = _data.TryGetTransformables(sector, out LevelData.Transformable[] transformables);
        if (has == false) { return; }

        foreach(var tr in transformables)
        {
            Render(tr);
        }
    }

    //private void TryRender(LevelData.Transformable[] transformables)
    //{
    //    foreach(var tr in transformables)
    //    {
    //        //if (isOcluded(tr.position))
    //        //    Render(tr);
    //        //else
    //        //    Skip(tr);

    //        Render(tr);
    //    }
    //}

    //private bool InViewPortRange(Vector2 vp)
    //{
    //    return vp.x >= 0 && vp.x <= 1f && vp.y >= 0 && vp.y <= 1f;
    //}

    //private bool isOcluded(Vector3 position)
    //{
    //    var vp = _targetRenderer.WorldToViewportPoint(position);
    //    return InViewPortRange(vp);
    //}

    private void EnableSectors(int from, int to)
    {
        for (int i = from; i <= to; i++)
        {
            EnableSector(i);
        }
    }

    private void Render(LevelData.Transformable transformable)
    {
        //if (Manager.IsCulled(transformable)) { return; }

        Manager.Append(transformable, _data.GetID(transformable));
    }

    private void DisableSector(int sector)
    {
        var hasSector = _data.TryGetTransformables(sector, out LevelData.Transformable[] tr);
        if(hasSector == false) { return; }

        foreach(var t in tr)
        {
            Manager.Remove(t);
        }
    }

    //private void DisableSectors(int ignoreFrom, int ignoreTo)
    //{
    //    foreach(var pair in _data._transformablesInSector)
    //    {
    //        if (pair.Key >= ignoreFrom && pair.Key <= ignoreTo)
    //            continue;

    //        foreach(var tr in pair.Value)
    //        {
    //            Manager.Remove(tr);
    //        }
    //    }
    //}

    //private void Skip(LevelData.Transformable data)
    //{
    //    Manager.Remove(data);
    //}
}
