﻿using UnityEngine;
using System.Collections;

public class TransformUtil 
{
    /// <summary>
    /// 透视坐标转正交坐标
    /// 将z坐标合并到y中
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="angle">0 <= camera euler angle x < 90</param>
    /// <returns></returns>
    public static Vector2 Position3DZ22DY(Vector3 origin, float angle = 45)
    {
        return new Vector3(origin.x, origin.y + origin.z * Mathf.Tan((90 - angle) * Mathf.PI / 180), 0);
    }

    /// <summary>
    /// 投影到XY平面
    /// </summary>
    /// <param name="origin"></param>
    /// <returns></returns>
    public static Vector3 ProjectByZ(Vector3 origin)
    {
        return new Vector3(origin.x, origin.y, 0);
    }
}
