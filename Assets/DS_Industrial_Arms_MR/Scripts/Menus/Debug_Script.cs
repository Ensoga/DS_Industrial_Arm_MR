using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_Script : MonoBehaviour
{
    public GameObject Robot;
    public float Inc=0.001F; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncS()
    {
        Inc = 0.001F;
    }
    public void IncM()
    {
        Inc = 0.01F;
    }
    public void IncL()
    {
        Inc = 0.1F;
    }

    public void XMas()
    {
        Robot.gameObject.transform.position = new Vector3(Robot.gameObject.transform.position.x + Inc, Robot.gameObject.transform.position.y, Robot.gameObject.transform.position.z);
    }

    public void XMenos()
    {
        Robot.gameObject.transform.position = new Vector3(Robot.gameObject.transform.position.x - Inc, Robot.gameObject.transform.position.y, Robot.gameObject.transform.position.z);
    }

    public void YMas()
    {
        Robot.gameObject.transform.position = new Vector3(Robot.gameObject.transform.position.x, Robot.gameObject.transform.position.y + Inc, Robot.gameObject.transform.position.z);
    }

    public void YMenos()
    {
        Robot.gameObject.transform.position = new Vector3(Robot.gameObject.transform.position.x, Robot.gameObject.transform.position.y - Inc, Robot.gameObject.transform.position.z);
    }

    public void ZMas()
    {
        Robot.gameObject.transform.position = new Vector3(Robot.gameObject.transform.position.x, Robot.gameObject.transform.position.y, Robot.gameObject.transform.position.z + Inc);
    }

    public void ZMenos()
    {
        Robot.gameObject.transform.position = new Vector3(Robot.gameObject.transform.position.x, Robot.gameObject.transform.position.y, Robot.gameObject.transform.position.z - Inc);
    }


}
