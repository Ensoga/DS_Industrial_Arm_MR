using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolMenu : MonoBehaviour
{
    #region Variables
    public bool CurrentMenu;

    #endregion

    #region MainMethods
    public void ButtonPressed()
    {
        if(CurrentMenu)
        {
            CurrentMenu = false;
        }
        else
        {
            CurrentMenu = true;
        }
        
    }

    public void ButtonUnpressed()
    {
        CurrentMenu = false;
    }
    #endregion
}