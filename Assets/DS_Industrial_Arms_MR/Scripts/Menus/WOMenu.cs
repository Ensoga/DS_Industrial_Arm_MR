using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WOMenu : MonoBehaviour
{
    [SerializeField] GameObject VisibleText;
    [SerializeField] GameObject HiddenText;
    [SerializeField] GameObject WOModelMenu;

    GameObject workObjects;
    [HideInInspector] public List<GameObject> WOList = new List<GameObject>();
    List<GameObject> WOModelMenuList = new List<GameObject>();
    [HideInInspector] public int index = 0;
    private bool _visible = false;

    GameObject robotMenuToggle;
    RobotMenu robotMenu;
    GameObject gridWallsParent;
    List<GameObject> gridWalls = new List<GameObject>();
    List<MeshRenderer> gridWallsMeshRenderer = new List<MeshRenderer>();

    private void Awake()
    {
        workObjects = GameObject.FindGameObjectWithTag("WorkObjects");
        robotMenuToggle = GameObject.Find("Robot Menu Toggle");
        robotMenu = robotMenuToggle.GetComponent<RobotMenu>();
        gridWallsParent = robotMenu.gridWallsParent;
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
        for (int i = 0; i < gridWallsParent.transform.childCount; i++)
        {
            gridWalls.Add(gridWallsParent.transform.GetChild(i).gameObject);
            gridWallsMeshRenderer.Add(gridWalls[i].GetComponent<MeshRenderer>());
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
        index++;
        if (index > WOList.Count - 1)
        {
            index = 0;
        }
        if (_visible)
        {
            SelectWO();
        }
        ShowWOModel();
    }

    public void PreviousButton()
    {
        index--;
        if (index < 0)
        {
            index = WOList.Count - 1;
        }
        if (_visible)
        {
            SelectWO();
        }
        ShowWOModel();
    }

    private void SelectWO()
    {
        foreach (GameObject WO in WOList)
        {
            WO.SetActive(false);
        }
        ResetGrid();
        WOList[index].SetActive(true);
    }

    private void ShowWOModel()
    {
        foreach (GameObject WOModelMenu in WOModelMenuList)
        {
            WOModelMenu.SetActive(false);
        }
        WOModelMenuList[index].SetActive(true);
    }

    public void VisibleButton()
    {
        _visible = !_visible;
        if (_visible)
        {
            WOList[index].SetActive(true);
        }
        else
        {
            foreach (GameObject WO in WOList)
            {
                WO.SetActive(false);
            }

            if (robotMenu.GridActive)
            {
                ResetGrid();
            }
        }
        VisibleText.SetActive(_visible);
        HiddenText.SetActive(!_visible);
    }

    private void ResetGrid()
    {
        foreach(MeshRenderer meshRenderer in gridWallsMeshRenderer)
        {
            meshRenderer.enabled = false;
        }
    }
}
