using System;
using System.Collections.Generic;
using System.Linq;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;

using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(UrdfRobot))]
public class URController : MonoBehaviour
{
    [Header("Robot configuration")]

    public UrdfRobot Robot;
    public Transform Origin;
    public GameObject FKMarker;

    public List<DH.Param> DHParams = new List<DH.Param>();
    public URConfiguration IKConfig = new URConfiguration();

    [Header("Robot control")]

    public List<float> JointTarget = new List<float>();
    public GameObject CartesianTarget;

    public TargetType CurrentTarget = TargetType.Cartesian;

    public bool RunJoint = false;
    public bool RunLinear = false;
    public bool SnapToTarget = false;

    [Header("Velocities")]

    public float PositionVelocity = 1; // m/s
    public float RotationVelocity = 90; // degrees/s
    public float JointVelocity = 360; // degrees/s

    [Header("Other settings")]

    public float SingularityThreshold = 0.01f;

    void Start()
    {
        Robot = GetComponent<UrdfRobot>();

        foreach (var joint in Robot.GetComponentsInChildren<HingeJoint>())
        {
            joint.useMotor = false;
            joint.useSpring = false;
        }
    }

    private void Update()
    {
        if (FKMarker != null)
        {
            Vector<float> currentJoints = new DenseVector(Robot.Values.ToArray());
            Matrix4x4 current = Origin.localToWorldMatrix * DH.FK(currentJoints, DHParams).ToLHWorld();
            FKMarker.transform.position = current.GetColumn(3);
            FKMarker.transform.rotation = current.rotation;
        }
    }

    void FixedUpdate()
    {

        if (CurrentTarget == TargetType.Cartesian && CartesianTarget == null ||
            CurrentTarget == TargetType.Joint && Robot.Values.Count != JointTarget.Count)
            return;

        Vector<float> currentJoints = new DenseVector(Robot.Values.ToArray());
        Matrix4x4 forward = DH.FK(currentJoints, DHParams);
        Matrix<float> jacobian = DH.Jacobian(currentJoints, DHParams);

        if (RunLinear)
        {
            Matrix4x4 target;
            if (CurrentTarget == TargetType.Cartesian)
            {
                target = LocalTransformMatrix(CartesianTarget.transform).ToRHWorld();
            }
            else
            {
                Vector<float> targetJoints = new DenseVector(JointTarget.ToArray());
                target = DH.FK(targetJoints, DHParams);
            }

            MoveLinear(forward, target, jacobian);
        }
        else if (RunJoint || SnapToTarget)
        {
            Vector<float> targetJoints;
            if (CurrentTarget == TargetType.Cartesian)
            {
                Matrix4x4 target = LocalTransformMatrix(CartesianTarget.transform).ToRHWorld();
                URConfiguration config = IKConfig;
                targetJoints = IK(target, config) ?? currentJoints;
            }
            else
            {
                targetJoints = new DenseVector(JointTarget.ToArray());
            }

            if (targetJoints == null)
                return;

            if(RunJoint)
            {
                MoveJoint(currentJoints, targetJoints, jacobian);
            }
            else if(SnapToTarget)
            {
                for (int i = 0; i < targetJoints.Count; ++i)
                    Robot.Values[i] = targetJoints[i];
            }
        }
    }

    private void MoveLinear(Matrix4x4 current, Matrix4x4 target, Matrix<float> jacobian)
    {
        if (IsSingularMatrix(jacobian))
        {
            Debug.Log("Inside singularity");
            RunLinear = false;
            return;
        }

        // dP Delta Position
        Vector3 dP = target.GetColumn(3) - current.GetColumn(3);

        // dR Delta Rotation, converts quaternion to angleaxis
        Quaternion dRQ = target.rotation * Quaternion.Inverse(current.rotation);
        dRQ.ToAngleAxis(out float dRAngle, out Vector3 dRAxis);
        Vector3 dR = dRAngle * Mathf.Deg2Rad * dRAxis;

        float positionRatio = dP.magnitude / PositionVelocity;
        float rotationRatio = dR.magnitude / (RotationVelocity * Mathf.Deg2Rad);
        float ratio = Mathf.Max(positionRatio, rotationRatio);
        bool endofMotion = Time.deltaTime / ratio >= 1;

        Vector<float> spatialVelocity = new DenseVector(new float[] { dR[0], dR[1], dR[2], dP[0], dP[1], dP[2] }) * (endofMotion ? 1 : Time.deltaTime / ratio);
        Vector<float> jointVelocity = jacobian.Inverse() * spatialVelocity;

        float jointRatio = JointVelocity * Mathf.Deg2Rad * Time.deltaTime / jointVelocity.AbsoluteMaximum();
        if (jointRatio < 1) jointVelocity *= jointRatio;

        for (int i = 0; i < jointVelocity.Count; ++i)
            Robot.Values[i] += jointVelocity[i];

        if (IsSingularJointPosition(new DenseVector(Robot.Values.ToArray()), DHParams))
        {
            Debug.Log("Close to singularity");
            RunLinear = false;
            return;
        }
    }

    private void MoveJoint(Vector<float> current, Vector<float> target, Matrix<float> jacobian)
    {
        Vector<float> jointDiff = target - current;
        Vector<float> spatialVelocity = jacobian * jointDiff;
        Vector<float> posVel = spatialVelocity.SubVector(3, 3);
        Vector<float> rotVel = spatialVelocity.SubVector(0, 3);

        float positionRatio = (float)posVel.L2Norm() / PositionVelocity;
        float rotationRatio = (float)rotVel.L2Norm() / (RotationVelocity * Mathf.Deg2Rad);
        float ratio = Mathf.Max(positionRatio, rotationRatio);
        bool endofMotion = Time.deltaTime / ratio >= 1;

        Vector<float> jointVelocity = jointDiff * (endofMotion ? 1 : Time.deltaTime / ratio);

        float jointRatio = JointVelocity * Mathf.Deg2Rad * Time.deltaTime / jointVelocity.AbsoluteMaximum();
        if (jointRatio < 1) jointVelocity *= jointRatio;

        for (int i = 0; i < jointVelocity.Count; ++i)
            Robot.Values[i] += jointVelocity[i];
    }

    private bool IsSingularJointPosition(Vector<float> jointValues, List<DH.Param> dhParams)
    {
        return IsSingularMatrix(DH.Jacobian(jointValues, dhParams));
    }

    private bool IsSingularMatrix(Matrix<float> matrix)
    {
        return Mathf.Abs(matrix.Determinant()) < SingularityThreshold;
    }

    private Matrix4x4 LocalTransformMatrix(Transform transform)
    {
        return Origin.transform.worldToLocalMatrix * transform.localToWorldMatrix;
    }

    private Vector<float> IK(Matrix4x4 T06, URConfiguration configuration)
    {
        Vector<float> result = new DenseVector(6);

        float a2 = DHParams[1].A;
        float a3 = DHParams[2].A;
        float d4 = DHParams[3].D;
        float d6 = DHParams[5].D;

        Vector3 x06 = T06.GetColumn(0);
        Vector3 y06 = T06.GetColumn(1);
        Vector3 z06 = T06.GetColumn(2);
        Vector3 p06 = T06.GetColumn(3);
        Vector3 p05 = p06 - d6 * z06;

        float shoulderSign = configuration.ShoulderRight ? -1 : 1;
        float elbowSign = configuration.ElbowDown ? -shoulderSign : shoulderSign;
        float wristSign = configuration.WristOutUp ? shoulderSign : -shoulderSign;

        float R = Mathf.Sqrt(p05.x * p05.x + p05.y * p05.y);
        if (R < d4)
        {
            Debug.Log("Singularity: Target is too close to the center of the robot looking at the xy-plane");
            return null;
        }

        float omega1 = Mathf.Atan2(p05.y, p05.x);
        float omega2 = Mathf.Acos(d4 / R);
        float th1 = omega1 + shoulderSign * omega2 + Mathf.PI / 2;
        result[0] = th1;

        float s1 = Mathf.Sin(th1);
        float c1 = Mathf.Cos(th1);
        float th5 = wristSign * Mathf.Acos(Mathf.Clamp((p06.x * s1 - p06.y * c1 - d4) / d6, -1, 1));
        if (Mathf.Abs(th5) < 1e-2f || Mathf.Abs(th5 - Mathf.PI) < 1e-2f || Mathf.Abs(th5 + Mathf.PI) < 1e-2f)
        {
            Debug.Log("Singularity: Target is within approx 0.57 degrees where the 4:th and 5:th links aligns");
            return null;
        }
        result[4] = th5;

        float s5 = Mathf.Sin(th5);
        float th6 = Mathf.Atan2((-y06.x * s1 + y06.y * c1) / s5, -(-x06.x * s1 + x06.y * c1) / s5);
        result[5] = th6;

        Matrix4x4 T46 = DH.FK(0, DHParams[3]) * DH.FK(th5, DHParams[4]) * DH.FK(th6, DHParams[5]);
        Matrix4x4 T01 = DH.FK(th1, DHParams[0]) * DH.FK(0, DHParams[1], true, false);
        Matrix4x4 T14 = T01.inverse * T06 * T46.inverse;

        Vector3 p14 = T14.GetColumn(3);
        float c234 = T14.m00;
        float s234 = T14.m10;

        float c3 = (p14.x * p14.x + p14.y * p14.y - a2 * a2 - a3 * a3) / (2 * a2 * a3);
        if (c3 < -1 || c3 > 1)
        {
            Debug.Log("Out of reach: Target is too far away from the robot");
            return null;
        }

        float s3 = elbowSign * Mathf.Sqrt(1 - c3 * c3);

        float th3 = Mathf.Atan2(s3, c3);
        result[2] = th3;

        if (Mathf.Abs(th3) < 1e-2f)
        {
            Debug.Log("Singularity: Target is within approx 0.57 degrees where the 2:nd and 3:rd links aligns");
            return null;
        }

        float k1 = a2 + a3 * c3;
        float k2 = a3 * s3;

        float compensation = elbowSign < 0 ? (shoulderSign < 0 ? 0 : 2 * Mathf.PI) : (shoulderSign > 0 ? 0 : -2 * Mathf.PI);
        float th2 = Mathf.Atan2(p14.y, p14.x) - Mathf.Atan2(k2, k1) + compensation;
        float th4 = Mathf.Atan2(s234, c234) - th3 - th2;
        result[1] = th2;
        result[3] = th4;

        return new DenseVector(new float[] { th1, th2, th3, th4, th5, th6 });
    }
}
