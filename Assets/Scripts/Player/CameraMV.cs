using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMV : MonoBehaviour
{
    public Transform target;    //따라갈 목표
    public Vector3 offset;      //보정값

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;

    }
}
