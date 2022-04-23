using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public List<GameObject> MenuTogglesGameObjects;
    List<GameObject> _Menus = new List<GameObject>();
    List<MenuToggle> _menuToggles = new List<MenuToggle>();

    private void Awake()
    {
        foreach(GameObject MenuTogglesGameObject in MenuTogglesGameObjects)
        {
            _menuToggles.Add(MenuTogglesGameObject.GetComponent<MenuToggle>());
            _Menus.Add(MenuTogglesGameObject.transform.GetChild(0).gameObject);
        }
    }

    public void ActivateMenu()
    {
        for(int i = 0; i< _Menus.Count; i++)
        {
            if(_menuToggles[i].currentMenu)
            {
                _Menus[i].SetActive(true);
            }
            else
            {
                _Menus[i].SetActive(false);
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
