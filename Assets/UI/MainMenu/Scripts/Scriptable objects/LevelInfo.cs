using UnityEngine;


[CreateAssetMenu(fileName = "NewLevel")]
public class LevelInfo : ScriptableObject
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private string name;
    [SerializeField] private string progress;


    public LocationInfo GetCopy() => new LocationInfo(sprite, name, progress);
}


public struct LocationInfo
{
    public Sprite Sprite { get; }
    public string Name { get; }
    public string Progress { get; }

    public LocationInfo(Sprite sprite, string name, string progress)
    {
        Sprite = sprite;
        Name = name;
        Progress = progress;
    }
}