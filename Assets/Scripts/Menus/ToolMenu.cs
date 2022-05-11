using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolMenu : MonoBehaviour
{
    [SerializeField] GameObject VisibleText;
    [SerializeField] GameObject HiddenText;

    GameObject tools;
    List<GameObject> ToolsList = new List<GameObject>();
    private int _index = 0;
    private bool _visible = false;

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
        }
        VisibleText.SetActive(false);
        HiddenText.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }

    public void PreviousButton()
    {
        _index--;
        if (_index < -1)
        {
            _index = ToolsList.Count-1;
        }
        if (_visible)
        {
            SelectTool();
        }
    }

    private void SelectTool()
    {
        foreach (GameObject Tool in ToolsList)
        {
            Tool.SetActive(false);
        }
        ToolsList[_index].SetActive(true);
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
