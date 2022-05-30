using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePathMover : MonoBehaviour {

    public Transform Target;
    public List<CurvePath> Paths = new List<CurvePath>();
    public bool Run = false;
    public float Velocity = 1;
    public int CurrentPath = 0;

    private float timeDir = 1;
    private float time = 0;
	
	void Update () {
        if (Target == null || Paths.Count == 0 || Run == false)
            return;

        if (CurrentPath > Paths.Count)
            CurrentPath = Paths.Count - 1;

        CurvePath path = Paths[CurrentPath];
        if (path == null)
            return;

        time += path.TVelocity * Velocity * timeDir * Time.deltaTime;
        if(time > 1 && CurrentPath < Paths.Count - 1)
        {
            time -= 1;
            path = Paths[++CurrentPath];
        }
        else if(time < 0 && CurrentPath > 0)
        {
            time += 1;
            path = Paths[--CurrentPath];
        }
        else if (time < 0 || time > 1)
        {
            timeDir = -timeDir;
        }

        time = Mathf.Clamp01(time);

        float rampUp = path.TAcceleration * path.TRampStart / 2f;
        float rampDown = path.TAcceleration * path.TRampEnd / 2f;

        float t;
        if(time < path.TRampStart)
        {
            t = time * time * rampUp / path.TRampStart / path.TRampStart;
        }
        else if(time > 1 - path.TRampEnd)
        {
            t = 1 - Mathf.Pow(1 - time, 2) * rampDown / path.TRampEnd / path.TRampEnd;
        }
        else
        {
            t = (time - path.TRampStart) * (1 - rampUp - rampDown) / (1 - path.TRampStart - path.TRampEnd) + rampUp;
        }

        Target.position = path.CalcPosition(t);
        Target.rotation = path.CalcRotation(t);
    }
}
