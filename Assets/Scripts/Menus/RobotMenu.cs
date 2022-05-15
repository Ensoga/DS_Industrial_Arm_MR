using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using HiS.XR;

public class RobotMenu : MonoBehaviour
{
    #region Variables
    [SerializeField] List<GameObject> ButtonList;
    [SerializeField] GameObject ScanButtonUI;
    [SerializeField] GameObject LockButtonUI;
    [SerializeField] GameObject PressToScan;
    [SerializeField] GameObject AfterFirstScan;
    [SerializeField] GameObject AfterLock;
    [SerializeField] GameObject RobotImage;
    [SerializeField] GameObject FreedriveImage;
    [SerializeField] GameObject PressureGaugeSolid;
    [SerializeField] GameObject PressureGaugeVF;
    [SerializeField] GameObject PressureGaugeVB;





    [Header("Submenu")]
    public GameObject submenu;
    public GameObject buttonParent;

    [Header("Move Button")]
    public GameObject moveButton;
    public Material OffMaterial;
    public Material OnMaterial;

    public Vector3 offSetPos = new Vector3(-0.025f, -0.1795f, 0.0738f);
    public Quaternion offSetRot = new Quaternion(-79.248f, 57.918f, 52.085f, 1f);

    private bool submenuAttached;

    GameObject UR10e;
    GameObject[] RobotVisuals;
    List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
    private bool RobotVisible = false;

    GameObject MR_camera;
    Varjo_Marker_Manager varjo_Marker_Manager;
    GameObject WorkObjects;
    GameObject EndEffector;
    private bool VacuumOn = false;

    FreedriveCloudBehaviour freedriveCloudBehaviour;
    private bool _VarjoFreedrive = false;
    private bool _InternalFreedrive;

    #endregion

    private void Awake()
    {
        RobotVisuals = GameObject.FindGameObjectsWithTag("RobotVisual");
        foreach (GameObject RobotVisual in RobotVisuals)
        {
            meshRenderers.Add(RobotVisual.GetComponent<MeshRenderer>());
        }

        UR10e = GameObject.FindGameObjectWithTag("UR10e");
        MR_camera = GameObject.Find("XRRig");
        varjo_Marker_Manager = MR_camera.GetComponent<Varjo_Marker_Manager>();

        EndEffector = GameObject.Find("ee_link");
        WorkObjects = GameObject.Find("WorkObjects");

        freedriveCloudBehaviour = GetComponent<FreedriveCloudBehaviour>();
    }

    // Start is called before the first frame update
    void Start()
    {
        varjo_Marker_Manager.enabled = false;
        LockButtonUI.SetActive(false);
        RobotImage.SetActive(false);
        FreedriveImage.SetActive(false);
        foreach (GameObject Button in ButtonList)
        {
            Button.SetActive(false);
        }
        UR10e.SetActive(RobotVisible);
        ScanButtonUI.SetActive(true);
        PressToScan.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (submenu.GetComponent<InteractionBehaviour>().isGrasped)
        {
            moveButton.GetComponent<MeshRenderer>().material = OnMaterial;
            if (submenuAttached)  
            {
                submenu.transform.parent = null;
                submenuAttached = false;
            }
        }
        else
        {
            moveButton.GetComponent<MeshRenderer>().material = OffMaterial;
        }
        */
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
        varjo_Marker_Manager.enabled = true;
        UR10e.SetActive(true);
        ScanButtonUI.SetActive(false);
        PressToScan.SetActive(false);
        AfterLock.SetActive(false);
        AfterFirstScan.SetActive(true);
        LockButtonUI.SetActive(true);
    }

    public void LockButton()    
    {
        varjo_Marker_Manager.enabled = false;
        AfterFirstScan.SetActive(false);
        LockButtonUI.SetActive(false);
        ScanButtonUI.SetActive(true);
        AfterLock.SetActive(true);
        RobotImage.SetActive(true);
        FreedriveImage.SetActive(true);
        PressureGaugeSolid.SetActive(true);


        foreach (GameObject Button in ButtonList)
        {
            Button.SetActive(true);
        }
    }

    public void VaccumButton()
    {
        VacuumOn = !VacuumOn;
        if (VacuumOn)
        {
            WorkObjects.transform.SetParent(EndEffector.transform);
            // Change Color
            PressureGaugeVB.SetActive(false);//preguntar a Enrique
            PressureGaugeVF.SetActive(true);
        }
        else
        {
            WorkObjects.transform.SetParent(null);
            PressureGaugeVF.SetActive(false);
            PressureGaugeVB.SetActive(true);
        }
    }

    public void FreedriveButton()
    {
        _VarjoFreedrive = !_VarjoFreedrive;
        freedriveCloudBehaviour.SetFreedrive(_VarjoFreedrive);
        
    }

    public void FreedriveButtonFeedback(bool _FreedriveValue)
    {
        // Change Color of the button.
    }
}
