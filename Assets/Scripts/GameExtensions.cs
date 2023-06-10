using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameExtensions
{
    public static Vector3 ToVector3(this Vector2 vec)
    {
        return new Vector3(vec.x, 0, vec.y);
    }

    public static Vector2 ToVector2(this Vector3 vec)
    {
        return new Vector2(vec.x, vec.z);
    }
}
