using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMenu : MonoBehaviour
{
    //List<GameObject> RobotVisuals = new List<GameObject>();
    GameObject[] RobotVisuals;
    List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
    private bool RobotVisible = true;

    GameObject MR_camera;
    Varjo_Marker_Manager varjo_Marker_Manager;

    private void Awake()
    {
        RobotVisuals = GameObject.FindGameObjectsWithTag("RobotVisual");
        foreach (GameObject RobotVisual in RobotVisuals)
        {
            meshRenderers.Add(RobotVisual.GetComponent<MeshRenderer>());
        }

        MR_camera = GameObject.Find("XRRig");
        varjo_Marker_Manager = MR_camera.GetComponent<Varjo_Marker_Manager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void VisibleButton()
    {
        //RobotVisible = !RobotVisible;
        //baselink.SetActive(RobotVisible);
        RobotVisible = !RobotVisible;
        if (RobotVisible)
        {
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                meshRenderer.enabled = true;
            }
        }
        else
        {
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                meshRenderer.enabled = false;
            }
        }
        
    }

    public void ScanButton()
    {
        varjo_Marker_Manager.enabled = true;
    }

    public void LockButton()
    {
        varjo_Marker_Manager.enabled = false;
    }

}
