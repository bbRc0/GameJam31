using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public Transform targetPosition;
    public LineRenderer lr;
    public DistanceJoint2D dj;

    void Start()
    {
        dj.enabled = false;
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            HookOn();
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            HookOff();
        }
    }
    public void HookOn()
    {
        dj.enabled = true;
        dj.connectedAnchor = targetPosition.position;
        lr.positionCount = 2;
        lr.SetPosition(0, targetPosition.position);
        lr.SetPosition(1, transform.position);       
    }

    public void HookOff()
    {
        dj.enabled = false;
        lr.positionCount = 0;
    }

}
