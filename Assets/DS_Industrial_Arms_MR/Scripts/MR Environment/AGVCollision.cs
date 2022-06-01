using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGVCollision : MonoBehaviour
{
    GameObject _workObjects;
    private List<GameObject> _WOList = new List<GameObject>();

    private void Awake()
    {
        _workObjects = GameObject.FindGameObjectWithTag("WorkObjects");
    }

    void Start()
    {
        for (int i = 0; i < _workObjects.transform.childCount; i++)
        {
            _WOList.Add(_workObjects.transform.GetChild(i).gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_WOList.Contains(other.GetComponent<Transform>().gameObject))
        {
            other.transform.SetParent(_workObjects.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
