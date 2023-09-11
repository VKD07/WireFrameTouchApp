using System;
using System.Collections.Generic;
using UnityEngine;

public class FaceGenManager : MonoBehaviour
{
    public Action OnFaceCreated;
    public Action OnFaceDestroyed;
    

    public List<GameObject> m_faceList = new List<GameObject>();

    void Start()
    {
        m_faceList = new List<GameObject>();
    }

    void Update()
    {
        
    }

}