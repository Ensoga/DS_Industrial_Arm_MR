using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HiS.XR.Utils;

namespace HiS.XR
{
    [RequireComponent(typeof(Slider))]
    [RequireComponent(typeof(CloudHandlerHelper))]
    public class TestSliderCloudBehaviour : MonoBehaviour
    {
        #region Variables
        private Slider slider;
        private CloudHandlerHelper cloudHandlerHelper;

        private bool _sliderInitialized = false;
        #endregion

        #region Main Methods
        void Start()
        {
            cloudHandlerHelper = GetComponent<CloudHandlerHelper>();
            slider = GetComponent<Slider>();

            InitializeSlider();
        }

        public void InitializeSlider()
        {
            cloudHandlerHelper.GetThingPropertyValue(cloudHandlerHelper.m_GetThingProperties[0], out double value);
            if (value > slider.maxValue)
            {
                Debug.LogError("Upper bound exceeded: Tried setting value " + value.ToString() + " but upper limit is " + slider.maxValue);
                Debug.LogWarning("Slider set to " + slider.maxValue);
                slider.value = slider.maxValue;
            }
            else if (value < slider.minValue)
            {
                Debug.LogError("Lower bound exceeded: Tried setting value " + value.ToString() + " but lower limit is " + slider.minValue);
                Debug.LogWarning("Slider set to " + slider.minValue);
                slider.value = slider.minValue;
            }
            else
            {
                Debug.Log("Slider initialized to " + value.ToString());
                slider.value = (float)value;
            }
            _sliderInitialized = true;
        }

        public void SetProperty()
        {
            if (_sliderInitialized)
                cloudHandlerHelper.SetPropertyValue(cloudHandlerHelper.m_GetThingProperties[0], Math.Round(slider.value, 4));
        }
        #endregion
    }
}
