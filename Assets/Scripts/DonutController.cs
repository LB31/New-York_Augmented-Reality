using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonutController : MonoBehaviour
{
    public Renderer rd;

    Color[] colors = new Color[]
    {
        Color.red,
        Color.yellow,
        Color.blue,
        Color.black,
    };

    private void OnMouseDown()
    {
        int random = Random.Range(0, 4);
        rd.material.color = colors[random];
    }
}
