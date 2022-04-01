using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;

using UnityEngine;

namespace UrdfImporter
{
    [UnityEditor.AssetImporters.ScriptedImporter(7, "urdf", 100)]
    public class UrdfImporter : UnityEditor.AssetImporters.ScriptedImporter
    {
        public string Package;

        public override void OnImportAsset(UnityEditor.AssetImporters.AssetImportContext ctx)
        {
            if (Package == null || Package == string.Empty)
            {
                string fullPath = EditorUtility.OpenFolderPanel("Select Package folder (must be inside Assets folder)", "Assets", "");
                int assetsPathOffset = Application.dataPath.Length - "Assets".Length;
                if(assetsPathOffset < fullPath.Length)
                    Package = fullPath.Substring(assetsPathOffset);
            }

            Urdf.Schema.robot robot = UrdfUtils.DeserializeXml<Urdf.Schema.robot>(ctx.assetPath);
            if (robot != null && Package != null && Package.StartsWith("Assets"))
            {
                CreateRobot(ctx, Package, robot);
            }
            else
            {
                Debug.LogError(
                    "Could not generate robot from urdf file. Possible problems:\n"
                    + " - Bad formatting: This script supports URDF v1.0 format.\n"
                    + " - Missing xmlns parameter: Add xmlns=\"http://www.ros.org\" in the robot node of the urdf file.\n"
                    + " - URDF Package is not in the Assets folder.");
            }
        }

        private static void CreateRobot(UnityEditor.AssetImporters.AssetImportContext ctx, string package, Urdf.Schema.robot robotDefinition)
        {
            GameObject robot = new GameObject(robotDefinition.name);
            ctx.AddObjectToAsset(robotDefinition.name, robot);
            ctx.SetMainObject(robot);

            UrdfRobot urdfRobot = robot.AddComponent<UrdfRobot>();
            urdfRobot.AllJoints = new List<Urdf.Joint>();
            
            Dictionary<string, GameObject> links = new Dictionary<string, GameObject>();
            List<GameObject> gameObjects = new List<GameObject>();

            foreach (Urdf.Schema.link schemaLink in robotDefinition.link)
            {
                GameObject linkObject = CreateLink(ctx, schemaLink, package);
                links.Add(schemaLink.name, linkObject);
                gameObjects.Add(linkObject);
            }

            foreach (Urdf.Schema.joint schemaJoint in robotDefinition.joint)
            {
                if (!links.ContainsKey(schemaJoint.child.link) || !links.ContainsKey(schemaJoint.parent.link))
                    continue;

                GameObject child = links[schemaJoint.child.link];
                GameObject parent = links[schemaJoint.parent.link];

                Vector3 position = schemaJoint.origin.UnityPosition();
                Quaternion rotation = schemaJoint.origin.UnityRotation();

                child.transform.parent = parent.transform;
                child.transform.localPosition = position;
                child.transform.localRotation = rotation;

                Urdf.JointType jointType = (Urdf.JointType)Enum.Parse(typeof(Urdf.JointType), schemaJoint.type, true);
                if (jointType == Urdf.JointType.Fixed)
                    continue;
                
                HingeJoint hingeJoint = child.AddComponent<HingeJoint>();
                hingeJoint.connectedAnchor = position;
                hingeJoint.autoConfigureConnectedAnchor = true;
                hingeJoint.connectedBody = parent.GetComponent<Rigidbody>();

                Urdf.Joint joint = new Urdf.Joint(schemaJoint.name, jointType);
                joint.Origin = new Pose(position, rotation);
                if (schemaJoint.limit != null)
                {
                    joint.Limit = new Urdf.JointLimit
                    {
                        Lower = (float)schemaJoint.limit.lower,
                        Upper = (float)schemaJoint.limit.upper,
                        Effort = (float)schemaJoint.limit.effort,
                        Velocity = (float)schemaJoint.limit.velocity
                    };
                }

                if (schemaJoint.axis != null)
                {
                    joint.Axis = schemaJoint.axis.UnityAxis();
                    hingeJoint.axis = joint.Axis;
                }

                joint.GameObject = child;
                urdfRobot.AllJoints.Add(joint);
            }

            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.transform.parent == null)
                    gameObject.transform.parent = robot.transform;
            }
            
            urdfRobot.MovableJoints = urdfRobot.AllJoints.Where(j => j.Type != Urdf.JointType.Fixed).ToList();
            urdfRobot.Values = new float[urdfRobot.MovableJoints.Count].ToList();
        }

        private static GameObject CreateLink(UnityEditor.AssetImporters.AssetImportContext ctx, Urdf.Schema.link schemaLink, string package)
        {

            GameObject linkGameObject = new GameObject(schemaLink.name);

            if (schemaLink.inertial != null)
            {
                Urdf.Schema.inertial inertial = schemaLink.inertial;
                Urdf.Schema.inertia inertia = inertial.inertia;

                Rigidbody rigidbody = linkGameObject.AddComponent<Rigidbody>();
                rigidbody.useGravity = true;
                rigidbody.mass = (float)inertial.mass.value;
                rigidbody.centerOfMass = inertial.origin.UnityPosition();
                rigidbody.inertiaTensor = new Vector3((float)inertia.ixx, (float)inertia.izz, (float)inertia.iyy);
            }
            
            if (schemaLink.visual != null)
            {
                GameObject visualGameObject = CreateVisualGameObject(ctx, schemaLink, package);
                visualGameObject.transform.parent = linkGameObject.transform;
            }

            if (schemaLink.collision != null)
            {
                GameObject collisionGameObject = CreateCollisionGameObject(ctx, schemaLink, package);
                collisionGameObject.transform.parent = linkGameObject.transform;
            }

            return linkGameObject;
        }

        private static GameObject CreateVisualGameObject(UnityEditor.AssetImporters.AssetImportContext ctx, Urdf.Schema.link schemaLink, string package)
        {
            GameObject gameObject = CreateGeometryGameObject(ctx, schemaLink, package, schemaLink.visual.geometry, true);
            DestroyAll(gameObject.GetComponentsInChildren<Collider>());
            DestroyAll(gameObject.GetComponentsInChildren<Rigidbody>());
            gameObject.name = "_visual";
            return gameObject;
        }

        private static GameObject CreateCollisionGameObject(UnityEditor.AssetImporters.AssetImportContext ctx, Urdf.Schema.link schemaLink, string package)
        {
            GameObject gameObject = CreateGeometryGameObject(ctx, schemaLink, package, schemaLink.collision.geometry, false);
            if (!gameObject.GetComponentsInChildren<Collider>().Any())
            {
                MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();
                foreach (MeshFilter meshFilter in meshFilters)
                {
                    if (meshFilters.Length == 1)
                    {
                        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
                        meshCollider.sharedMesh = meshFilter.sharedMesh;
                    }
                    else
                    {
                        GameObject meshColliderObject = new GameObject();
                        meshColliderObject.transform.parent = gameObject.transform;
                        MeshCollider meshCollider = meshColliderObject.AddComponent<MeshCollider>();
                        meshCollider.sharedMesh = meshFilter.sharedMesh;
                    }
                }
            }

            DestroyAll(gameObject.GetComponentsInChildren<MeshRenderer>());

            gameObject.name = "_collision";

            return gameObject;
        }

        private static void DestroyAll<T>(T[] objects) where T : UnityEngine.Object
        {
            foreach (T o in objects)
                DestroyImmediate(o);
        }

        private static GameObject CreateGeometryGameObject(
            UnityEditor.AssetImporters.AssetImportContext ctx,
            Urdf.Schema.link schemaLink,
            string package,
            Urdf.Schema.geometry schemaGeometry,
            bool visual)
        {
            GameObject gameObject;

            if (schemaGeometry.Item is Urdf.Schema.mesh)
            {
                Urdf.Schema.mesh schemaMesh = (Urdf.Schema.mesh)schemaGeometry.Item;
                string path = Path.Combine(package, schemaMesh.filename.Replace("package://", ""));
                string fileType = Path.GetExtension(path).ToLower();

                gameObject = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(path));

                if (fileType == ".stl" && visual)
                {
                    Material defaultMaterial = new Material(Shader.Find("Diffuse"));
                    defaultMaterial.name = schemaLink.name + " material";

                    if(schemaLink.visual.material.color != null)
                        defaultMaterial.color = UrdfUtils.PropertyToColor(schemaLink.visual.material.color.rgba);

                    if (schemaLink.visual.material.name != null)
                    {
                        var property = typeof(Color).GetProperties().Where(p => p.Name.ToLower() == schemaLink.visual.material.name.ToLower()).SingleOrDefault();
                        if (property != null)
                            defaultMaterial.color = (Color) property.GetValue(null, null);
                    }

                    ctx.AddObjectToAsset(defaultMaterial.name, defaultMaterial);

                    foreach (var renderer in gameObject.GetComponentsInChildren<MeshRenderer>())
                        renderer.sharedMaterial = defaultMaterial;
                }
            }
            else if (schemaGeometry.Item is Urdf.Schema.box)
            {
                Urdf.Schema.box box = (Urdf.Schema.box)schemaGeometry.Item;
                gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObject.transform.localScale = UrdfUtils.PropertyToVector3(box.size).ToLHWorld();
            }
            else if (schemaGeometry.Item is Urdf.Schema.cylinder)
            {
                Urdf.Schema.cylinder cylinder = (Urdf.Schema.cylinder)schemaGeometry.Item;
                float scaleXZ = (float)cylinder.radius * 2;
                float scaleY = (float)cylinder.length / 2;
                gameObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                gameObject.transform.localScale = new Vector3(scaleXZ, scaleY, scaleXZ);
            }
            else if (schemaGeometry.Item is Urdf.Schema.sphere)
            {
                Urdf.Schema.sphere sphere = (Urdf.Schema.sphere)schemaGeometry.Item;
                float scale = (float)sphere.radius * 2;
                gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                gameObject.transform.localScale = new Vector3(scale, scale, scale);
            }
            else
            {
                throw new FormatException("Unsupported geometry type.");
            }

            Urdf.Schema.pose origin = (visual ? schemaLink.visual.origin : schemaLink.collision.origin) ?? new Urdf.Schema.pose();
            gameObject.transform.localPosition += origin.UnityPosition();
            gameObject.transform.localRotation *= origin.UnityRotation();

            return gameObject;
        }
    }
}