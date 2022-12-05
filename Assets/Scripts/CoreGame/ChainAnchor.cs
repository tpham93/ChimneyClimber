using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainAnchor : MonoBehaviour
{
    [SerializeField]
    public ChainPiece startPiece;
    public ChainPiece StartPiece
    {
        get {return startPiece;}
        set { startPiece = value; }
    }
    [SerializeField]
    public ChainPiece endPiece;
    public ChainPiece EndPiece
    {
        get {return endPiece;}
        set { endPiece = value; }
    }
    private Rigidbody2D rbody2D;

    // // Start is called before the first frame update
    void Start()
    {
        rbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(endPiece != null)
        {
            rbody2D.SetRotation(endPiece.transform.rotation);
        }
    }

    public void ConnectTo(ChainPiece start, ChainPiece end)
    {
        StartPiece = start;
        EndPiece = end;
    }

    public void Hide()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
