using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolMenu : MonoBehaviour
{
    [SerializeField] GameObject VisibleText;
    [SerializeField] GameObject HiddenText;
    [SerializeField] GameObject toolsModelMenu;
    [SerializeField] GameObject VacuumOFF;
    [SerializeField] GameObject VacuumON;

    GameObject tools;
    [HideInInspector] public List<GameObject> ToolsList = new List<GameObject>();
    List<GameObject> ToolsModelMenuList = new List<GameObject>();
    [HideInInspector] public int index = 0;
    private bool _visible = false;

    [HideInInspector] public bool VacuumSelected;
    GameObject robotMenuToggle;
    RobotMenu robotMenu;
    
    [SerializeField] Vector3 _rotation;
    [SerializeField] float _speed;
    [SerializeField] float _limit;
    [SerializeField] float _speedUD;

    private void Awake()
    {
        tools = GameObject.FindGameObjectWithTag("Tools");
        robotMenuToggle = GameObject.Find("Robot Menu Toggle");
        robotMenu = robotMenuToggle.GetComponent<RobotMenu>();
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i=0; i < tools.transform.childCount; i++)
        {
            ToolsList.Add(tools.transform.GetChild(i).gameObject);
            ToolsList[i].SetActive(false);
            ToolsModelMenuList.Add(toolsModelMenu.transform.GetChild(i).gameObject);
            ToolsModelMenuList[i].SetActive(false);
        }
        VacuumSelected = false;
        VisibleText.SetActive(false);
        HiddenText.SetActive(true);
        ToolsModelMenuList[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        toolsModelMenu.transform.Rotate(_rotation * _speed * Time.deltaTime);
        //float y = Mathf.PingPong(Time.time * _speedUD, 1) * 6 - 3; //Code for hovering
        //toolsModelMenu.transform.position = new Vector3(0, y, 0);

        /*
        if(_visible)
        {
            ToolModelMenu.transform.Rotate(_rotation * _speed * Time.deltaTime);
            //tool1.transform.Rotate(_rotation * _speed * Time.deltaTime);

            float y = Mathf.PingPong(Time.time * _speedUD, 1) * 6 - 3; //Code for hovering
            ToolModelMenu.transform.position = new Vector3(0, y, 0);
        }*/
    }

    public void NextButton()
    {
        index++;
        if (index > ToolsList.Count-1)
        {
            index = 0;
        }
        if (_visible)
        {
            SelectTool();
        }
        ShowToolModel();
    }

    public void PreviousButton()
    {
        index--;
        if (index < 0)
        {
            index = ToolsList.Count-1;
        }
        if (_visible)
        {
            SelectTool();
        }
        ShowToolModel();
    }

    private void SelectTool()
    {
        foreach (GameObject Tool in ToolsList)
        {
            Tool.SetActive(false);
        }
        ToolsList[index].SetActive(true);

        if (_visible && ToolsList[index].name == "Vacuum")
        {
            VacuumSelected = true;
            robotMenu.VacuumNotActive.SetActive(false);
        }
        else
        {
            VacuumSelected = false;
            robotMenu.VacuumButton();
        }
        
    }

    private void ShowToolModel()
    {
        foreach (GameObject ToolsModelMenu in ToolsModelMenuList)
        {
            ToolsModelMenu.SetActive(false);
        }
        ToolsModelMenuList[index].SetActive(true);
    }

    public void VisibleButton()
    {
        _visible = !_visible;
        if(_visible)
        {
            ToolsList[index].SetActive(true);
            if (ToolsList[index].name == "Vacuum")
            {
                VacuumSelected = true;
                robotMenu.VacuumNotActive.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject Tool in ToolsList)
            {
                Tool.SetActive(false);
            }
            VacuumSelected = false;
            robotMenu.VacuumButton();
        }
        VisibleText.SetActive(_visible);
        HiddenText.SetActive(!_visible);
    }
}
