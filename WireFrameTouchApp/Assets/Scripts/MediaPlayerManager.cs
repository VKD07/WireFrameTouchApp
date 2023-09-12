using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MediaPlayerManager : MonoBehaviour
{
    [SerializeField] GameObject backBtn;
    [SerializeField] GameObject mediaPlayer;

    private void Start()
    {
        backBtn.GetComponent<Button>().onClick.AddListener(() => CloseVideo());
    }
    public void EnableBackButton(bool enable)
    {
        backBtn.SetActive(enable);
    }

    void CloseVideo()
    {
        EnableBackButton(false);
        mediaPlayer.GetComponent<DisplayUGUI>().enabled = false;
    }
}
