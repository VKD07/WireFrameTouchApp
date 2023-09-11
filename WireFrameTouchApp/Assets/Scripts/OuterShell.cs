using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OuterShell : MonoBehaviour
{
    //Prefabs neede
    public GameObject vertexPositionsPrefab; // Assign your cube prefab in the Unity inspector.
    public GameObject lineRendererPrefab; // Assign your line renderer prefab in the Unity inspector.
    public GameObject trianglePrefab;

    //Collection of List for the inner and outer vertex, triangles, and line renderers
    private List<GameObject> outerVertexPos = new List<GameObject>();
    private List<GameObject> innerVertexPos = new List<GameObject>();
    public List<Vector3> innerPos;
    public List<Vector3> outerPos;
    private List<LineRenderer> lineRenderers = new List<LineRenderer>();
    //private List<GameObject> triangles = new List<GameObject>();
    public List<Vector3> trianglePoints;
    public List<GameObject> vertexButtons;

    public Transform outerSphereParent; //used to activate and deactivate outer shell
    public float outerSphereRadius;

    MeshFilter meshFilter;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        outerSphereParent.gameObject.SetActive(false);
        ExtractVertexAndAddItToAList(meshFilter);
        CreateTriangles();
    }

    private void Update()
    {
        UpdateLineRendererPosition();
    }

    private void ExtractVertexAndAddItToAList(MeshFilter meshFilter)
    {
        if (meshFilter != null)
        {
            Mesh mesh = meshFilter.mesh;
            Vector3[] vertices = mesh.vertices;
            Vector3[] normals = mesh.normals;
            // Convert local vertex positions to world positions and store unique positions.
            List<Vector3> uniqueWorldVertices = new List<Vector3>();
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 worldVertex = transform.TransformPoint(vertices[i]);
                if (!uniqueWorldVertices.Contains(worldVertex))
                {
                    uniqueWorldVertices.Add(worldVertex);

                    // sphereRadius = Random.Range(1, 2f);
                    // Calculate the scaled position for the cube based on the desired sphere radius.
                    Vector3 scaledPosition = ScalePosition(worldVertex, outerSphereRadius);

                    // Instantiate a cube at the scaled position with the correct orientation.
                    GameObject offsetCube = InstantiateCubeAtPositionWithOrientation(scaledPosition, transform.TransformDirection(normals[i]));

                    GameObject origCube = InstantiateCubeAtPositionWithOrientation(worldVertex, transform.TransformDirection(normals[i]));

                    // Instantiate a line renderer between the offset and the original cube position.
                    LineRenderer lineRenderer = InstantiateLineRenderer(scaledPosition, worldVertex);

                    // Store the line renderer in the list.
                    lineRenderers.Add(lineRenderer);

                    // Store the cube in the list.
                    outerVertexPos.Add(offsetCube);
                    innerVertexPos.Add(origCube);

                    innerPos.Add(worldVertex);
                    outerPos.Add(scaledPosition);

                }
                Vector3 trianglePos = ScalePosition(worldVertex, outerSphereRadius);
                trianglePoints.Add(trianglePos);
            }
        }
    }

    private Vector3 ScalePosition(Vector3 position, float radius)
    {
        // Calculate the direction from the center of the sphere.
        //radius = Random.Range(1.5f, 2);
        Vector3 direction = position - transform.position;

        // Normalize the direction vector.
        direction.Normalize();

        // Scale the direction vector by the desired radius.
        return transform.position + direction * radius;
    }

    private GameObject InstantiateCubeAtPositionWithOrientation(Vector3 position, Vector3 upDirection)
    {
        // Instantiate your cube prefab at the specified position.
        GameObject vertex = Instantiate(vertexPositionsPrefab, position, Quaternion.identity);
        vertex.transform.SetParent(outerSphereParent);
        //vertex.SetActive(false);

        // Calculate the rotation to align the cube's up direction with the provided upDirection.
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, upDirection);

        // Apply the rotation to the cube.
        vertex.transform.rotation = rotation;
        vertexButtons.Add(vertex);
        return vertex;
    }

    private LineRenderer InstantiateLineRenderer(Vector3 start, Vector3 end)
    {
        // Instantiate your line renderer prefab.
        GameObject lineRendererObject = Instantiate(lineRendererPrefab);
        lineRendererObject.transform.SetParent(outerSphereParent);

        // Get the LineRenderer component from the instantiated object.
        LineRenderer lineRenderer = lineRendererObject.GetComponent<LineRenderer>();

        // Set the positions of the line renderer.
        lineRenderer.SetPositions(new Vector3[] { start, end });

        return lineRenderer;
    }

    private void CreateTriangles()
    {
        // Loop through the outerPoints list to create triangles.
        for (int i = 0; i < trianglePoints.Count; i += 3)
        {
            if (i + 2 < trianglePoints.Count)
            {
                // Create a new GameObject for the triangle.

                int random = Random.Range(0, 2);
                if (random == 0)
                {
                    GameObject triangleObject = Instantiate(trianglePrefab);
                    triangleObject.transform.SetParent(outerSphereParent);

                    //int random = Random.Range(0, 2);
                    //if (random == 0)
                    //{
                    //    triangleObject.SetActive(false);
                    //}

                    // Get the mesh filter of the triangle object.
                    MeshFilter triangleMeshFilter = triangleObject.GetComponent<MeshFilter>();
                    // Create a new mesh for the triangle.
                    Mesh triangleMesh = new Mesh();
                    triangleMesh.vertices = new Vector3[] { trianglePoints[i], trianglePoints[i + 1], trianglePoints[i + 2] };
                    triangleMesh.triangles = new int[] { 0, 1, 2 };

                    // Assign the mesh to the MeshFilter component.
                    triangleMeshFilter.mesh = triangleMesh;

                    // Add the triangle object to the list.
                    //triangles.Add(triangleObject);

                    CreateTriangleMat(triangleObject.GetComponent<Renderer>());
                }
            }
        }
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

    private void UpdateLineRendererPosition()
    {
        // Update line renderer positions based on cube and vertex positions.
        for (int i = 0; i < lineRenderers.Count; i++)
        {
            lineRenderers[i].SetPositions(new Vector3[] { innerVertexPos[i].transform.position, outerVertexPos[i].transform.position });
        }
    }

    public void EnableOuterSphere(bool enable)
    {
        outerSphereParent.gameObject.SetActive(enable);
    }
}
