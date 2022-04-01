using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteAlways]
public class BezierPath : CurvePath {

    [Header("Visibility settings")]
    public Material Material;
    public bool Visibility = true;
    public int Segments = 20;

    [Header("Path settings")]
    public float Vel = 1;
    public float Acc = 1;
    public float RampStart = 0.1f;
    public float RampEnd = 0.1f;

    public Transform[] Targets;

    void Update()
    {
        if (Targets.Length != transform.childCount)
            Targets = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
            Targets[i] = transform.GetChild(i);

        if (Material == null)
        {
            Material = new Material(Shader.Find("UI/Default"));
            Material.name = "Material";
            Material.color = Color.black;
        }
    }

    public override float TVelocity => Vel;
    public override float TAcceleration => Acc;
    public override float TRampStart => RampStart;
    public override float TRampEnd => RampEnd;

    override public Vector3 CalcPosition(float t)
    {
        return CalcBezier(Targets.Select(tr => tr.position).ToArray(), t);
    }

    override public Quaternion CalcRotation(float t)
    {
        return CalcBezier(Targets.Select(tr => tr.rotation).ToArray(), t);
    }

    void DrawFrame() {
        if (Targets.Length == 0)
            return;

        foreach (var target in Targets)
            if (target == null)
                return;

        float resolution = 1.0f / Segments;

        Vector3[] targets = Targets.Select(t=>t.position).ToArray();

        Vector3 prev = targets[0];
        for (int i=1; i<=Segments; ++i)
        {
            float t = i * resolution;
            Vector3 current = CalcBezier(targets, t);
            DrawLine(prev, current, Material);
            prev = current;
        }
    }

    private Vector3 CalcBezier(Vector3[] vectors, float t)
    {
        if (vectors.Length == 0)
            return Vector3.zero;

        if (vectors.Length == 1)
            return vectors[0];

        if (vectors.Length == 2)
            return Vector3.Lerp(vectors[0], vectors[1], t);

        Vector3[] input = new Vector3[vectors.Length - 1];
        for(int i=1; i<vectors.Length; ++i)
            input[i - 1] = Vector3.Lerp(vectors[i-1], vectors[i], t);

        return CalcBezier(input, t);
    }

    private Quaternion CalcBezier(Quaternion[] rotations, float t)
    {
        if (rotations.Length == 0)
            return Quaternion.identity;

        if (rotations.Length == 1)
            return rotations[0];

        if (rotations.Length == 2)
            return Quaternion.Lerp(rotations[0], rotations[1], t);

        Quaternion[] input = new Quaternion[rotations.Length - 1];
        for (int i = 1; i < rotations.Length; ++i)
            input[i - 1] = Quaternion.Lerp(rotations[i - 1], rotations[i], t);

        return CalcBezier(input, t);
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
