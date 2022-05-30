using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CurvePath : MonoBehaviour
{
    public abstract float TVelocity { get; }
    public abstract float TAcceleration { get; }
    public abstract float TRampStart { get; }
    public abstract float TRampEnd { get; }

    public abstract Vector3 CalcPosition(float t);
    public abstract Quaternion CalcRotation(float t);
}
