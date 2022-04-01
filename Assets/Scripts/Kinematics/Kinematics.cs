using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;

public static class Kinematics {

    public static Matrix<float> SpatialTransform(Matrix4x4 m)
    {
        Vector<float> p = GetTranslation(m);
        Matrix<float> Sp = SymmetricSkew(p);
        Matrix<float> R = GetRotation(m);
        Matrix<float> SpR = Sp * R;

        return new DenseMatrix(6, 6, new[]
        {
            R[0, 0], R[0, 1], R[0, 2], 0, 0, 0,
            R[1, 0], R[1, 1], R[1, 2], 0, 0, 0,
            R[2, 0], R[2, 1], R[2, 2], 0, 0, 0,
            SpR[0, 0], SpR[0, 1], SpR[0, 2], R[0, 0], R[0, 1], R[0, 2],
            SpR[1, 0], SpR[1, 1], SpR[1, 2], R[1, 0], R[1, 1], R[1, 2],
            SpR[2, 0], SpR[2, 1], SpR[2, 2], R[2, 0], R[2, 1], R[2, 2]
        }).Transpose();
    }

    public static Matrix<float> InverseSpatialTransform(Matrix4x4 m)
    {
        Vector<float> p = GetTranslation(m);
        Matrix<float> Sp = SymmetricSkew(p);
        Matrix<float> R = GetRotation(m).Transpose();
        Matrix<float> RSp = -R * Sp;

        return new DenseMatrix(6, 6, new[]
        {
            R[0, 0], R[0, 1], R[0, 2], 0, 0, 0,
            R[1, 0], R[1, 1], R[1, 2], 0, 0, 0,
            R[2, 0], R[2, 1], R[2, 2], 0, 0, 0,
            RSp[0, 0], RSp[0, 1], RSp[0, 2], R[0, 0], R[0, 1], R[0, 2],
            RSp[1, 0], RSp[1, 1], RSp[1, 2], R[1, 0], R[1, 1], R[1, 2],
            RSp[2, 0], RSp[2, 1], RSp[2, 2], R[2, 0], R[2, 1], R[2, 2]
        }).Transpose();
    }

    public static Vector<float> GetTranslation(Matrix4x4 m)
    {
        return new DenseVector(new float[] { m.m03, m.m13, m.m23 });
    }

    public static Matrix<float> GetRotation(Matrix4x4 m)
    {
        return new DenseMatrix(3, 3, new[]
        {
            m.m00, m.m10, m.m20,
            m.m01, m.m11, m.m21,
            m.m02, m.m12, m.m22
        });
    }

    public static Matrix<float> SymmetricSkew(Vector<float> vector)
    {
        return new DenseMatrix(3, 3, new[]
        {
            0, vector[2], -vector[1],
            -vector[2], 0, vector[0],
            vector[1], -vector[0], 0
        });
    }
}
