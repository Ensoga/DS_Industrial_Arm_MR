using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMenu : MonoBehaviour
{
    /*
    MenuManager menuManager;
    //[SerializeField] GameObject MenuScripts;

    MenuList menuList;

    private void Awake()
    {
        menuManager = GetComponent<MenuManager>();
        menuList = GetComponent<MenuList>();
    }

    #region On Pressed and Unpressed methods

    public void RobotMenuPressed()
    {
        RobotMenuAct(menuManager.currentMenu);
        menuManager.ManageMenu(menuList.Menus);
    }

    public void ToolMenuPressed()
    {
        ToolMenuAct(menuManager.currentMenu);
        menuManager.ManageMenu(menuList.Menus);
    }

    public void AnyMenuUnPressed()
    {
        AnyMenuDeact(menuManager.currentMenu);
        menuManager.ManageMenu(menuList.Menus);
    }

    #endregion

    #region Get Menu methods

    /* Menu coding:
     * No Menu selected = -1
     * Robot Menu = 0
     * Tool Menu = 1
    

    void RobotMenuAct(int currentMenu)
    {
        if(currentMenu != 0)
        {
            currentMenu = 0;
        }
        else
        {
            currentMenu = -1;
        }
    }

    void ToolMenuAct(int currentMenu)
    {
        if (currentMenu != 1)
        {
            currentMenu = 1;
        }
        else
        {
            currentMenu = -1;
        }
    }

    void AnyMenuDeact(int currentMenu)
    {
        currentMenu = -1;
    }

    #endregion

    */
}
