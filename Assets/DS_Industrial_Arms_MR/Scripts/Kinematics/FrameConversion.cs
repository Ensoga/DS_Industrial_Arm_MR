using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FrameConversion
{
    private static Matrix4x4 Tlh = new Matrix4x4(Vector3.left, Vector3.back, Vector3.up, new Vector4(0, 0, 0, 1));
    private static Matrix4x4 Trh = new Matrix4x4(Vector3.left, Vector3.forward, Vector3.down, new Vector4(0, 0, 0, 1));
    private static Matrix4x4 Tw = new Matrix4x4(Vector3.right, Vector3.forward, Vector3.up, new Vector4(0, 0, 0, 1));

    public static Matrix4x4 ToLH(this Matrix4x4 matrix)
    {
        return Tlh * matrix * Trh;
    }

    public static Matrix4x4 ToRH(this Matrix4x4 matrix)
    {
        return Trh * matrix * Tlh;
    }

    public static Matrix4x4 ToLHWorld(this Matrix4x4 matrix)
    {
        return Tw * matrix * Tw;
    }

    public static Matrix4x4 ToRHWorld(this Matrix4x4 matrix)
    {
        return Tw * matrix * Tw;
    }

    public static Vector4 ToLH(this Vector4 vector)
    {
        return new Vector4(-vector.x, vector.z, -vector.y, vector.w);
    }

    public static Vector4 ToRH(this Vector4 vector)
    {
        return new Vector4(-vector.x, -vector.z, vector.y, vector.w);
    }

    public static Vector4 ToLHWorld(this Vector4 vector)
    {
        return new Vector4(vector.x, vector.z, vector.y, vector.w);
    }

    public static Vector4 ToRHWorld(this Vector4 vector)
    {
        return new Vector4(vector.x, vector.z, vector.y, vector.w);
    }

    public static Vector3 ToLH(this Vector3 vector)
    {
        return new Vector3(-vector.x, vector.z, -vector.y);
    }

    public static Vector3 ToRH(this Vector3 vector)
    {
        return new Vector3(-vector.x, -vector.z, vector.y);
    }

    public static Vector3 ToLHWorld(this Vector3 vector)
    {
        return new Vector3(vector.x, vector.z, vector.y);
    }

    public static Vector3 ToRHWorld(this Vector3 vector)
    {
        return new Vector3(vector.x, vector.z, vector.y);
    }

    public static Vector4 ToLHAxis(this Vector4 axis)
    {
        return new Vector4(axis.x, axis.z, -axis.y, axis.w);
    }

    public static Vector4 ToRHAxis(this Vector4 axis)
    {
        return new Vector4(axis.x, -axis.z, axis.y, axis.w);
    }

    public static Vector3 ToLHAxis(this Vector3 axis)
    {
        return new Vector3(axis.x, -axis.z, axis.y);
    }

    public static Vector3 ToRHAxis(this Vector3 axis)
    {
        return new Vector3(axis.x, axis.z, -axis.y);
    }

    public static Quaternion ToRH(this Quaternion quaternion)
    {
        return new Quaternion(quaternion.x, quaternion.z, -quaternion.y, quaternion.w);
    }

    public static Quaternion ToLH(this Quaternion quaternion)
    {
        return new Quaternion(quaternion.x, -quaternion.z, quaternion.y, quaternion.w);
    }
}