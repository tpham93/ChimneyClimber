using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Present : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed;
    [SerializeField]
    float angleLimit;
    PresentManager presentManager;
    int presentID;
    public int PresentID{
        get { return presentID; }
        set { presentID = value; }
    }
    bool collected;
    Rigidbody2D rigidBody2D;
    BoxCollider2D boxCollider2D;
    // Start is called before the first frame update
    void Start()
    {
        collected = false;
        boxCollider2D = GetComponent<BoxCollider2D>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        presentManager = transform.parent.GetComponent<PresentManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        float newAngle = Mathf.Sin(Time.time*2*Mathf.PI) * rotationSpeed * angleLimit;
        rigidBody2D.SetRotation(newAngle);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        OnCollect();
    }

    void OnCollect()
    {
        if(!collected)
        {
            presentManager.OnCollectPresent(presentID);
            boxCollider2D.enabled = false;
            Destroy(this.gameObject);
        }
        collected = true;
    }
}
