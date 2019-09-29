using UnityEngine;

public static class MyExtensions
{
    public static Vector3 ToVector3(this Vector2Int v)
    {
        return new Vector3(v.x, v.y, 0);
    }
}
