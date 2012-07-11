using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Model;

public class TileBehaviour : MonoBehaviour
{
    public Tile Tile;
    
    public void SetMaterial()
    {
        var mat = new Material(Shader.Find(" Glossy"));

        mat.color = Tile.CanPass ? Color.green : mat.color = Color.black;
        
        this.renderer.material = mat;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseUp()
    {
        Debug.Log(string.Format("Tile {0}", Tile.ToString()));
        
        Messenger<TileBehaviour>.Broadcast("Tile selected", this);
    }    
}
