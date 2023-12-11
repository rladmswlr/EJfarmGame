using UnityEngine;

public class Spin : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 10;
    public Vector3 axis = Vector3.right;

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            target.Rotate(axis, Time.deltaTime * rotationSpeed);
        }
    }
}