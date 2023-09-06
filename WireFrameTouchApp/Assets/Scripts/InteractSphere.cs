using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSphere : MonoBehaviour
{
    public float rotSpeed = 50f;
    float rotX;
    private void OnMouseDrag()
    {
        rotX = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;
        transform.Rotate(Vector3.forward, -rotX);
    }
}
