using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCollision : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    GameObject _workObjects;
    private List<GameObject> _WOList = new List<GameObject>();

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _workObjects = GameObject.FindGameObjectWithTag("WorkObjects");
    }

    void Start()
    {
        _meshRenderer.enabled = false;
        for (int i = 0; i < _workObjects.transform.childCount; i++)
        {
            _WOList.Add(_workObjects.transform.GetChild(i).gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_WOList.Contains(other.GetComponent<Transform>().gameObject))
        {
            _meshRenderer.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_WOList.Contains(other.GetComponent<Transform>().gameObject))
        {
            _meshRenderer.enabled = false;
        }
    }
}
