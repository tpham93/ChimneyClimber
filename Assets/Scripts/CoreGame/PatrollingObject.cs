using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Movement{
    public Vector3 translation;
    public float rotationDirection=1;
}

public class PatrollingObject : MonoBehaviour
{
    [SerializeField] public Movement[] movements;
    [SerializeField] public float speed;
    [SerializeField] public bool allowRotation;
    [SerializeField] public float overflowRotationDirection=1;
    private Vector3[] route;
    private float[] rotationDirections;
    private int currentTargetIndex;
    private Rigidbody2D rbody2D;
    private float lastDirectionChange=0;
    private float offsetAngle;
    private float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        route = new Vector3[movements.Length+1];
        route[0] = transform.position;
        for(int i = 0; i < movements.Length; ++i)
        {
            route[i+1] = movements[i].translation + transform.position;
        }
        rotationDirections = new float[movements.Length+1];
        rotationDirections[0] = overflowRotationDirection;
        for(int i = 0; i < movements.Length; ++i)
        {
            rotationDirections[i+1] = movements[i].rotationDirection;
        }
        currentTargetIndex = 1;
        rbody2D = GetComponent<Rigidbody2D>();

        offsetAngle = Random.Range(0, 360);
        Vector3 spriteSite = GetComponent<SpriteRenderer>().bounds.size;
        float radius = Mathf.Max(spriteSite.x, spriteSite.y);
        float circumference = 2 * Mathf.PI * radius;
        rotationSpeed = speed/circumference*360.0f;
    }

    // Update is called once per frame
    void Update()
    {
    }

    float GetCurrentRotation()
    {
        return offsetAngle -(rotationDirections[currentTargetIndex] * rotationSpeed * (Time.timeSinceLevelLoad-lastDirectionChange));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 currentTargetPosition = route[currentTargetIndex];
        Vector2 posDiff = currentTargetPosition - transform.position;
        float posDist = posDiff.magnitude;
        Vector2 posDir = posDiff/posDist;

        float distToTravel = Time.fixedDeltaTime * speed;

        Vector2 newPosition = transform.position;
        if(posDist <= distToTravel)
        {
            offsetAngle = GetCurrentRotation();
            lastDirectionChange = Time.timeSinceLevelLoad;

            newPosition = currentTargetPosition;
            currentTargetIndex = (currentTargetIndex + 1)%route.Length;
        }else{
            newPosition += posDir * distToTravel;
        }
        rbody2D.MovePosition(newPosition);
        rbody2D.SetRotation(GetCurrentRotation());
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(PatrollingObject))]
[InitializeOnLoadAttribute]
[ExecuteInEditMode] 
public class PatrollingObjectEditor : Editor
{
    bool subscribed = false;
    private void OnEnable()
    {
        if(!subscribed)
        {
            subscribed = true;
            SceneView.duringSceneGui += CustomOnSceneGUI;
        }
    }
    private void OnDisable()
    {
        PatrollingObject linkedObject = (PatrollingObject)target;
        if(subscribed && (linkedObject==null || !linkedObject.isActiveAndEnabled))
        {
            subscribed = false;
            SceneView.duringSceneGui -= CustomOnSceneGUI;
        }
    }
    
    private void CustomOnSceneGUI(SceneView view)
    {
        PatrollingObject linkedObject = (PatrollingObject)target;

        if(linkedObject==null)
        {
            OnDisable();
            return;
        }

        Handles.color = Color.green;

        Gizmos.matrix = linkedObject.transform.localToWorldMatrix;

        int nPos = linkedObject.movements.Length;

        Vector3[] route = new Vector3[linkedObject.movements.Length+1];
        route[0] = linkedObject.transform.position;
        for(int i = 0; i < linkedObject.movements.Length; ++i)
        {
            route[i+1] = linkedObject.movements[i].translation + route[0];
        }

        for(int i = 0; i < route.Length; ++i)
        {
            Vector3 startPos = route[i];
            Vector3 endPos = route[(i+1)%route.Length];

            Handles.DrawAAPolyLine(startPos, endPos);
        }

        EditorGUI.BeginChangeCheck();

        Vector3[] newTargetPositions = new Vector3[nPos];

        for(int i = 0; i < nPos; ++i)
        {
            float size = 1f/50;
            Vector3 snap = Vector3.one * 1f/50;
            newTargetPositions[i] = Handles.FreeMoveHandle(
                linkedObject.movements[i].translation+route[0],
                size*Camera.current.orthographicSize,
                snap*Camera.current.orthographicSize,
                Handles.CircleHandleCap
            )-route[0];
        }

        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(linkedObject, "Update position");

            for(int i = 0; i < nPos; ++i)
            {
                linkedObject.movements[i].translation = newTargetPositions[i];
            }
        }

    }
}
#endif