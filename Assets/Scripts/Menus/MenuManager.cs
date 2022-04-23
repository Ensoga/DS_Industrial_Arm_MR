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
        foreach (GameObject MenuTogglesGameObject in MenuTogglesGameObjects)
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
}
