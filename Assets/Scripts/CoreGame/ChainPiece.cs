using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainPiece : MonoBehaviour
{
    [SerializeField] private ChainAnchor startAnchor;
    public ChainAnchor StartAnchor
    {
        get { return startAnchor; }
        set { startAnchor = value; }
    }
    [SerializeField] private ChainAnchor endAnchor;
    public ChainAnchor EndAnchor
    {
        get {
            connectedToPlayer = endAnchor.GetComponent<Player>() != null;
            return endAnchor;
        }
        set { endAnchor = value; }
    }
    private Rigidbody2D rbody2D;
    private BoxCollider2D boxCollider2D;
    private HingeJoint2D hingeJoint2D;
    private FixedJoint2D fixedJoint2D;

    private bool connectedToPlayer = true;
    // Start is called before the first frame update
    void Start()
    {
        initComponentReferences();
        hingeJoint2D.anchor = new Vector2(
            0,
            0.5f
        );
        fixedJoint2D.anchor = new Vector2(
            0,
            -0.5f
        );
        rbody2D.centerOfMass = new Vector2(0,1);
    }

    void initComponentReferences()
    {
        rbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        hingeJoint2D = GetComponent<HingeJoint2D>();
        fixedJoint2D = GetComponent<FixedJoint2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SetLength(GetNewLength());
    }

    public float GetNewLength()
    {
        Vector3 posDiff = endAnchor.transform.position-startAnchor.transform.position;
        posDiff.z = 0;
        return posDiff.magnitude;
    }

    public void SetLength(float length)
    {
        Vector3 newScale = transform.localScale;
        newScale.y = length;
        transform.localScale = newScale;
    }

    public void ConnectTo(ChainAnchor start, ChainAnchor end)
    {
        if(rbody2D==null){
            initComponentReferences();
        }
        Rigidbody2D startRigidBody2D = start.GetComponent<Rigidbody2D>();
        Rigidbody2D endRigidBody2D = end.GetComponent<Rigidbody2D>();
        startAnchor = start;
        endAnchor = end;
        start.endPiece = this;
        end.startPiece = this;
        hingeJoint2D.connectedBody = startRigidBody2D;
        fixedJoint2D.connectedBody = endRigidBody2D;
        transform.position = (startRigidBody2D.position+endRigidBody2D.position)/2f;
        SetLength(GetNewLength());
    }

    public void Deactivate()
    {
        rbody2D.bodyType = RigidbodyType2D.Static;
        hingeJoint2D.enabled = false;
        fixedJoint2D.enabled = false;
        boxCollider2D.enabled = false;
    }
}
