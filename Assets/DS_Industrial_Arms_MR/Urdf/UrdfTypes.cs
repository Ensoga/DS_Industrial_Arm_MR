using System;

using UnityEngine;

namespace Urdf
{
    [Serializable]
    public enum JointType
    {
        Revolute,
        Continuous,
        Prismatic,
        Fixed,
        Floating,
        Planar
    }

    [Serializable]
    public class JointLimit
    {
        public float Lower;
        public float Upper;
        public float Effort;
        public float Velocity;
    }

    [Serializable]
    public class Joint
    {
        public string Name;
        public JointType Type;

        public Pose Origin;
        public JointLimit Limit;
        public Vector3 Axis;

        public float Offset;

        private float value1;
        public float Value { get { return value1; } set { value1 = value; Update(); } }
        public float Value1 { get { return value1; } set { value1 = value; Update(); } }

        private float value2;
        public float Value2 { get { return value2; } set { value2 = value; Update(); } }

        private float value3;
        public float Value3 { get { return value3; } set { value3 = value; Update(); } }

        private float value4;
        public float Value4 { get { return value4; } set { value4 = value; Update(); } }

        private float value5;
        public float Value5 { get { return value5; } set { value5 = value; Update(); } }

        private float value6;
        public float Value6 { get { return value6; } set { value6 = value; Update(); } }

        public GameObject GameObject;

        public Joint(string name, JointType type)
        {
            Name = name;
            Type = type;

            Axis = Vector3.right;
            Origin = new Pose(Vector3.zero, Quaternion.identity);
        }

        private void Update()
        {
            if (Type == JointType.Fixed)
                return;

            if (Type == JointType.Revolute)
            {
                if (Limit != null)
                    value1 = Mathf.Clamp(value1, Limit.Lower, Limit.Upper);

                GameObject.transform.localRotation = Quaternion.AngleAxis((value1 + Offset) * Mathf.Rad2Deg, Origin.rotation * Axis) * Origin.rotation;
            }
            else if (Type == JointType.Prismatic)
            {
                if (Limit != null)
                    value1 = Mathf.Clamp(value1, Limit.Lower, Limit.Upper);

                GameObject.transform.localPosition = Origin.position + (value1 + Offset) * Axis;
            }
            else
            {
                throw new NotImplementedException("The joint type " + Type + ", has not been implemented.");
            }
        }
    }
}