using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMenu : MonoBehaviour
{
    GameObject baselink;
    private bool RobotVisible = true;

    GameObject MR_camera;
    Varjo_Marker_Manager varjo_Marker_Manager;

    private void Awake()
    {
        baselink = GameObject.Find("base_link");
        
        MR_camera = GameObject.Find("XRRig");
        varjo_Marker_Manager = MR_camera.GetComponent<Varjo_Marker_Manager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        baselink.transform.localScale = new Vector3(1, 1, 1);
        //baselink.SetActive(RobotVisible);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void VisibleButton()
    {
        //RobotVisible = !RobotVisible;
        //baselink.SetActive(RobotVisible);
        if(RobotVisible)
        {
            baselink.transform.localScale = new Vector3(0, 0, 0);
            RobotVisible = false;
        }
        else
        {
            baselink.transform.localScale = new Vector3(1, 1, 1);
            RobotVisible = true;
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
