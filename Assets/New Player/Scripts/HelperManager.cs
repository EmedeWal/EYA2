using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperManager : MonoBehaviour
{
    public CameraController CameraController;
    public Transform PlayerTransform;

    private void Start()
    {
        SingletonBase[] singletons = FindObjectsByType<SingletonBase>(FindObjectsSortMode.None);
        foreach (SingletonBase singleton in singletons) singleton.SingletonSetup();
        
        CameraController.Init(PlayerTransform);
    }

    private void Update()
    {

    }

    private void LateUpdate()
    {

    }

    private void FixedUpdate()
    {

    }
}
