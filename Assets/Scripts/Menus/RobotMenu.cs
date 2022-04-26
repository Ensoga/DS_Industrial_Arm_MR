using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMenu : MonoBehaviour
{
    GameObject baselink;
    private bool RobotVisible = true;

    private void Awake()
    {
        baselink = GameObject.Find("base_link");
    }

    // Start is called before the first frame update
    void Start()
    {
        baselink.SetActive(RobotVisible);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void VisibleButton()
    {
        RobotVisible = !RobotVisible;
        baselink.SetActive(RobotVisible);
    }
}
