using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;
using System.IO;
using UnityEngine.UI;
using RenderHeads.Media.AVProVideo;

public class VideoManager : MonoBehaviour
{
    [SerializeField] OuterShell outerVertexButtons;
    [SerializeField] string fileExtension = "mp4";
    [SerializeField] string[] m_videoPaths;
    [SerializeField] List<string> mp4FileNames;
    [SerializeField] GameObject mediaPlayer;
    MediaPlayerManager mediaPlayerManager;

    private void Awake()
    {
        mediaPlayer = GameObject.FindGameObjectWithTag("MediaPlayer");
        mediaPlayerManager = FindObjectOfType<MediaPlayerManager>();
    }

    private void OnEnable()
    {
        GetVideosData();
        for (int i = 0; i < mp4FileNames.Count; i++)
        {
            int randomBtn = Random.Range(i, outerVertexButtons.vertexButtons.Count);
            GameObject vertexButton = outerVertexButtons.vertexButtons[randomBtn].transform.Find("Canvas").transform.Find("ButtonText").gameObject;
            TextMeshProUGUI buttonText = vertexButton.GetComponent<TextMeshProUGUI>();
            buttonText.SetText(mp4FileNames[i]);

            Button chosenBtn = outerVertexButtons.vertexButtons[randomBtn].GetComponent<Button>();

            int videoIndex = i;
            chosenBtn.onClick.AddListener(() => SetVideo(videoIndex));
        }
    }

    void GetVideosData()
    {
        string streamingAssetsPath = Application.streamingAssetsPath;
        string[] files = Directory.GetFiles(streamingAssetsPath, $"*.{fileExtension}");

        mp4FileNames = new List<string>();
        m_videoPaths = new string[files.Length];

        for (int i = 0; i < files.Length; i++)
        {
            string filePath = files[i];
            string mp4FileName = Path.GetFileNameWithoutExtension(filePath);
            mp4FileNames.Add(mp4FileName);
            m_videoPaths[i] = filePath;
        }
    }

    void SetVideo(int videoIndex)
    {
        MediaPlayer media = mediaPlayer.GetComponent<MediaPlayer>();
        mediaPlayerManager.EnableBackButton(true);
        mediaPlayer.GetComponent<DisplayUGUI>().enabled = true;
        media.OpenVideoFromFile(media.m_VideoLocation, m_videoPaths[videoIndex], true);
    }
}
