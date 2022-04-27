using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickThings : MonoBehaviour
{
    public GameObject Robot;
    private bool vacuum = false;

    public void VacuumControl()
    {
        if (vacuum==false)
        {
            this.transform.SetParent(Robot.transform);
            vacuum = true;
        }
        else
        {
            this.transform.SetParent(null);
            vacuum = false;
        }   
    }
}
