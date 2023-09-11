using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleSurfaceGenerator : MonoBehaviour
{
    [SerializeField] GameObject trianglePrefab;
    [SerializeField] Transform outerSphereParent;
    Vector3[] vertices;
    List<GameObject> triangles = new List<GameObject>();
    VertexPoints vertexPoints;
    void Start()
    {
        vertexPoints = GetComponent<VertexPoints>();
        CreateTriangles();

        StartCoroutine(CreateTrianglesWithDelay());
    }

    IEnumerator CreateTrianglesWithDelay()
    {
        yield return new WaitForSeconds(0.2f);
        // outerSphereParent = GameObject.Find("Outer").transform;
    }

    // Update is called once per frame
    void Update()
    {
        vertices = vertexPoints.GetOutwardSurfacePoints();
        print(vertexPoints.GetOutwardSurfacePoints()[1]);

        Mesh triangleMesh = triangles[0].GetComponent<MeshFilter>().mesh;

        triangleMesh.vertices = new Vector3[] { vertices[1], vertices[1 + 1], vertices[1 + 2] };
        triangleMesh.RecalculateNormals();
        triangleMesh.RecalculateBounds();
    }

    private void CreateTriangles()
    {
        Vector3[] vertices = vertexPoints.GetOutwardSurfacePoints();

        // Ensure we have at least 3 vertices to create a triangle.
        if (vertices.Length < 3)
        {
            Debug.LogWarning("Not enough vertices to create triangles.");
            return;
        }

        // Loop through the vertices to create triangles.


        // Create a new GameObject for the triangle.
        GameObject triangleObject = Instantiate(trianglePrefab);

        // Get the mesh filter of the triangle object.
        MeshFilter triangleMeshFilter = triangleObject.GetComponent<MeshFilter>();

        // Create a new mesh for the triangle using the current vertex and the next two vertices.
        Mesh triangleMesh = new Mesh();
        triangleMesh.vertices = new Vector3[] { vertices[0], vertices[0 + 1], vertices[0 + 2] };
        triangleMesh.triangles = new int[] { 0, 1, 2 };

        // Assign the mesh to the MeshFilter component.
        triangleMeshFilter.mesh = triangleMesh;



        // Create a material for the triangle.
        CreateTriangleMat(triangleObject.GetComponent<Renderer>());
        triangles.Add(triangleObject);


    }

    void CreateTriangleMat(Renderer triangle)
    {
        Material triangleMat = new Material(Shader.Find("Standard"));
        Color baseColor = new Color(0.54f, 1, 0.87f, 0.5f);
        Color emissionColor = new Color(0.27f, 0.70f, 0.45f); // Adjust the emission color as needed.
        float emissionIntensity = 1.5f; // Adjust the emission intensity as needed.

        triangleMat.SetColor("_Color", baseColor);
        triangleMat.SetFloat("_Mode", 3);
        triangleMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        triangleMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        triangleMat.SetInt("_ZWrite", 0); // Disable Z-write for transparency.

        triangleMat.EnableKeyword("_ALPHABLEND_ON");
        triangleMat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent; // Set the render queue for transparency.
        triangleMat.SetFloat("_Glossiness", 0); // Set the smoothness to 0.

        // Set emission color and intensity.
        triangleMat.SetColor("_EmissionColor", emissionColor * emissionIntensity);
        triangleMat.EnableKeyword("_EMISSION"); // Enable emission.

        triangle.material = triangleMat;
    }

}
