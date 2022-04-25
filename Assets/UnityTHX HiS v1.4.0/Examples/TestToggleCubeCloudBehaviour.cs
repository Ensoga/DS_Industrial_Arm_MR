using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HiS.XR.Utils;

namespace HiS.XR
{
    [RequireComponent(typeof(Toggle))]
    [RequireComponent(typeof(CloudHandlerHelper))]
    public class TestToggleCubeCloudBehaviour : MonoBehaviour
    {
        #region Variables
        private Toggle toggle;
        private CloudHandlerHelper cloudHandlerHelper;
        #endregion

        #region Main Methods
        void Start()
        {
            cloudHandlerHelper = GetComponent<CloudHandlerHelper>();
            toggle = GetComponent<Toggle>();

            cloudHandlerHelper.GetThingPropertyValue(cloudHandlerHelper.m_GetThingProperties[0], out bool value);
            Debug.Log("Toggle initialized to " + value.ToString());
            toggle.isOn = value;
        }

        public void SetToggleProperty()
        {
            cloudHandlerHelper.SetPropertyValue(cloudHandlerHelper.m_GetThingProperties[0], toggle.isOn);
        }
        #endregion
    }
}
