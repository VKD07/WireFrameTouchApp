using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleAlphaLoop : MonoBehaviour
{
    Renderer render;
    public float alpha;
    public float randomSpeed;
    void Start()
    {
        render = GetComponent<Renderer>();
        randomSpeed = Random.Range(0.1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate alpha using Mathf.Sin to create a smooth wave from 0 to 1.
        alpha = Mathf.Sin(Time.time * randomSpeed) * 0.25f + 0.25f;

        // Create a new color with the calculated alpha.
        Color currentRenderColor = render.material.color;
        Color newColor = new Color(currentRenderColor.r, currentRenderColor.g, currentRenderColor.b, alpha);

        // Set the new color to the material.
        render.material.SetColor("_Color", newColor);
    }
}
