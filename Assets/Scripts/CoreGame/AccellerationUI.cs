using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccellerationUI : MonoBehaviour
{
    [SerializeField] float duration=0.5f;
    [SerializeField] float directionMult;
    private float currentDuration=0;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material.SetFloat("_DirectionMult", directionMult);
    }

    // Update is called once per frame
    void Update()
    {
        currentDuration -= Time.deltaTime;
        spriteRenderer.enabled = currentDuration>=0;
        float timeOffset = Mathf.Pow(1-(currentDuration/duration), 2);
        spriteRenderer.material.SetFloat("_TimeOffset", timeOffset);
    }

    public bool Activate()
    {
        if(currentDuration <= 0)
        {
            currentDuration=duration;
            return true;
        }
        return false;
    }

    
}
