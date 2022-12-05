using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaneColliderResizer : MonoBehaviour
{
    private Player player;
    // private Rigidbody2D rigidbody2D;
    private BoxCollider2D boxCollider2D;
    // Start is called before the first frame update
    void Start()
    {
        player = transform.GetComponentInParent<Player>();
        // rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // float newDistance = player.GetLastCaneLength();
        // boxCollider2D.size = new Vector2(boxCollider2D.size.x, newDistance);
        // boxCollider2D.offset = new Vector2(boxCollider2D.offset.x, newDistance/2);
    }
}
