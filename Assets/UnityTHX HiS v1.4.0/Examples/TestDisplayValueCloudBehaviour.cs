using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HiS.XR.Utils;

namespace HiS.XR
{
    [RequireComponent(typeof(Text))]
    [RequireComponent(typeof(CloudHandlerHelper))]
    public class TestDisplayValueCloudBehaviour : MonoBehaviour
    {
        #region Variables
        private CloudHandlerHelper cloudHandlerHelper;
        private Text textLabel;

        private string _propertyName;
        #endregion

        #region Main Methods
        void Start()
        {
            cloudHandlerHelper = GetComponent<CloudHandlerHelper>();
            textLabel = GetComponent<Text>();

            cloudHandlerHelper.SetSyncListener();
            _propertyName = cloudHandlerHelper.m_GetThingProperties[0];
        }

        void Update()
        {
            // Method 2: Accessing key directly on class' dict
            if (cloudHandlerHelper.m_ThinworxBaseType == THXbaseTypes.Number)
            {
                cloudHandlerHelper.GetThingPropertyValue(_propertyName, out double result);
                textLabel.text = result.ToString();
            }
            else if (cloudHandlerHelper.m_ThinworxBaseType == THXbaseTypes.String)
            {
                cloudHandlerHelper.GetThingPropertyValue(_propertyName, out string result);
                textLabel.text = result;
            }
            else
                Debug.LogError("Base type not included");
        }
        #endregion

        #region Aux Methods
        #endregion
    }
}
