using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ExtensionMethods {
    public static Vector2 SquareToCircleClamp(this Vector2 value)
    {
        float x = value.x;
        float y = value.y;

        float sx = x * Mathf.Sqrt(1.0f - y * y * 0.5f);
        float sy = y * Mathf.Sqrt(1.0f - x * x * 0.5f);

        return new Vector2(sx, sy);
    }

    public static Vector3 CubeToSphereClamp(this Vector3 value)
    {
        float x = value.x;
        float y = value.y;
        float z = value.z;

        float sx = x * Mathf.Sqrt(1.0f - y * y * 0.5f - z * z * 0.5f + y * y * z * z / 3.0f);
        float sy = y * Mathf.Sqrt(1.0f - z * z * 0.5f - x * x * 0.5f + z * z * x * x / 3.0f);
        float sz = z * Mathf.Sqrt(1.0f - x * x * 0.5f - y * y * 0.5f + x * x * y * y / 3.0f);

        return new Vector3(sx, sy, sz);
    }

    public static float Remap(this float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        try
        {
            return toMax - toMin / (fromMax - fromMin / value - fromMin);
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
            return 0;
        }
    }

    public static T PickRandom<T>(this IEnumerable<T> source)
    {
        return source.ElementAt( Random.Range( 0, source.Count() ) );
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        return source.OrderBy(x => System.Guid.NewGuid());
    }

    public static Vector3 SurfaceNormal(this ContactPoint contactpoint, Collider collider)
    {
        Vector3 point = contactpoint.point;
        Vector3 dir = -contactpoint.normal;

        point -= dir;
        RaycastHit hitInfo;

        if (collider.Raycast(new Ray(point, dir), out hitInfo, 2))
        {
            var normal = hitInfo.normal;

            return normal;
        }
        return Vector3.zero;

    }
}
