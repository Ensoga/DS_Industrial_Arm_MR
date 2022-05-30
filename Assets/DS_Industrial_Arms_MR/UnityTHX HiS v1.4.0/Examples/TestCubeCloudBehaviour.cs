using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HiS.XR.Utils;

namespace HiS.XR
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(CloudHandlerHelper))]
    public class TestCubeCloudBehaviour : MonoBehaviour
    {
        #region Variables
        private MeshRenderer meshComponent;
        private CloudHandlerHelper cloudHandlerHelper;

        private string _propertyName;
        private Dictionary<string, bool> THXanswer = new Dictionary<string, bool>();
        #endregion

        #region Main Methods
        void Start()
        {
            cloudHandlerHelper = GetComponent<CloudHandlerHelper>();
            meshComponent = GetComponent<MeshRenderer>();

            cloudHandlerHelper.SetSyncListener();
            _propertyName = cloudHandlerHelper.m_GetThingProperties[0];
        }

        void Update()
        {
            // Method 1: Extract dictionary to local variable and access it
            cloudHandlerHelper.GetThingProperties(out THXanswer);
            meshComponent.enabled = THXanswer[_propertyName];
        }
        #endregion
    }
}
