using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Model;

public class PieceBehaviour : MonoBehaviour
{
    public GamePiece Piece;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseUp()
    {
        Debug.Log(string.Format("{0} selected", this.renderer.material.color.ToString()));

        Messenger<PieceBehaviour>.Broadcast("Piece selected", this);

    }    
}
