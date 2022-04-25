using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using HiS.XR.Classes;
using HiS.XR.Utils;
using System.Reflection;
using System.Text;

namespace HiS.XR
{
    public class CloudHandler
    {
        #region Variables
        private bool _debugInfo;
        private DebugLevel _debugLevel;

        private string _thingName;
        private string _appKey;

        private List<string> _getThingProperties;

        private THXbaseTypes _thinworxBaseType;

        private int _getFrequency;

        private Dictionary<string, bool> _thingBooleanValuesDict = new Dictionary<string, bool>();
        private Dictionary<string, double> _thingNumericValuesDict = new Dictionary<string, double>();
        private Dictionary<string, string> _thingTextValuesDict = new Dictionary<string, string>();

        private string _THX_URL = "https://assar.his.se/Thingworx/Things/";
        private const string _delimiterOfJSON = "}},\"rows\":[";
        private int _delimiterLocation = 0;
        #endregion

        #region Constructors
        public CloudHandler(string thingName, string appKey, List<string> getThingProperties, THXbaseTypes thinworxBaseType, int getFrequency,
            bool debugInfo, DebugLevel debugLevel)
        {
            _thingName = thingName;
            _appKey = appKey;

            _getThingProperties = getThingProperties;

            _thinworxBaseType = thinworxBaseType;

            _getFrequency = getFrequency;

            _debugInfo = debugInfo;
            _debugLevel = debugLevel;

            CheckForInputErrors();
            InitializeDictionaries();

            _THX_URL = _THX_URL + _thingName + "/Properties";
        }
        #endregion

        #region Main Methods
        public IEnumerator GetThingProperties()
        {
            if (_debugInfo && _debugLevel >= DebugLevel.Debug)
                Debug.Log("Retrieving from: " + _THX_URL);

            UnityWebRequest thingRequest = UnityWebRequest.Get(_THX_URL);
            thingRequest.SetRequestHeader("Accept", "application/json");
            thingRequest.SetRequestHeader("appKey", _appKey);
            yield return thingRequest.SendWebRequest();

            if (thingRequest.result == UnityWebRequest.Result.ConnectionError || thingRequest.result == UnityWebRequest.Result.ProtocolError)
                Debug.LogError(thingRequest.error);
            else
            {
                string answer = AdjustAnswer(thingRequest.downloadHandler.text);

                // Dynamic creation of class
                Type type = typeof(JSONconstructor);
                MethodInfo methodInfo = type.GetMethod("Get" + _thingName);
                object classInstance = Activator.CreateInstance(type, null);
                object[] parametersArray = new object[] { answer };
                var objectAnswer = methodInfo.Invoke(classInstance, parametersArray);

                if (_debugInfo && _debugLevel >= DebugLevel.Trace)
                    Debug.Log(answer.ToString());

                switch (_thinworxBaseType)
                {
                    case THXbaseTypes.Boolean:
                        foreach (string thingProperty in _getThingProperties)
                        {
                            _thingBooleanValuesDict[thingProperty] = (bool)objectAnswer.GetType().GetField(thingProperty).GetValue(objectAnswer);

                            if (_debugInfo && _debugLevel >= DebugLevel.Info)
                                Debug.Log(thingProperty + " = " + _thingBooleanValuesDict[thingProperty].ToString());
                        }
                        break;
                    case THXbaseTypes.Number:
                        foreach (string thingProperty in _getThingProperties)
                        {
                            _thingNumericValuesDict[thingProperty] = (double)objectAnswer.GetType().GetField(thingProperty).GetValue(objectAnswer);

                            if (_debugInfo && _debugLevel >= DebugLevel.Info)
                                Debug.Log(thingProperty + " = " + _thingNumericValuesDict[thingProperty].ToString());
                        }
                        break;
                    case THXbaseTypes.String:
                        foreach (string thingProperty in _getThingProperties)
                        {
                            _thingTextValuesDict[thingProperty] = (string)objectAnswer.GetType().GetField(thingProperty).GetValue(objectAnswer);

                            if (_debugInfo && _debugLevel >= DebugLevel.Info)
                                Debug.Log(thingProperty + " = " + _thingTextValuesDict[thingProperty].ToString());
                        }
                        break;
                    default:
                        Debug.LogError("Something went wrong assigning Thingworx basetype");
                        break;
                }
            }
        }
        public IEnumerator SetThingProperty(string property, string value)
        {
            if (_debugInfo)
                Debug.Log("Setting variable at: " + _THX_URL);

            byte[] bodyData = RequestHandler(property, value);

            UnityWebRequest thingRequest = UnityWebRequest.Put(_THX_URL + "/" + property, bodyData);
            thingRequest.SetRequestHeader("Content-Type", "application/json");
            thingRequest.SetRequestHeader("appKey", _appKey);
            yield return thingRequest.SendWebRequest();

            if (thingRequest.result == UnityWebRequest.Result.ConnectionError || thingRequest.result == UnityWebRequest.Result.ProtocolError)
                Debug.LogError("UnityTHX error: " + thingRequest.error);
            else
            {
                if (_debugInfo)
                    Debug.Log("Done! Put" + property + " = " + value);
            }
        }
        #endregion

        #region Aux Methods [reciever]
        public Dictionary<string, bool> GetBooleanDictionary()
        {
            return _thingBooleanValuesDict;
        }
        public Dictionary<string, double> GetNumericDictionary()
        {
            return _thingNumericValuesDict;
        }
        public Dictionary<string, string> GetStringDictionary()
        {
            return _thingTextValuesDict;
        }
        public bool GetBooleanDictionaryKey(string propertyName)
        {
            return _thingBooleanValuesDict[propertyName];
        }
        public double GetNumericDictionaryKey(string propertyName)
        {
            return _thingNumericValuesDict[propertyName];
        }
        public string GetStringDictionaryKey(string propertyName)
        {
            return _thingTextValuesDict[propertyName];
        }
        public string GetURL()
        {
            return _THX_URL;
        }
        #endregion

        #region Aux Methods [sender]
        private byte[] RequestHandler(string property, string value)
        {
            string request = "{\"" + property + "\":" + value + "}";
            if (_debugInfo)
            {
                Debug.Log("Sending JSON: " + request);
            }
            byte[] bytes = Encoding.UTF8.GetBytes(request.ToString());
            return bytes;
        }
        #endregion

        #region Aux Methods [reciever]
        private string AdjustAnswer(string answer)
        {
            if (_debugInfo && _debugLevel >= DebugLevel.Trace)
                Debug.Log("Original text: " + answer);

            if (_delimiterLocation == 0)
            {
                if (_debugInfo && _debugLevel >= DebugLevel.Trace)
                    Debug.Log("Locating delimiter...");

                _delimiterLocation = answer.IndexOf(_delimiterOfJSON) + _delimiterOfJSON.Length;

                if (_debugInfo && _debugLevel >= DebugLevel.Trace)
                    Debug.Log("Delimiter in " + _delimiterLocation.ToString());
            }

            string trimmedAnswer = answer.Substring(_delimiterLocation);
            int endLocation = trimmedAnswer.IndexOf("]}");
            trimmedAnswer = trimmedAnswer.Substring(0, trimmedAnswer.Length - (trimmedAnswer.Length - endLocation));

            if (_debugInfo && _debugLevel >= DebugLevel.Trace)
                Debug.Log("Trimmed text: " + trimmedAnswer);

            return trimmedAnswer;
        }
        #endregion

        #region Aux Methods [common]
        private void CheckForInputErrors()
        {
            string @namespace = "HiS.XR.Classes";
            string @class = _thingName;
            string @field;

            // Check whether thing exist
            var myClassType = Type.GetType(string.Format("{0}.{1}", @namespace, @class));
            if (myClassType == null)
                throw new ArgumentException("Error with Thing or its properties! Check THXclasses.cs");

            // Check whether thing property exist
            foreach (string thingProperty in _getThingProperties)
            {
                @field = thingProperty;
                var myFieldType = myClassType.GetField(@field);
                if (myFieldType == null)
                    throw new ArgumentException("Error with Thing or its properties! Check THXclasses.cs");
            }

            if (_appKey == "") // TODO: Inital check on code 401
                Debug.LogError("No valid app key");

            if (float.Parse(Application.unityVersion.Substring(0, 4)) < 2020)
                Debug.LogError("Need to use deprecated methods. Change after 'if' conditional, after yield return on SetThingsProperties() from " +
                    "'thingRequest.isNetworkError || thingRequest.isHttpError' to 'thingRequest.result == UnityWebRequest.Result.ConnectionError || " +
                    "thingRequest.result == UnityWebRequest.Result.ProtocolError'");
        }
        public void InitializeDictionaries()
        {
            // Initialize dictionaries so they do not have to be created later on the Get
            if (_debugInfo && _debugLevel >= DebugLevel.Debug)
                Debug.Log("Initializing dictionaries");

            switch (_thinworxBaseType)
            {
                case THXbaseTypes.Boolean:
                    foreach (string thingProperty in _getThingProperties)
                    {
                        _thingBooleanValuesDict.Add(thingProperty, default);

                        if (_debugInfo && _debugLevel >= DebugLevel.Debug)
                            Debug.Log(thingProperty + " added to dictionary");
                    }
                    break;
                case THXbaseTypes.Number:
                    foreach (string thingProperty in _getThingProperties)
                    {
                        _thingNumericValuesDict.Add(thingProperty, default);

                        if (_debugInfo && _debugLevel >= DebugLevel.Debug)
                            Debug.Log(thingProperty + " added to dictionary");
                    }
                    break;
                case THXbaseTypes.String:
                    foreach (string thingProperty in _getThingProperties)
                    {
                        _thingTextValuesDict.Add(thingProperty, default);

                        if (_debugInfo && _debugLevel >= DebugLevel.Debug)
                            Debug.Log(thingProperty + " added to dictionary");
                    }
                    break;
                default:
                    Debug.LogError("Something went wrong assigning Thingworx basetype");
                    break;
            }
        }
        #endregion
    }
}
