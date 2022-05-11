using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolMenu : MonoBehaviour
{
    [SerializeField] GameObject VisibleText;
    [SerializeField] GameObject HiddenText;
    [SerializeField] GameObject toolsModelMenu;

    GameObject tools;
    List<GameObject> ToolsList = new List<GameObject>();
    List<GameObject> ToolsModelMenuList = new List<GameObject>();
    private int _index = 0;
    private bool _visible = false;

    /*
    [SerializeField] Vector3 _rotation;
    [SerializeField] float _speed;
    [SerializeField] float _limit;
    [SerializeField] float _speedUD;*/

    private void Awake()
    {
        tools = GameObject.FindGameObjectWithTag("Tools");
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
        VisibleText.SetActive(false);
        HiddenText.SetActive(true);
        ToolsModelMenuList[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
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
        _index++;
        if (_index > ToolsList.Count-1)
        {
            _index = 0;
        }
        if (_visible)
        {
            SelectTool();
        }
        ShowToolModel();
    }

    public void PreviousButton()
    {
        _index--;
        if (_index < 0)
        {
            _index = ToolsList.Count-1;
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
        ToolsList[_index].SetActive(true);
    }

    private void ShowToolModel()
    {
        foreach (GameObject ToolsModelMenu in ToolsModelMenuList)
        {
            ToolsModelMenu.SetActive(false);
        }
        ToolsModelMenuList[_index].SetActive(true);
    }

    public void VisibleButton()
    {
        _visible = !_visible;
        if(_visible)
        {
            ToolsList[_index].SetActive(true);
        }
        else
        {
            foreach (GameObject Tool in ToolsList)
            {
                Tool.SetActive(false);
            }
        }
        VisibleText.SetActive(_visible);
        HiddenText.SetActive(!_visible);
    }
}
