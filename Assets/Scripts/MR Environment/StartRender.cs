using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Varjo.XR;

public class StartRender : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Start rendering the video see-through image
        VarjoMixedReality.StartRender();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        // Enable Depth Estimation.
        VarjoMixedReality.EnableDepthEstimation();
    }

    private void OnDisable()
    {
        // Disable Depth Estimation.
        VarjoMixedReality.DisableDepthEstimation();
    }
}
