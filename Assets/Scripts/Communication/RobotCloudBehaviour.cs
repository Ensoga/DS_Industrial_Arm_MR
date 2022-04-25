using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HiS.XR.Utils;

namespace HiS.XR
{
    [RequireComponent(typeof(CloudHandlerHelper))]
    [RequireComponent(typeof(UrdfRobot))]

    public class RobotCloudBehaviour : MonoBehaviour
    {
        #region Variables

        public bool RobotIsOnline = false;

        private List<string> _propertyNames = new List<string>();
        private Dictionary<string, double> _THXanswer = new Dictionary<string, double>();

        CloudHandlerHelper cloudHandlerHelper;
        UrdfRobot urdfRobot;

        #endregion

        private void Awake()
        {
            cloudHandlerHelper = GetComponent<CloudHandlerHelper>();
            urdfRobot = GetComponent<UrdfRobot>();
            _propertyNames.Clear(); // Just in case
        }

        // Start is called before the first frame update
        void Start()
        {
            cloudHandlerHelper.SetSyncListener();
            foreach(string property in cloudHandlerHelper.m_GetThingProperties)
            {
                _propertyNames.Add(property);
            }
            InitializeJoints();
        }

        // Update is called once per frame
        void Update()
        {
            if(RobotIsOnline)
            {
                cloudHandlerHelper.GetThingProperties(out _THXanswer);
                for(int i = 0; i < _propertyNames.Count; i++)
                {
                    urdfRobot.Values[i] = ((float)_THXanswer[_propertyNames[i]])/1000;
                }
            }
        }

        void InitializeJoints()
        {
            int index = 0;
            foreach (string property in _propertyNames)
            {
                cloudHandlerHelper.GetThingPropertyValue(property, out double value);
                if(value > ((Mathf.PI)*2000))
                {
                    Debug.LogError("Upper bound exceeded: Tried setting value " + value.ToString() + " but upper limit is 6.28 rad.");
                    Debug.LogWarning(property + " set to 0.");
                    urdfRobot.Values[index] = 0;
                }
                else if(value < ((Mathf.PI) * -2000))
                {
                    Debug.LogError("Lower bound exceeded: Tried setting value " + value.ToString() + " but lower limit is -6.28 rad.");
                    Debug.LogWarning(property + " set to 0.");
                    urdfRobot.Values[index] = 0;
                }
                else
                {
                    Debug.Log(property + " initialized to " + (value / 1000).ToString() + " rad.");
                    urdfRobot.Values[index] = ((float)value) / 1000;
                }
                index++;
            }
        }
    }
}


