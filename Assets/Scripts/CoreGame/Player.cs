using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float descendingSpeed = 20;
    [SerializeField] bool allowForce = true;
    [SerializeField] Vector2 inputDirection = new Vector2();
    [SerializeField] bool splitCanePressed = false;
    [SerializeField] GameObject chainAnchorPrefab;
    [SerializeField] GameObject chainPiecePrefab;
    [SerializeField] float accelerationMaxCooldown;
    private SplitMarker splitMarker;
    public SplitMarker CurrentSplitMarker
    {
        get { return splitMarker; }
        set { splitMarker = value; }
    }
    private SpawnPoint spawnPoint;
    public SpawnPoint CurrentSpawnPoint
    {
        get { return spawnPoint; }
        set { spawnPoint = value; }
    }
    private ChainAnchor chainAnchor;
    private Rigidbody2D rigidBody2D;
    private DistanceJoint2D distanceJoint2D;
    private BoxCollider2D boxCollider2D;
    private AccellerationUI fart;
    private AccellerationUI breath;
    private float accelerationCooldown;
    private Transform chainTransform;
    public Transform ChainCollectionTransform
    {
        get { return chainTransform; }
        set { chainTransform = value; }
    }

    LevelState levelState;
    float startTime;
    public LevelState CurrentLevelState
    {
        get { return levelState; }
    }

    // Start is called before the first frame update
    void Start()
    {
        // GetComponent<DistanceJoint2D>().autoConfigureDistance = false; 
        chainAnchor = GetComponent<ChainAnchor>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        // distanceJoint2D = GetComponent<DistanceJoint2D>();
        // boxCollider2D =  GetComponent<BoxCollider2D>();

        fart = transform.Find("Fart").gameObject.GetComponent<AccellerationUI>();
        breath = transform.Find("Breath").gameObject.GetComponent<AccellerationUI>();
        rigidBody2D.centerOfMass = new Vector2();
        accelerationCooldown = accelerationMaxCooldown;

        startTime = Time.time;
        
        levelState = new LevelState(0, 1, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        levelState.time = Time.time - startTime;
    }


    void FixedUpdate()
    {

        PlayerMovement();

        if(splitCanePressed)
        {
            splitCanePressed = false;
            SplitCane();
        }
    }

    public void PlayerMovement()
    {
        accelerationCooldown -= Time.fixedDeltaTime;
        bool allowAccelleration = accelerationCooldown < 0;
        Vector2 accelerationInputDirection = new Vector2(allowAccelleration?inputDirection.x:0, 0);
        if(accelerationInputDirection.x < -1e-6)
        {
            accelerationCooldown = accelerationMaxCooldown;
            fart.Activate();
        }
        if(accelerationInputDirection.x > 1e-6)
        {
            accelerationCooldown = accelerationMaxCooldown;
            breath.Activate();
        }
        
        Vector2 force = speed*accelerationInputDirection;
        Vector2 rotatedForce = transform.rotation*force;
        rotatedForce.y=0;
        this.allowForce = true;
        if(this.allowForce && force.sqrMagnitude>0)
        {
            rigidBody2D.AddForce(rotatedForce);
        } 

        Vector3 startPosition = chainAnchor.startPiece.StartAnchor.transform.position;
        Vector3 endPosition = transform.position;

        float distance = (endPosition - startPosition).magnitude;
        float newDistance = distance + Time.fixedDeltaTime * -inputDirection.y * descendingSpeed;

        chainAnchor.startPiece.GetComponent<ChainPiece>().SetLength(newDistance);
    }

    public float GetLastCaneLength()
    {
        return distanceJoint2D.distance;
    }

    public void SplitCane()
    {
        Vector3 middlePoint = chainAnchor.startPiece.transform.position;
        GameObject newChainAnchorObj = Instantiate(chainAnchorPrefab, middlePoint, transform.rotation, chainTransform);
        ChainAnchor newChainAnchor = newChainAnchorObj.GetComponent<ChainAnchor>();
        GameObject newChainPieceObj = Instantiate(chainPiecePrefab, middlePoint, transform.rotation, chainTransform);
        ChainPiece newChainPiece = newChainPieceObj.GetComponent<ChainPiece>();

        ChainAnchor oldChainAnchor = chainAnchor.startPiece.StartAnchor;
        ChainPiece oldChainPiece = chainAnchor.startPiece;

        oldChainPiece.ConnectTo(oldChainAnchor, newChainAnchor);
        newChainPiece.ConnectTo(newChainAnchor, chainAnchor);

        splitMarker.TargetChainPiece = newChainPiece;
        oldChainPiece.Deactivate();
        levelState.numCanes += 1;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.started || context.performed)
        {
            Vector2 newInputDirection = context.ReadValue<Vector2>();
            inputDirection = newInputDirection;
        }
        else
        {
            inputDirection = Vector2.zero;
        }
    }

    public void OnSplitCane(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            splitCanePressed = true;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.gameObject.GetComponent<Enemy>())
        {
            levelState.numDeaths += 1;
            spawnPoint.Respawn();
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        levelState.numCollisionsWall += 1;
    }
}
