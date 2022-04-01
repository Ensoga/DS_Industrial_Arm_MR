using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTracker : MonoBehaviour {

    public Material Material;
    public bool Visibility = true;
    public int History = 100;
    public float Interval = 0.1f;

    private Queue<Vector3> positions = new Queue<Vector3>();
    private float lastUpdate = 0;
	
	void Update () {
        if (Material == null)
        {
            Material = new Material(Shader.Find("UI/Default"));
            Material.name = "Material";
            Material.color = Color.black;
        }

        if (Time.time - lastUpdate > Interval)
        {
            positions.Enqueue(transform.position);
            lastUpdate = Time.time;
        }

        if (positions.Count > History)
            positions.Dequeue();
    }

    void DrawFrame()
    {
        if (positions.Count < 2)
            return;

        Vector3 prev = Vector3.zero;
        foreach (Vector3 position in positions)
        {
            if(prev != Vector3.zero)
                DrawLine(prev, position, Material);

            prev = position;
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
    
    void OnPostRender()
    {
        if (Visibility)
            DrawFrame();
    }
    
    void OnDrawGizmos()
    {
        if (Visibility)
            DrawFrame();
    }
}
