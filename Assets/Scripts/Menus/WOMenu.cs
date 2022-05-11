using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WOMenu : MonoBehaviour
{
    [SerializeField] GameObject VisibleText;
    [SerializeField] GameObject HiddenText;

    GameObject workObjects;
    List<GameObject> WorkObjectsList = new List<GameObject>();
    private int _index = 0;
    private bool _visible = false;

    private void Awake()
    {
        workObjects = GameObject.FindGameObjectWithTag("WorkObjects");
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < workObjects.transform.childCount; i++)
        {
            WorkObjectsList.Add(workObjects.transform.GetChild(i).gameObject);
            WorkObjectsList[i].SetActive(false);
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
        if (_index > WorkObjectsList.Count - 1)
        {
            _index = 0;
        }
        if (_visible)
        {
            SelectWorkObject();
        }
    }

    public void PreviousButton()
    {
        _index--;
        if (_index < -1)
        {
            _index = WorkObjectsList.Count - 1;
        }
        if (_visible)
        {
            SelectWorkObject();
        }
    }

    private void SelectWorkObject()
    {
        foreach (GameObject Tool in WorkObjectsList)
        {
            Tool.SetActive(false);
        }
        WorkObjectsList[_index].SetActive(true);
    }

    public void VisibleButton()
    {
        _visible = !_visible;
        if (_visible)
        {
            WorkObjectsList[_index].SetActive(true);
        }
        else
        {
            foreach (GameObject Tool in WorkObjectsList)
            {
                Tool.SetActive(false);
            }
        }
        VisibleText.SetActive(_visible);
        HiddenText.SetActive(!_visible);
    }
}
