using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SphereSpawner : MonoBehaviour
{
    [Header("Button Objects")]
    [SerializeField] GameObject m_button;
    [SerializeField] GameObject m_buttonParent;

    [Header("Button Parent Properties")]
    [SerializeField] float btnsDistance;
    [SerializeField] float numOfBtns;
    [SerializeField] FileInfo[] m_btns;
    [SerializeField] public string[] m_videoPaths;
    public bool stopRotation;
    DirectoryInfo directoryInfo;
    FileInfo[] allFiles;
    float degree;
    public float btnAngle;

    [Header("Rotation Settings")]
    [SerializeField] public float rotationSpeed;
    [SerializeField] float reductionRate;
    public float initialRotation;

    void Start()
    {
        //GetVideos();
        InitButtonsPos();
        initialRotation = rotationSpeed;
    }

    private void Update()
    {
        RotateParent();
        ReduceRotationSpeedToNormal();
    }

    private void GetVideos()
    {
        int i = 0;
        directoryInfo = new DirectoryInfo(Application.streamingAssetsPath);
        allFiles = directoryInfo.GetFiles("*.mp4");
        m_videoPaths = new string[allFiles.Length];
        foreach (var path in allFiles)
        {
            m_videoPaths[i] = path.ToString();
            i++;
        }
        numOfBtns = m_videoPaths.Length;
    }

    private void InitButtonsPos()
    {
        btnAngle = 360 / numOfBtns;
        for (int i = 0; i < numOfBtns; i++)
        {
            Vector3 btnPos = new Vector3(GetXCoordinate(btnsDistance, degree), 0f, GetZCoordinate(btnsDistance, degree));
            GameObject buttonObj = Instantiate(m_button, m_buttonParent.transform);
            //buttonObj.transform.name = i.ToString(); //setting the name
            buttonObj.transform.parent = m_buttonParent.transform;
            buttonObj.transform.position = btnPos;
            degree += btnAngle;
       }
    }

    #region button Rotation
    public void RotateParent()
    {
        if (!stopRotation)
        {
            m_buttonParent.transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
        }
    }

    private void ReduceRotationSpeedToNormal()
    {
        if (rotationSpeed > initialRotation)
        {
            rotationSpeed -= reductionRate * Time.deltaTime;
        }
        else if (rotationSpeed <= -initialRotation)
        {
            rotationSpeed += reductionRate * Time.deltaTime;
        }
    }

    public void SwipeParent(float rotationSpeed)
    {
        m_buttonParent.transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
    public void StopParentRotation(bool p_value)
    {
        stopRotation = p_value;
    }
    #endregion

    #region calculating Button Position
    float GetXCoordinate(float radius, float degree)
    {
        float angleInRad = degree * Mathf.PI / 180;
        float x = radius * Mathf.Cos(angleInRad);
        return x;
    }
    float GetZCoordinate(float radius, float degree)
    {
        float angleInRad = degree * Mathf.PI / 180;
        float y = radius * Mathf.Sin(angleInRad);
        return y;
    }
    #endregion
}
