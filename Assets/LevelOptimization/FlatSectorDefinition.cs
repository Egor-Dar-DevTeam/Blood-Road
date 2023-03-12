using UnityEngine;

public class FlatSectorDefinition
{
    public enum Axis : short
    {
        X, Y, Z
    }

    public static int _sectorScale = 1;
    public static Axis _axis = Axis.Z;
    public static int GetSectorPosition(Vector3 position)
    {
        float axisValue = 0;
        switch (_axis)
        {
            case Axis.X:
                axisValue = position.x;
                break;
            case Axis.Y:
                axisValue = position.y;
                break;
            case Axis.Z:
                axisValue = position.z;
                break;
        }
        return Mathf.RoundToInt(axisValue / _sectorScale);
    }
}
