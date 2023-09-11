using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class SphereTopicGenerator : MonoBehaviour
{

    [Description("Creates the Topic Buttons on the Outer Vertices")]
    [SerializeField] VertexPoints _vectorPoint;
    [Tooltip("Chooses Selected Vertices where the button will spawn")]
    [SerializeField] int[] m_selectedVertices;// should have choice to randomize or use these instead
    [Description("This is the topic names shown on the buttons.")]
    [SerializeField] string[] m_topics;

    [SerializeField] GameObject m_floatingButtonPrefab;

    void Start()
    {
        //todo: m_topics GET IN CONFIG LATER
        SpawnFloatingButtons();
    }

    public List<int> SelectVectorIndexes()
    {
        List<int> indexList = new List<int>();
        var pointListCount = _vectorPoint.GetOutwardSurfacePoints().Length;
        while (indexList.Count < m_topics.Length)
        {
            int genIndex = Random.Range(0, pointListCount);
            if (!indexList.Contains(genIndex))
            {
                indexList.Add(genIndex);
            }
        }
        return indexList;
    }

    private void SpawnFloatingButtons()
    {
        //GameObject topicParent = new GameObject("Topic Parent");
        var pointList = _vectorPoint.GetOutwardSurfacePoints();
        var selectedTopicIndexes = SelectVectorIndexes();

        for (int vectorIndex = 0; vectorIndex < pointList.Length; vectorIndex++)
        {
            if (selectedTopicIndexes.Contains(vectorIndex))
            {
                var topicName = m_topics[selectedTopicIndexes.IndexOf(vectorIndex)];
                SpawnFloatingButton(pointList, topicName, vectorIndex);
            }
            else
            {
                //var emptyPoint = new GameObject("Empty Point");
                //emptyPoint.transform.position = pointList[vectorIndex];
                //emptyPoint.transform.parent = transform;
                //Debug.Log($"Created Empty {vectorIndex}");
            }
        }
    }

    string GetTopicName(int p_index)
    {
        for (int index = 0; index < m_topics.Length; index++)
        {
            if (p_index == index)
            {
                return m_topics[index];
            }
        }
        Debug.LogError($"Could not find element");
        return null;
    }

    private void SpawnFloatingButton(Vector3[] p_pointList, string p_topicName, int p_selectedIndex)
    {
        var spawnedObj = Instantiate(m_floatingButtonPrefab, transform);
        //int selectedIndex = Random.Range(0, p_pointList.Length);

        spawnedObj.name = p_topicName;
        spawnedObj.GetComponentInChildren<TextMeshProUGUI>().text = p_topicName;
        spawnedObj.transform.position = _vectorPoint.GetRandomPositionfromVector(p_selectedIndex);
    }
}