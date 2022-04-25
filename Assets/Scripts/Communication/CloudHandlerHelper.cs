using System;
using System.Collections;
using System.Collections.Generic;
using HiS.XR.Utils;
using UnityEngine;

namespace HiS.XR
{
    public class CloudHandlerHelper : MonoBehaviour
    {
        #region Variables
        public bool m_DebugInfo = false;

        [ShowIfAttribute(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(m_DebugInfo))]
        public DebugLevel m_DebugLevel;

        [Header("Thingworx Thing")]
        public string m_ThingName;
        public string m_AppKey;

        [Header("Properties")]
        public List<string> m_GetThingProperties;

        public THXbaseTypes m_ThinworxBaseType;

        [Header("Refresh variables")]
        public int m_GetFrequency = 10;

        private CloudHandler cloudHandler;
        #endregion

        #region Main Methods
        void Awake()
        {
            cloudHandler = new CloudHandler(m_ThingName, m_AppKey, m_GetThingProperties, m_ThinworxBaseType, m_GetFrequency,
                m_DebugInfo, m_DebugLevel);

            // Reads first values on Awake
            StartCoroutine(cloudHandler.GetThingProperties());
        }
        public void SetSyncListener()
        {
            foreach (var prop in m_GetThingProperties)
                Debug.Log("Sync listener to property " + prop + " [" + m_GetFrequency + " times per second]");

            InvokeRepeating(nameof(CallGet), 1f, (float)1 / m_GetFrequency);
        }
        public void SetOneTimeListener()
        {
            foreach (var prop in m_GetThingProperties)
                Debug.Log("One-time listener to property " + prop);

            StartCoroutine(cloudHandler.GetThingProperties());
        }
        private void CallGet()
        {
            StartCoroutine(cloudHandler.GetThingProperties());
        }
        private void SetProperty(string property, string value)
        {
            if (m_DebugInfo)
                Debug.Log("Sending " + value + " to " + property);
            StartCoroutine(cloudHandler.SetThingProperty(property, value));

        }
        #endregion

        #region Aux Methods [reciever]
        // Overload
        public void GetThingProperties(out Dictionary<string, bool> result)
        {
            result = cloudHandler.GetBooleanDictionary();
        }
        public void GetThingProperties(out Dictionary<string, double> result)
        {
            result = cloudHandler.GetNumericDictionary();
        }
        public void GetThingProperties(out Dictionary<string, string> result)
        {
            result = cloudHandler.GetStringDictionary();
        }
        // Overload
        public void GetThingPropertyValue(string propertyName, out bool result)
        {
            result = cloudHandler.GetBooleanDictionaryKey(propertyName);
        }
        public void GetThingPropertyValue(string propertyName, out double result)
        {
            result = cloudHandler.GetNumericDictionaryKey(propertyName);
        }
        public void GetThingPropertyValue(string propertyName, out string result)
        {
            result = cloudHandler.GetStringDictionaryKey(propertyName);
        }
        #endregion

        #region Aux Methods [sender]
        // Overload
        public void SetPropertyValue(string property, bool value)
        {
            SetProperty(property, value.ToString().ToLower());
        }
        public void SetPropertyValue(string property, double value)
        {
            SetProperty(property, value.ToString());
        }
        public void SetPropertyValue(string property, string value)
        {
            string sendValue = "\"" + value + "\"";
            SetProperty(property, sendValue);
        }
        #endregion
    }
}

