using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using UnityEngine;

namespace UrdfImporter
{
    public static class UrdfUtils
    {
        public static T DeserializeXml<T>(string path) where T : class
        {
            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    object result = serializer.Deserialize(fileStream);
                    return result as T;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            return null;
        }

        public static Vector3 PropertyToVector3(string s)
        {
            s = s.Trim();
            s = Regex.Replace(s, "\\s+", " ");

            string[] values = s.Split(' ');

            Vector3 result = Vector3.zero;
            if (values.Length == 3)
            {
                try
                {
                    result.x = float.Parse(values[0], CultureInfo.InvariantCulture);
                    result.y = float.Parse(values[1], CultureInfo.InvariantCulture);
                    result.z = float.Parse(values[2], CultureInfo.InvariantCulture);
                }
                catch (Exception e)
                {
                    Debug.Log(s);
                    Debug.LogError(e.Message);
                }
            }

            return result;
        }

        public static Color PropertyToColor(string s)
        {
            s = s.Trim();
            s = Regex.Replace(s, "\\s+", " ");

            string[] values = s.Split(' ');
            Color c = new Color();
            if (values.Length == 4)
            {
                try
                {
                    c.r = float.Parse(values[0]);
                    c.g = float.Parse(values[1]);
                    c.b = float.Parse(values[2]);
                    c.a = float.Parse(values[3]);
                }
                catch (Exception e)
                {
                    Debug.Log(s);
                    Debug.LogError(e.Message);
                }
            }

            return c;
        }

        public static Vector3 UnityPosition(this Urdf.Schema.pose pose)
        {
            Vector3 v = PropertyToVector3(pose.xyz);
            return v.ToLH();
        }

        public static Quaternion UnityRotation(this Urdf.Schema.pose pose)
        {
            Vector3 v = PropertyToVector3(pose.rpy) * Mathf.Rad2Deg;
            return (Quaternion.Euler(0, 0, v.z) * Quaternion.Euler(v.x, v.y, 0)).ToLH();
        }

        public static Vector3 UnityAxis(this Urdf.Schema.axis axis)
        {
            Vector3 v = PropertyToVector3(axis.xyz);
            return v.ToLHAxis();
        }
    }
}
