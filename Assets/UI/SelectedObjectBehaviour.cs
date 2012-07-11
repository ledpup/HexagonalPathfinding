using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class SelectedObjectBehaviour : MonoBehaviour
{
    float _totalTime;

    void Start()
    {
        _totalTime = 0;
        Scale();
    }

    // Update is called once per frame
    void Update()
    {
        Scale();
        
    }

    private void Scale()
    {
        _totalTime += Time.deltaTime;
        var scale = (float)(1.5 + Math.Sin(_totalTime * 10) * .5);
        this.gameObject.transform.localScale = new Vector3(scale, scale, scale);
    }

}
