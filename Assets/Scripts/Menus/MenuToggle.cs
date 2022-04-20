using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuToggle : MonoBehaviour
{
    [HideInInspector] public bool currentMenu = false;

    public void ButtonPressed()
    {
        currentMenu = !currentMenu;
    }
}
