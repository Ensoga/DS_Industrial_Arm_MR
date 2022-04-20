using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    MenuList menulist;
    //public List<MenuToggle> menuToggles;
    MenuToggle[] menuToggles;

    private void Awake()
    {
        menulist = GetComponent<MenuList>();
        foreach(GameObject Menu in menulist.Menus)
        {
            menuToggles = Menu.GetComponents<MenuToggle>();
        }
    }

    void ActivateMenu(List<GameObject> p_menuList)
    {
        for(int i = 0; i< p_menuList.Count; i++)
        {
            if(menuToggles[i].currentMenu)
            {
                p_menuList[i].SetActive(true);
            }
            else
            {
                p_menuList[i].SetActive(false);
            }
        }
    }
    

    /*
    public int currentMenu = -1;

    public void ManageMenu(List<GameObject> MenuList)
    {
        if(currentMenu == -1)
        {
            DeactivateAll(MenuList);
        }
        else
        {
            ActivateCurrentMenu(currentMenu, MenuList);
            // It deactivates all menus but the current one, which is activated                                    
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
    */
}
