using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HiS.XR.Classes
{
    public class THXclasses : MonoBehaviour { }

    [Serializable]
    public class UnityTHXexamples
    {
        public string name;
        public string description;
        public string thingTemplate;
        public List<string> tags;

        // Keep it alphabetic
        public bool CubeSwitcher_Reciever;
        public double Number_Reciever;
        public string Text_Reciever;
    }

    public class UR10e_ES
    {
        public string name;
        public string description;
        public string thingTemplate;
        public List<string> tags;

        // Keep it alphabetic

        public double Modbus_UR10e_Joint_Angles_Base;
        public double Modbus_UR10e_Joint_Angles_Elbow;
        public double Modbus_UR10e_Joint_Angles_Shoulder;
        public double Modbus_UR10e_Joint_Angles_Wrist1;
        public double Modbus_UR10e_Joint_Angles_Wrist2;
        public double Modbus_UR10e_Joint_Angles_Wrist3;
    }


    public class JSONconstructor
    {
        public UR10e_ES GetUR10e_ES(string answer)
        {
            return JsonUtility.FromJson<UR10e_ES>(answer);
        }
    }
}
