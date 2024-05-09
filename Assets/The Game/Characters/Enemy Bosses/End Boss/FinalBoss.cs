using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{

    [HideInInspector] public bool canRotate = true;
    public float rotationSpeed = 8f;

    private IBaseNode BTRootNode;

    private void CreateBT()
    {
        BTRootNode = new FacePlayerNode(this);
    }

    void Start()
    {
        CreateBT();
    }

    void Update()
    {
        BTRootNode?.Update();
    }
}
