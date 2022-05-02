using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Robot_invisible : MonoBehaviour
{

    public Material Material1;
    //in the editor this is what you would set as the object you want to change
    GameObject Partes_Invisibilizar;

    public bool Robot_Invisible = false;
    public List<GameObject> invisivilizar = new List<GameObject>();
    public List<Material> materiales = new List<Material>();


    void Start()
    {
        Partes_Invisibilizar = GameObject.FindWithTag("Invisibilizar");
        invisivilizar.Add(Partes_Invisibilizar);
    }

    public void Invisibilizar_Robot()
    {
        if (Robot_Invisible == false)
        {
            foreach (GameObject visual in invisivilizar)
            {
                visual.GetComponent<MeshRenderer>().material = Material1;
            }
            Robot_Invisible = true;
        }
        else
        {
            Robot_Invisible = false;
        }
       
    }

}