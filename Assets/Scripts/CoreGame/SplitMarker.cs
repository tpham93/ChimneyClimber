using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitMarker : MonoBehaviour
{
    [SerializeField] ChainPiece chainPiece;
    public ChainPiece TargetChainPiece
    {
        get { return chainPiece; }
        set { chainPiece = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = chainPiece.transform.position;
        this.transform.rotation = chainPiece.transform.rotation;
    }
}