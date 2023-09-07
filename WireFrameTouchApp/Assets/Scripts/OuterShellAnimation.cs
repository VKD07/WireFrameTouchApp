using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterShellAnimation : MonoBehaviour
{
    Vector3 targetScale;
    float step;
    float scale;

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
    }

    private void OnDisable()
    {
        scale = 0;
        transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        IncreaseScale();
    }

    private void IncreaseScale()
    {
        if (this.gameObject.activeSelf && scale < 1)
        {
            scale += Time.deltaTime * 2;
        }
        else
        {
            scale = 1;
        }
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
