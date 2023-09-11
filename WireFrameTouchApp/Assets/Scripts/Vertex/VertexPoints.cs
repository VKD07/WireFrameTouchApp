using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public struct DebugMeshVertex
{
    public float vertexSize;
    public Color vertexColor;
}

public enum SpawnSetting
{
    BOTH,
    SURFACE,
    OUTER
}

public class VertexPoints : MonoBehaviour
{
    [SerializeField] MeshFilter m_meshFilter;
    [Header("Visual")]
    [SerializeField] bool m_spawnVertexSpheres;
    [SerializeField] SpawnSetting m_spawnSetting = SpawnSetting.BOTH;
    [SerializeField] GameObject m_spherePrefab;

    [Header("3D Vertices")]
    [SerializeField] Vector3[] m_surfaceVertices;
    [SerializeField] Vector3[] m_outsideVertices;
    public float m_outwardDistanceMultiplier = 1.0f;

    [Header("Debug")]
    [SerializeField] bool m_showInInspector = true;
    [SerializeField] DebugMeshVertex m_surfaceVertex;
    [SerializeField] DebugMeshVertex m_outwardVertex;

    #region Getters
    public SpawnSetting GetSpawnChoice() => m_spawnSetting;
    public Vector3[] GetSurfacePoints()
    {
        return m_surfaceVertices;
    }

    public Vector3[] GetOutwardSurfacePoints()
    {
        return m_outsideVertices;
    }

    public Vector3 GetRandomPositionfromVector(int p_rangeStart = 0, int p_rangeEnd = 1)
    {
        return m_outsideVertices[UnityEngine.Random.Range(p_rangeStart, p_rangeEnd)];
    }

    public Vector3 GetRandomPositionfromVector(int p_index)
    {
        return m_outsideVertices[p_index];
    }

    #endregion

    #region LifeCycle

    private void Start()
    {
        GetSurfaceVertices();
        GetOutwardVertices();
        SpawnSpheres();

    }

    private void FixedUpdate()
    {
        GetSurfaceVertices();
        GetOutwardVertices();
    }

    #endregion

    void SpawnSpheres()
    {
        switch (m_spawnSetting)
        {
            case SpawnSetting.BOTH:
                InstantiateSpheres(m_surfaceVertices, new GameObject("Surface"), true);
                InstantiateSpheres(m_outsideVertices, new GameObject("Outer"));
                break;
            case SpawnSetting.SURFACE:
                InstantiateSpheres(m_surfaceVertices, new GameObject("Surface"), true);
                break;
            case SpawnSetting.OUTER:
                InstantiateSpheres(m_outsideVertices, new GameObject("Outer"));
                break;
            default:
                break;
        }
    }

    void InstantiateSpheres(Vector3[] vertices, GameObject p_parent, bool p_isSurfaceSpheres = false)
    {
        p_parent.transform.localPosition = Vector3.zero;
        p_parent.transform.parent = transform;
        int index = 0;
        foreach (var vertex in vertices)
        {
            var obj = Instantiate(m_spherePrefab, vertex, Quaternion.identity, p_parent.transform);
            obj.name = $"{transform.name} Point {obj.transform.GetSiblingIndex()}";
            LineFollow spawnedSphereLineBehaviour = obj.GetComponent<LineFollow>();
            VertexDetection vDetection = obj.GetComponent<VertexDetection>();
            VertexPointBehaviour vBehaviour = obj.GetComponent<VertexPointBehaviour>();
            vBehaviour.m_vertexPoint = this;
            if (!p_isSurfaceSpheres)
            {
                spawnedSphereLineBehaviour.SetVectorPoint(this);
                spawnedSphereLineBehaviour.SetVectorLineIndex(index);
            }
            else
            {
                obj.tag = null;
                vDetection.enabled = false;
                spawnedSphereLineBehaviour.enabled = false;
                vBehaviour.enabled = false;
            }

            index++;
        }
    }

    [ContextMenu("Get Surface Vertices")]

    public void GetSurfaceVertices()
    {
        // Get the Mesh Filter component
        m_meshFilter = GetComponent<MeshFilter>();

        if (m_meshFilter != null)
        {
            Mesh mesh = m_meshFilter.sharedMesh;
            Vector3[] localVertices = mesh.vertices;
            List<Vector3> worldVertices = new List<Vector3>();
            Transform transform = this.transform;

            for (int i = 0; i < localVertices.Length; i++)
            {
                Vector3 worldVertex = transform.TransformPoint(localVertices[i]);
                if (!worldVertices.Contains(worldVertex))
                {
                    worldVertices.Add(worldVertex);
                }
            }
            m_surfaceVertices = worldVertices.ToArray();
        }
    }

    public void GetOutwardVertices()
    {
        List<Vector3> outwardVertices = new List<Vector3>();
        for (int vertexIndex = 0; vertexIndex < m_surfaceVertices.Length; vertexIndex++)
        {
            var calculatedPoint = (m_surfaceVertices[vertexIndex] - transform.position) * m_outwardDistanceMultiplier;

            outwardVertices.Add(calculatedPoint);
        }
        m_outsideVertices = outwardVertices.ToArray();
    }

    public Vector3 GetSurfaceVertex(int p_positionIndex)
    {
        return m_surfaceVertices[p_positionIndex];
    }

    private Vector3 GetCenterPosition()
    {
        return transform.position;
    }

    public Vector3[] GetVectorOutwardLine(int p_positionIndex)
    {
        return new Vector3[] { m_surfaceVertices[p_positionIndex], m_outsideVertices[p_positionIndex] };
    }

    void InstantiateSpheres(Vector3[] vertices)
    {
        foreach (var vertex in vertices)
        {
            var obj = Instantiate(m_spherePrefab, vertex, Quaternion.identity, transform);
            obj.name = $"{transform.name} Point {obj.transform.GetSiblingIndex()}";
        }
    }

    #region Debug

    private void OnDrawGizmos()
    {
        GetSurfaceVertices();
        GetOutwardVertices();
        ShowPointsInInspector(m_showInInspector);
    }

    void ShowPointsInInspector(bool p_showInInspector)
    {
        if (!p_showInInspector)
            return;
        DrawVertexGizmo(m_surfaceVertices, m_surfaceVertex.vertexColor, m_surfaceVertex.vertexSize);
        DrawVertexGizmo(m_outsideVertices, m_outwardVertex.vertexColor, m_outwardVertex.vertexSize);
    }

    void DrawVertexGizmo(Vector3[] p_pointList, Color p_color, float p_size = 0.3f)
    {
        for (int vertexIndex = 0; vertexIndex < p_pointList.Length; vertexIndex++)
        {
            var meshPoint = p_pointList[vertexIndex];
            Gizmos.DrawSphere(meshPoint, p_size);
            Gizmos.color = p_color;
            Handles.Label(meshPoint, $"Point {vertexIndex}");
        }
    }

    #endregion
}