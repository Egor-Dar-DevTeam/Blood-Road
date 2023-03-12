using UnityEngine;

public class LevelStaticBuilder : MonoBehaviour
{
    public Transform _root;
    public LevelBuilderInfo[] _levelGroupInfos;
    public LevelPoolManager poolFactory;
    public LayerMask _layermask;

    [System.Serializable]
    public struct LevelBuilderInfo
    {
        public string id;
        public GameObject prefab;
        public int maxNumberInCamera;
        public Transform[] segmentsToReplace;
    }
}

public static class TransformExtension
{
    public static void CopyData(this Transform origin, Transform from)
    {
        origin.position = from.position;
        origin.rotation = from.rotation;
        origin.localScale = from.localScale;
    }

    public static void CopyData(this Transform origin, LevelData.Transformable from)
    {
        origin.position = from.position;
        origin.rotation = from.rotation;
        origin.localScale = from.localScale;
    }

    public static LevelData.Transformable ToTransformable(this Transform origin)
    {
        return new LevelData.Transformable
        {
            position = origin.position,
            rotation = origin.rotation,
            localScale = origin.localScale
        };
      
    }
}