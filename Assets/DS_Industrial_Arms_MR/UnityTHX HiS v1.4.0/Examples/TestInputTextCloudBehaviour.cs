using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HiS.XR
{
    [RequireComponent(typeof(InputField))]
    [RequireComponent(typeof(CloudHandlerHelper))]
    public class TestInputTextCloudBehaviour : MonoBehaviour
    {
        #region Variables
        private InputField inputField;
        private CloudHandlerHelper cloudHandlerHelper;
        #endregion

        #region Main Methods
        void Start()
        {
            cloudHandlerHelper = GetComponent<CloudHandlerHelper>();
            inputField = GetComponent<InputField>();

            cloudHandlerHelper.GetThingPropertyValue(cloudHandlerHelper.m_GetThingProperties[0], out string value);
            Debug.Log("Input field initialized to " + value);
            inputField.text = value;
        }

        public void SetTextPropertyToMatch()
        {
            cloudHandlerHelper.SetPropertyValue(cloudHandlerHelper.m_GetThingProperties[0], inputField.text);
        }
        #endregion
    }
}
