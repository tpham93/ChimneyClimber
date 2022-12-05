using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] Player playerPrefab;
    [SerializeField] ChainAnchor chainAnchorPrefab;
    [SerializeField] ChainPiece chainPiecePrefab;
    [SerializeField] SplitMarker splitMarkerPrefab;
    [SerializeField] Vector3 playerOffset;
    [SerializeField] FollowingCamera2D followingCamera2D;
    [SerializeField] PresentIndicatorManager presentIndicatorManager;
    List<GameObject> chainCollections;
    private Player player;
    private ChainAnchor playerAnchor;
    private Rigidbody2D playerRigidBody2D;
    private SplitMarker splitMarker;
    private int numChainCollections;

    // Start is called before the first frame update
    void Start()
    {
        chainCollections = new List<GameObject>();
        player = GameObject.Instantiate<Player>(playerPrefab, transform.position+playerOffset, Quaternion.identity);
        player.name = "Player";
        playerAnchor = player.GetComponent<ChainAnchor>();
        playerRigidBody2D = player.GetComponent<Rigidbody2D>();
        splitMarker = GameObject.Instantiate<SplitMarker>(splitMarkerPrefab, transform.position+playerOffset*0.5f, Quaternion.identity);
        splitMarker.name = "SplitMarker";
        followingCamera2D.targetTransform = player.transform;
        presentIndicatorManager.CenteredPlayer = player;
        player.CurrentSpawnPoint = this;
        player.CurrentSplitMarker = splitMarker;
        Respawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, Camera.current.orthographicSize*0.02f);
    }

    public void Respawn()
    {
        Vector3 oldPlayerPosition = player.transform.position;
        playerRigidBody2D.velocity = Vector2.zero;
        player.transform.SetPositionAndRotation(transform.position+playerOffset, Quaternion.identity);
        playerRigidBody2D.rotation = 0;
        playerRigidBody2D.position = player.transform.position;
        splitMarker.transform.position = transform.position+playerOffset*0.5f;

        ChainPiece oldConnectedChainPiece = playerAnchor.startPiece;
        if(oldConnectedChainPiece != null)
        {
            ChainAnchor newEndChainAnchor = GameObject.Instantiate<ChainAnchor>(chainAnchorPrefab, oldPlayerPosition, Quaternion.identity, oldConnectedChainPiece.transform.parent);
            oldConnectedChainPiece.ConnectTo(oldConnectedChainPiece.StartAnchor, newEndChainAnchor);
            oldConnectedChainPiece.Deactivate();
            newEndChainAnchor.Hide();
            followingCamera2D.OnRespawn();
        }

        GameObject chainCollection = createNewChainCollection();

        ChainAnchor baseAnchor = GameObject.Instantiate<ChainAnchor>(chainAnchorPrefab, transform.position, Quaternion.identity, chainCollection.transform);
        baseAnchor.GetComponent<Rigidbody2D>().position = baseAnchor.transform.position;

        ChainPiece chainPiece = GameObject.Instantiate<ChainPiece>(chainPiecePrefab, transform.position+playerOffset*0.5f, Quaternion.identity, chainCollection.transform);
        chainPiece.GetComponent<Rigidbody2D>().position = chainPiece.transform.position;
        
        player.ChainCollectionTransform = chainCollection.transform;
        chainPiece.ConnectTo(baseAnchor, playerAnchor);
        splitMarker.TargetChainPiece = chainPiece;
    }

    GameObject createNewChainCollection()
    {
        GameObject chainCollection = new GameObject("Chain Collection "+(chainCollections.Count+1).ToString());
        chainCollection.transform.position = Vector3.zero;
        chainCollections.Add(chainCollection);
        return chainCollection;
    }
}