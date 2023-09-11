using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class VertexDetection : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] List<Vector3> objPosition;
    [SerializeField] float detectionRadius = 5f;
    [SerializeField] string targetTag = $"Vertex";

    [Header("Visual")]
    [SerializeField] Material triangleMaterial;

    [Header("Debug")]
    public float lineThickness = 1f;

    public Action OnFaceCreated;
    public Action OnFaceDestroyed;
    void Update()
    {
        DetectNearbyPoints();
        CreateFace();
    }

    private void DetectNearbyPoints()
    {
        if (objPosition == null)
            objPosition = new List<Vector3>();
        objPosition.Clear();
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var item in colliders)
        {
            if (item.tag == targetTag)
            {
                objPosition.Add(item.gameObject.transform.position);
            }
        }
    }
    private void CreateFace()
    {
        if (objPosition.Count > 3)
        {
            //Face is ready to be made.
            CreatePlane(objPosition[0], objPosition[1], objPosition[2], objPosition.Count / 3);
            OnFaceCreated?.Invoke();
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere in the Unity Editor to visualize the detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        for (int i = 0; i < objPosition.Count; i++)
        {
            Handles.DrawLine(this.transform.position, objPosition[i], lineThickness);
        }
    }

    public void CreatePlane(Vector3 v1, Vector3 v2, Vector3 v3, int triangleCount)
    {
        Debug.Log($"Trying to create Triangle Mesh");
        // Calculate the normal vector of the plane
        Vector3 normal = Vector3.Cross(v2 - v1, v3 - v1).normalized;

        // Create a new GameObject for the plane
        GameObject planeObject = new GameObject("Plane " + triangleCount);
        MeshFilter meshFilter = planeObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = planeObject.AddComponent<MeshRenderer>();
        Mesh mesh = new Mesh();

        // Define vertices
        Vector3[] planeVertices = new Vector3[] { v1, v2, v3 };
        mesh.vertices = planeVertices;

        // Define triangles
        int[] triangles = new int[] { 0, 1, 2 };
        mesh.triangles = triangles;

        meshFilter.mesh = mesh;
        meshRenderer.material = triangleMaterial;
        // Set the normal vector to ensure proper rendering
        meshFilter.mesh.normals = new Vector3[] { normal, normal, normal };
    }

}