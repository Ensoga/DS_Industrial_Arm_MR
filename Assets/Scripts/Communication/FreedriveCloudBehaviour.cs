using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HiS.XR
{
    [RequireComponent(typeof(CloudHandlerHelper))]

    public class FreedriveCloudBehaviour : MonoBehaviour
    {
        CloudHandlerHelper cloudHandlerHelper;
        GameObject UR10e;
        RobotCloudBehaviour robotCloudBehaviour;

        GameObject RobotMenuToggle;
        RobotMenu robotMenu;

        private List<string> _propertyNames = new List<string>();
        private Dictionary<string, bool> _THXanswer = new Dictionary<string, bool>();

        private void Awake()
        {
            cloudHandlerHelper = GetComponent<CloudHandlerHelper>();
            UR10e = GameObject.FindGameObjectWithTag("UR10e");
            robotCloudBehaviour = UR10e.GetComponent<RobotCloudBehaviour>();

            RobotMenuToggle = GameObject.Find("Robot Menu Toggle");
            robotMenu = RobotMenuToggle.GetComponent<RobotMenu>();

            _propertyNames.Clear(); // Just in case
        }

        // Start is called before the first frame update
        void Start()
        {
            cloudHandlerHelper.SetSyncListener();
            foreach (string property in cloudHandlerHelper.m_GetThingProperties)
            {
                _propertyNames.Add(property);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (robotCloudBehaviour.RobotIsOnline)
            {
                cloudHandlerHelper.GetThingProperties(out _THXanswer);
            }
            robotMenu.FreedriveButtonFeedback(_THXanswer["InternalFreedrive"]);
        }

        public void SetFreedrive(bool _VarjoFreedrive)
        {
            cloudHandlerHelper.SetPropertyValue("VarjoFreedrive", _VarjoFreedrive);
        }

    }

}