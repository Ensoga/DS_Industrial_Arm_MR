using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(MeshFilter))]
public class MeshNormalsVisualizer : MonoBehaviour
{
    public Material Material;
    public bool Visibility = true;
    public float Length = 0.01f;

    private Vector3[] Points = new Vector3[4];
    private MeshFilter meshFilter;

    void Update()
    {
        if (meshFilter == null)
            meshFilter = GetComponent<MeshFilter>();

        if (Material == null)
        {
            Material = new Material(Shader.Find("UI/Default"));
            Material.name = "XMaterial";
            Material.color = Color.red;
        }
    }
    
    void DrawNormals()
    {
        if (meshFilter == null)
            return;

        var vertices = meshFilter.sharedMesh.vertices;
        var normals = meshFilter.sharedMesh.normals;

        for (int i=0; i<vertices.Length; ++i)
        {
            Vector3 vertex = transform.TransformPoint(vertices[i]);
            Vector3 normal = transform.rotation * normals[i] * Length;
            DrawLine(vertex, vertex + normal, Material);
        }
    }

    private void DrawLine(Vector3 p1, Vector3 p2, Material material)
    {
        GL.Begin(GL.LINES);
        material.SetPass(0);
        GL.Color(new Color(material.color.r, material.color.g, material.color.b, material.color.a));
        GL.Vertex3(p1.x, p1.y, p1.z);
        GL.Vertex3(p2.x, p2.y, p2.z);
        GL.End();
    }

    // To show the lines in the game window when it is running
    void OnPostRender()
    {
        if (Visibility)
            DrawNormals();
    }

    // To show the lines in the editor
    void OnDrawGizmos()
    {
        if (Visibility)
            DrawNormals();
    }
}
