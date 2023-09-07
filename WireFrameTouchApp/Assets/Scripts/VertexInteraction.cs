using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VertexInteraction : MonoBehaviour
{
    [SerializeField] float rayCastLength = 10f;
    [SerializeField] LayerMask layerMask;
    RaycastHit hit;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, rayCastLength, layerMask))
        {
            InteractVertex(hit.transform.gameObject);
            //SelectAndDragUp();
        }
    }

    void InteractVertex(GameObject vertex)
    {
        if(Input.GetMouseButtonDown(0))
        {
            vertex.GetComponent<Button>().onClick.Invoke();
        }
    }
}
