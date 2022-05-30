using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class FrameVisualizer : MonoBehaviour
{
    private static readonly Vector4 Zero = new Vector4(0, 0, 0, 1);
    private static readonly Vector4 X = new Vector4(1, 0, 0, 1);
    private static readonly Vector4 Y = new Vector4(0, 1, 0, 1);
    private static readonly Vector4 Z = new Vector4(0, 0, 1, 1);

    public Material XMaterial;
    public Material YMaterial;
    public Material ZMaterial;
    public bool Visibility = true;
    public float Length = 0.2f;

    private Vector3[] Points = new Vector3[4];

    void Update()
    {
        if(XMaterial == null || YMaterial == null || ZMaterial == null)
        {
            if (XMaterial == null)
            {
                XMaterial = new Material(Shader.Find("UI/Default"));
                XMaterial.name = "XMaterial";
                XMaterial.color = Color.red;
            }
            if (YMaterial == null)
            {
                YMaterial = new Material(Shader.Find("UI/Default")); ;
                YMaterial.name = "YMaterial";
                YMaterial.color = Color.green;
            }
            if (ZMaterial == null)
            {
                ZMaterial = new Material(Shader.Find("UI/Default")); ;
                ZMaterial.name = "ZMaterial";
                ZMaterial.color = Color.blue;
            }
        }

        Vector3 scale = transform.lossyScale;
        scale = new Vector3(Mathf.Sign(scale.x), Mathf.Sign(scale.y), Mathf.Sign(scale.z));
        scale *= Length;

        Matrix4x4 matrix = Matrix4x4.TRS(transform.position, transform.rotation, scale);
        Points[0] = matrix * Zero;
        Points[1] = matrix * X;
        Points[2] = matrix * Y;
        Points[3] = matrix * Z;
    }
    
    void DrawFrame()
    {
        DrawLine(Points[0], Points[1], XMaterial);
        DrawLine(Points[0], Points[2], YMaterial);
        DrawLine(Points[0], Points[3], ZMaterial);
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
            DrawFrame();
    }

    // To show the lines in the editor
    void OnDrawGizmos()
    {
        if (Visibility)
            DrawFrame();
    }
}
