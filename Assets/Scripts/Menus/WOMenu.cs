using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WOMenu : MonoBehaviour
{
    [SerializeField] GameObject VisibleText;
    [SerializeField] GameObject HiddenText;
    [SerializeField] GameObject WOModelMenu;

    GameObject workObjects;
    List<GameObject> WOList = new List<GameObject>();
    List<GameObject> WOModelMenuList = new List<GameObject>();
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
            WOList.Add(workObjects.transform.GetChild(i).gameObject);
            WOList[i].SetActive(false);
            WOModelMenuList.Add(WOModelMenu.transform.GetChild(i).gameObject);
            WOModelMenuList[i].SetActive(false);
        }
        VisibleText.SetActive(false);
        HiddenText.SetActive(true);
        WOModelMenuList[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NextButton()
    {
        _index++;
        if (_index > WOList.Count - 1)
        {
            _index = 0;
        }
        if (_visible)
        {
            SelectWO();
        }
        ShowWOModel();
    }

    public void PreviousButton()
    {
        _index--;
        if (_index < 0)
        {
            _index = WOList.Count - 1;
        }
        if (_visible)
        {
            SelectWO();
        }
        ShowWOModel();
    }

    private void SelectWO()
    {
        foreach (GameObject Tool in WOList)
        {
            Tool.SetActive(false);
        }
        WOList[_index].SetActive(true);
    }

    private void ShowWOModel()
    {
        foreach (GameObject WOModelMenu in WOModelMenuList)
        {
            WOModelMenu.SetActive(false);
        }
        WOModelMenuList[_index].SetActive(true);
    }

    public void VisibleButton()
    {
        _visible = !_visible;
        if (_visible)
        {
            WOList[_index].SetActive(true);
        }
        else
        {
            foreach (GameObject Tool in WOList)
            {
                Tool.SetActive(false);
            }
        }
        VisibleText.SetActive(_visible);
        HiddenText.SetActive(!_visible);
    }
}
