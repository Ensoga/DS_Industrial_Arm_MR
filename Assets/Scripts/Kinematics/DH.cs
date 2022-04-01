using System;
using System.Collections.Generic;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;

using UnityEngine;

public static class DH {

    [Serializable]
    public class Param
    {
        public float D;
        public float Theta;
        public float A;
        public float Alpha;
        
        public JointType Type;
    }

    public static Matrix4x4 FK(Vector<float> jointValues, List<Param> dhParams, bool firstDTheta = true, bool lastAAlpha = true)
    {
        Matrix4x4 forward = Matrix4x4.identity;
        for (int i = 0; i < dhParams.Count; i++)
            forward *= FK(jointValues[i], dhParams[i], i > 0 || firstDTheta, i < dhParams.Count - 1 || lastAAlpha);

        return forward;
    }

    public static Matrix4x4 FK(float jointValue, Param dhParam, bool dTheta = true, bool aAlpha = true)
    {
        float d = dTheta ? dhParam.D + (dhParam.Type == JointType.Prismatic ? jointValue : 0) : 0;
        float theta = dTheta ? dhParam.Theta + (dhParam.Type == JointType.Revolute ? jointValue : 0) : 0;
        float a = aAlpha ? dhParam.A : 0;
        float alpha = aAlpha ? dhParam.Alpha : 0;

        float st = Mathf.Sin(theta);
        float ct = Mathf.Cos(theta);
        float sa = Mathf.Sin(alpha);
        float ca = Mathf.Cos(alpha);

        return new Matrix4x4()
        {
            m00 = ct, m01 = -st * ca, m02 = st * sa, m03 = a * ct,
            m10 = st, m11 = ct * ca, m12 = -ct * sa, m13 = a * st,
            m20 = 0, m21 = sa, m22 = ca, m23 = d,
            m30 = 0, m31 = 0, m32 = 0, m33 = 1
        };
    }
    
    public static Vector<float> GetFreeMode(Param dhParam)
    {
        switch(dhParam.Type)
        {
            case JointType.Revolute: return new DenseVector(new float[] { 0, 0, 1, 0, 0, 0 });
            case JointType.Prismatic: return new DenseVector(new float[] { 0, 0, 0, 0, 0, 1 });
        }
        throw new ArgumentException($"DH-Parameters does not support JointType: {dhParam.Type}");
    }

    public static Matrix<float> Jacobian(Vector<float> jointValues, List<Param> dhParams)
    {
        // Frame k is attached to the tool with global rotation
        Matrix4x4 k = FK(jointValues, dhParams);
        k.m03 = 0; k.m13 = 0; k.m23 = 0;
        
        Matrix<float> J = new DenseMatrix(6, 6);
        Matrix<float> X = Kinematics.SpatialTransform(k);
        for (int i = dhParams.Count - 1; i >= 0; --i)
        {
            Param dhParam = dhParams[i];
            float jointValue = jointValues[i];
            X *= Kinematics.InverseSpatialTransform(FK(jointValue, dhParam));
            J.SetColumn(i, X * GetFreeMode(dhParam));
        }

        return J;
    }

    public static Matrix<float> JacobianWithTool(Vector<float> jointValues, List<Param> dhParams, Matrix4x4 tool)
    {
        // Frame k is attached to the tool with global rotation
        Matrix4x4 k = tool;
        k.m03 = 0; k.m13 = 0; k.m23 = 0;
        Matrix4x4 dh2k = FK(jointValues, dhParams).inverse * tool;

        Matrix<float> J = new DenseMatrix(6, 6);
        Matrix<float> X = Kinematics.SpatialTransform(k);
        for (int i = dhParams.Count - 1; i >= 0; --i)
        {
            Param dhParam = dhParams[i];
            float jointValue = jointValues[i];
            if (i == dhParams.Count - 1)
                X *= Kinematics.InverseSpatialTransform(dh2k);
            X *= Kinematics.InverseSpatialTransform(FK(jointValue, dhParam));
            J.SetColumn(i, X * GetFreeMode(dhParam));
        }

        return J;
    }
}
