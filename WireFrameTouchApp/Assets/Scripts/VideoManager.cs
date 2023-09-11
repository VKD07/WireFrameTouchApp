using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    OuterShell outerVertexButtons;
    [NonReorderable]
    [SerializeField] VertexVideo[] vertexVideo; 
    void Start()
    {
        outerVertexButtons= GetComponent<OuterShell>();

        for (int i = 0; i < vertexVideo.Length; i++)
        {
            TextMeshProUGUI buttonText = outerVertexButtons.vertexButtons[i].transform.Find("ButtonText").GetComponent<TextMeshProUGUI>();
            buttonText.SetText(vertexVideo[i].name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class VertexVideo
{
    public string name;
    public VideoClip video;
}
