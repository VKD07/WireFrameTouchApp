using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSphere : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 30, 0); // Rotation speed in degrees per second

    void Update()
    {
        // Rotate the object around its local axis
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
