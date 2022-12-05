using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowingCamera2D : MonoBehaviour
{
    [SerializeField] public Transform targetTransform;
    [SerializeField] Vector3 screenOffset;
    [SerializeField] Vector2 screenMargin;
    [SerializeField] float speed;
    Camera cam;
    bool duringRespawn;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        duringRespawn = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 positionDiff = GetPositionDifference();
        float distance = positionDiff.magnitude;
        Vector3 direction = positionDiff;
        if(distance>=10e-6){
            direction /= distance;
        }

        if(duringRespawn && distance<=1)
        {
            duringRespawn = false;
            Time.timeScale = 1;
        }

        Vector3  curPos = transform.position;

        Vector3 movementVec = Time.unscaledDeltaTime*speed*direction;
        movementVec.z = 0;
        if(movementVec.magnitude > positionDiff.magnitude)
        {
            movementVec = positionDiff;
        }
        curPos += movementVec;

        // curPos.x = Mathf.Clamp(curPos.x, targetPosition.x+bottomLeftDiff.x, targetPosition.x+topRightDiff.x);
        // curPos.y = Mathf.Clamp(curPos.y, targetPosition.y+bottomLeftDiff.y, targetPosition.y+topRightDiff.y);

        transform.position = curPos;
    }

    void Reset()
    {
        
    }

    Vector3 GetPositionDifference()
    {
        Vector3 targetPosition = targetTransform.position; 
        Vector3 screenSize = new Vector3(Screen.width, Screen.height, 0);

        Vector3 screenMargin3D = new Vector3(screenMargin.x, screenMargin.y, 0);
        float zDiff = targetPosition.z - transform.position.z;
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0,0,zDiff)+screenMargin3D);
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1,1,zDiff)-screenMargin3D);
        Vector3 cameraWorldPos = cam.ViewportToWorldPoint(new Vector3(screenOffset.x, screenOffset.y,zDiff));
        Vector3 bottomLeftDiff = bottomLeft-cameraWorldPos;
        Vector3 topRightDiff = topRight-cameraWorldPos;

        Vector3 positionDiff = targetPosition - cameraWorldPos;
        positionDiff.z = 0;
        return positionDiff;
    }

    public void OnRespawn()
    {
        duringRespawn = true;
        Time.timeScale = 0.01f;
    }
}
