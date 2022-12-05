using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentIndicator : MonoBehaviour
{
    Present targetPresent;
    public Present TargetPresent{
        set{ targetPresent = value; }
    }
    float radius;
    public float Radius{
        set{ radius = value; }
    }
    Camera centeredCamera;
    public Camera CenteredCamera{
        set{ centeredCamera = value; }
    }
    Player centeredPlayer;
    public Player CenteredPlayer{
        set{ centeredPlayer = value; }
    }
    float curRadius;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float radius_lerp_mult = (Mathf.Sin(Time.time*Mathf.PI)+1)/2;
        curRadius = Mathf.Lerp(0.9f*radius, radius, radius_lerp_mult);

        Vector3 presentPos = targetPresent.transform.position;
        Vector3 playerPos = centeredPlayer.transform.position;
        Vector3 viewportOffset = centeredCamera.WorldToViewportPoint(playerPos);
        Vector3 targetScreenPos = centeredCamera.WorldToViewportPoint(presentPos) - viewportOffset;
        Vector3 normalizedTargetScreenPos = targetScreenPos;
        normalizedTargetScreenPos.z = 0;
        float newLength = Mathf.Min(curRadius/2, normalizedTargetScreenPos.magnitude);
        normalizedTargetScreenPos = normalizedTargetScreenPos.normalized;
        Vector3 scaledTargetScreenPos = normalizedTargetScreenPos*newLength;
        Vector3 newScreenPos = new Vector3(scaledTargetScreenPos.x, scaledTargetScreenPos.y) + viewportOffset;
        Vector3 newWorldPos = centeredCamera.ViewportToWorldPoint(newScreenPos);
        newWorldPos.z = transform.parent.position.z;
        this.transform.position = newWorldPos;

        Vector3 posDiff = presentPos - playerPos;
        float rotation = Mathf.Atan2(posDiff.y, posDiff.x)*Mathf.Rad2Deg-90;
        
        Quaternion newRotation = Quaternion.Euler(0,0,rotation);
        transform.SetPositionAndRotation(newWorldPos, newRotation);
    }
    
    public void OnCollectPresent()
    {
        Destroy(this.gameObject);
    }
}
