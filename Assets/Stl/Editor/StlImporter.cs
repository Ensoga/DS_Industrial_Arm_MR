using System.Collections;
using System.Collections.Generic;
using System.IO;



using UnityEngine;

namespace Stl
{
    [UnityEditor.AssetImporters.ScriptedImporter(2, "stl", 50)]
    public class StlImporter : UnityEditor.AssetImporters.ScriptedImporter
    {
        public float ScaleFactor = 1;
        public bool MergeVertices = false;
        public float MergeAngle = 80;
        public float MergeTolerance = 1e-6f;

        public override void OnImportAsset(UnityEditor.AssetImporters.AssetImportContext ctx)
        {
            if (ScaleFactor <= 0)
                ScaleFactor = 1;

            string fileName = Path.GetFileNameWithoutExtension(ctx.assetPath);

            Material defaultMaterial = new Material(Shader.Find("Diffuse"));
            defaultMaterial.name = fileName + " material";

            ctx.AddObjectToAsset(defaultMaterial.name, defaultMaterial);
            
            GameObject parent = null;
            Mesh[] meshes = StlConverter.Load(ctx.assetPath, ScaleFactor, MergeVertices, MergeTolerance, MergeAngle);
            for (int i = 0; i < meshes.Length; ++i)
            {
                GameObject gameObject = new GameObject();
                gameObject.name = fileName + "_" + i;

                Mesh mesh = meshes[i];
                mesh.name = gameObject.name + "_mesh";

                MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
                meshFilter.sharedMesh = mesh;

                MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
                meshRenderer.sharedMaterial = defaultMaterial;
                
                gameObject.AddComponent<MeshCollider>();

                if (i == 0)
                {
                    parent = gameObject;
                }
                else
                {
                    gameObject.transform.parent = parent.transform;
                }

                ctx.AddObjectToAsset(mesh.name, mesh);

                if (i == 0)
                {
                    ctx.AddObjectToAsset(gameObject.name, gameObject);
                    ctx.SetMainObject(gameObject);
                }
            }
        }
    }
}