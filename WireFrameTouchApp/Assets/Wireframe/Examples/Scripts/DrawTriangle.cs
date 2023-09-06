using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTriangle : MonoBehaviour
{
    Mesh m;
    MeshFilter mf;
    public Vector3[] VerteicesArray;
    // Use this for initialization
    void Start()
    {
        mf = GetComponent<MeshFilter>();
        m = new Mesh();
        mf.mesh = m;
        drawTriangle();
    }

    //This draws a triangle
    void drawTriangle()
    {
        //We need two arrays one to hold the vertices and one to hold the triangles
        VerteicesArray = new Vector3[3];
        int[] trianglesArray = new int[3];

        //lets add 3 vertices in the 3d space
        VerteicesArray[0] = new Vector3(-9.525327f, 0.3199999f, 0.3199999f);
        VerteicesArray[1] = new Vector3(-8.861331f, 0.6186912f, 0.6186912f);
        VerteicesArray[2] = new Vector3(-10.09551f, 0.6186917f, 11.17787f);

        //define the order in which the vertices in the VerteicesArray shoudl be used to draw the triangle
        trianglesArray[0] = 0;
        trianglesArray[1] = 1;
        trianglesArray[2] = 2;

        //add these two triangles to the mesh
        m.vertices = VerteicesArray;
        m.triangles = trianglesArray;
    }
}
