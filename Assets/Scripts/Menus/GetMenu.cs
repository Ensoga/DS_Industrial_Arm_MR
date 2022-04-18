using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMenu : MonoBehaviour
{
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
     * No Menu selected = 0
     * Robot Menu = 1
     * Tool Menu = 2
    */

    void RobotMenuAct(int currentMenu)
    {
        if(currentMenu == 1)
        {
            currentMenu = 0;
        }
        else
        {
            currentMenu = 1;
        }
    }

    void ToolMenuAct(int currentMenu)
    {
        if (currentMenu == 2)
        {
            currentMenu = 0;
        }
        else
        {
            currentMenu = 2;
        }
    }

    void AnyMenuDeact(int currentMenu)
    {
        currentMenu = 0;
    }

    #endregion

}
