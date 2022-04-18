using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public int currentMenu = 0;

    public void ManageMenu(List<GameObject> MenuList)
    {
        if(currentMenu == 0)
        {
            DeactivateAll(MenuList);
        }
        else
        {
            ActivateCurrentMenu(currentMenu, MenuList);
            // It deactivates all menus but the current one, which is activated
        }
        //foreach (GameObject Menu in MenuList)
        {
            //if(Menu.CurrentMenu)
            {
                //Menu.SetActive(true);
            }
            //else
            {
                //Menu.SetActive(false);
            }
        }
    }

    void DeactivateAll(List<GameObject> MenuList)
    {
        foreach (GameObject Menu in MenuList)
        {
            Menu.SetActive(false);
        }
    }

    void ActivateCurrentMenu(int currentMenu, List<GameObject> MenuList)
    {
        for(int i=0; i<MenuList.Count; i++)
        {
            if(i == currentMenu)
            {
                MenuList[i].SetActive(true);
            }
            else
            {
                MenuList[i].SetActive(false);
            }
        }
    }
}
