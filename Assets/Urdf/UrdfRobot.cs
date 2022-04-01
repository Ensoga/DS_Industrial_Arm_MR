using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteAlways]
public class UrdfRobot : MonoBehaviour
{
    public bool BaseIsKinematic = false;
    private bool? _baseIsKinematic = null;

    public bool BaseUseGravity = false;
    private bool? _baseUseGravity = null;

    public bool LinksAreKinematic = false;
    private bool? _linksAreKinematic = null;

    public bool LinksUseGravity = false;
    private bool? _linksUseGravity = null;

    public List<Urdf.Joint> AllJoints;
    public List<Urdf.Joint> MovableJoints;
    public List<float> Values;


    void Update()
    {
        if (_baseIsKinematic != BaseIsKinematic)
        {
            _baseIsKinematic = BaseIsKinematic;
            GetComponentInChildren<Rigidbody>().isKinematic = BaseIsKinematic;
        }

        if (_linksAreKinematic != LinksAreKinematic)
        {
            _linksAreKinematic = LinksAreKinematic;
            foreach (Rigidbody rigidBody in GetComponentsInChildren<Rigidbody>().Skip(1))
            {
                rigidBody.isKinematic = LinksAreKinematic;
            }
        }

        if (_baseUseGravity != BaseUseGravity)
        {
            _baseUseGravity = BaseUseGravity;
            GetComponentInChildren<Rigidbody>().useGravity = BaseUseGravity;
        }

        if (_linksUseGravity != LinksUseGravity)
        {
            _linksUseGravity = LinksUseGravity;
            foreach (Rigidbody rigidBody in GetComponentsInChildren<Rigidbody>().Skip(1))
            {
                rigidBody.useGravity = LinksUseGravity;
            }
        }

        for (int i = 0; i < MovableJoints.Count; ++i)
        {
            // Updates the robot position with its connected gameobject in the setter, also clamps the value
            MovableJoints[i].Value = Values[i];

            // If the value is clamped, then we update the old value
            Values[i] = MovableJoints[i].Value;
        }
    }
}
