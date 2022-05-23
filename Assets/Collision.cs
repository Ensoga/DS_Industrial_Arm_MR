using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        GetComponent<MeshRenderer>().enabled = true;
    }
    private void OnTriggerExit(Collider other)
    {
        GetComponent<MeshRenderer>().enabled = false;
    }


    void Update()
    {

    }
}
