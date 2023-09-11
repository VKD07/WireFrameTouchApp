using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTopic : MonoBehaviour
{
    [SerializeField] Vector3 originalPosition = Vector3.zero;
    [SerializeField] Vector3 positionOffset = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = transform.position + positionOffset;
    }

    private void FixedUpdate()
    {
    }

}