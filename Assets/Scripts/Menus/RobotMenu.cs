using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMenu : MonoBehaviour
{
    public List<GameObject> ButtonList;
    public GameObject WarningCanvas; 
    public GameObject ScanCanvas;
    public GameObject LockCanvas;

    GameObject UR10e;
    GameObject[] RobotVisuals;
    List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
    private bool RobotVisible = false;

    GameObject MR_camera;
    Varjo_Marker_Manager varjo_Marker_Manager;
    private bool ScanMode = false;
    GameObject WorkObjects;
    GameObject EndEffector;
    private bool VacuumOn = false;

    private void Awake()
    {
        RobotVisuals = GameObject.FindGameObjectsWithTag("RobotVisual");
        foreach (GameObject RobotVisual in RobotVisuals)
        {
            meshRenderers.Add(RobotVisual.GetComponent<MeshRenderer>());
        }

        UR10e = GameObject.Find("ur10_robot");
        MR_camera = GameObject.Find("XRRig");
        varjo_Marker_Manager = MR_camera.GetComponent<Varjo_Marker_Manager>();

        EndEffector = GameObject.Find("ee_link");
        WorkObjects = GameObject.Find("WorkObjects");
    }

    // Start is called before the first frame update
    void Start()
    {
        UR10e.SetActive(RobotVisible);

        foreach (GameObject Button in ButtonList)
        {
            Button.SetActive(false);
        }
        WarningCanvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void VisibleButton()
    {
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
        ScanMode = !ScanMode;
        if (ScanMode)
        {
            varjo_Marker_Manager.enabled = true;
            UR10e.SetActive(true);
            ScanCanvas.SetActive(false);
            LockCanvas.SetActive(true);
            
            foreach (GameObject Button in ButtonList)
            {
                Button.SetActive(false);
            }
            WarningCanvas.SetActive(true);
        }
        else
        {
            varjo_Marker_Manager.enabled = false;
            LockCanvas.SetActive(false);
            ScanCanvas.SetActive(true);

            WarningCanvas.SetActive(false);
            foreach (GameObject Button in ButtonList)
            {
                Button.SetActive(true);
            }
        }
    }

    public void LockButton()    // No borrar hasta haber implementado el robot menu
    {
        varjo_Marker_Manager.enabled = false;
    }

    public void VaccumButton()
    {
        VacuumOn = !VacuumOn;
        if (VacuumOn)
        {
            WorkObjects.transform.SetParent(EndEffector.transform);
        }
        else
        {
            WorkObjects.transform.SetParent(null);
        }
    }
}
