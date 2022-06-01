using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;
using Varjo.XR;
using System;

public class Varjo_Marker_Manager : MonoBehaviour
{
    // Serializable struct to make it easy to add tracked objects in the Inspector. 
    [Serializable]
    public struct TrackedObject
    {
        public long id;
        public GameObject gameObject;
        public bool dynamicTracking;
    }

    [HideInInspector] public bool RobotMarkerEnabled;

    // Robot's Offsets
    public float RobotOffSet_x = -0.79F;
    public float RobotOffSet_y = -0.4395F;
    public float RobotOffSet_z = -0.258F;
    public Quaternion quaternion = Quaternion.Euler(0,90,90);

    // AGV's Offsets
    /*public float AGVOffSet_x = 0.0F;
    public float AGVOffSet_y = 0.0F;
    public float AGVOffSet_z = 0.0F;*/




    // A public array for all the tracked objects. 
    public TrackedObject[] trackedObjects = new TrackedObject[1];

    // A list for found markers.
    private List<VarjoMarker> markers = new List<VarjoMarker>();

    // A list for IDs of removed markers.
    private List<long> removedMarkerIds = new List<long>();

    private void OnEnable()
    {
        // Enable Varjo Marker tracking.
        VarjoMarkers.EnableVarjoMarkers(true);
    }

    private void OnDisable()
    {
        // Disable Varjo Marker tracking.
        VarjoMarkers.EnableVarjoMarkers(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if Varjo Marker tracking is enabled and functional.
        if (VarjoMarkers.IsVarjoMarkersEnabled())
        {
            // Get a list of markers with up-to-date data.
            VarjoMarkers.GetVarjoMarkers(out markers);

            // Loop through found markers and update gameobjects matching the marker ID in the array.
            foreach (var marker in markers)
            {
                for (var i = 0; i < trackedObjects.Length; i++)
                {
                    if (trackedObjects[i].id == marker.id)
                    {
                        // This simple marker manager controls only visibility and pose of the GameObjects.
                        trackedObjects[i].gameObject.SetActive(true);

                        /*if (trackedObjects[i].id == 200)
                        {
                            trackedObjects[i].gameObject.transform.localPosition = new Vector3(marker.pose.position.x - AGVOffSet_x, marker.pose.position.y - AGVOffSet_y, marker.pose.position.z - AGVOffSet_z);
                        }*/

                        if (trackedObjects[i].id == 300)
                        {
                            //if (RobotMarkerEnabled)
                            {
                                OverlayRobotDM(marker);
                            }
                            
                        }

                        // Set the marker tracking mode
                        if ((marker.flags == VarjoMarkerFlags.DoPrediction) != trackedObjects[i].dynamicTracking)
                        {
                            if (trackedObjects[i].dynamicTracking)
                            {
                                VarjoMarkers.AddVarjoMarkerFlags(marker.id, VarjoMarkerFlags.DoPrediction);
                            }
                            else
                            {
                                VarjoMarkers.RemoveVarjoMarkerFlags(marker.id, VarjoMarkerFlags.DoPrediction);
                            }
                        }
                    }
                }
                
            }

            // Get a list of IDs of removed markers.
            VarjoMarkers.GetRemovedVarjoMarkerIds(out removedMarkerIds);

            // Loop through removed marker IDs and deactivate gameobjects matching the marker IDs in the array.
            foreach (var id in removedMarkerIds)
            {
                for (var i = 0; i < trackedObjects.Length; i++)
                {
                    if (trackedObjects[i].id == id)
                    {
                        trackedObjects[i].gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    public void OverlayRobotDM(VarjoMarker marker)
    {
        trackedObjects[0].gameObject.transform.localPosition = new Vector3(marker.pose.position.x - RobotOffSet_x, marker.pose.position.y - RobotOffSet_y, marker.pose.position.z - RobotOffSet_z);
        trackedObjects[0].gameObject.transform.localRotation = marker.pose.rotation * quaternion;
    }

}
