using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentIndicatorManager : MonoBehaviour
{
    [SerializeField] PresentIndicator presentIndicatorPrefab;
    Dictionary<int, PresentIndicator> presentIndicators;
    [SerializeField] float radius;
    private Player player;
    public Player CenteredPlayer
    {
        get{ return player;}
        set{ player=value;}
    }
    // Start is called before the first frame update
    void Awake()
    {
        presentIndicators = new Dictionary<int, PresentIndicator>();
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RegisterPresent(int presentID, Present present)
    {
        PresentIndicator presentIndicator = Instantiate<PresentIndicator>(
            presentIndicatorPrefab, 
            Vector3.zero, 
            Quaternion.identity, 
            this.transform);
        presentIndicators.Add(presentID, presentIndicator);
        presentIndicator.TargetPresent = present;
        presentIndicator.Radius = radius;
        presentIndicator.CenteredCamera = transform.parent.GetComponent<Camera>();
        presentIndicator.CenteredPlayer = player;
    }
    
    public void OnCollectPresent(int presentID)
    {
        presentIndicators[presentID].OnCollectPresent();
        presentIndicators.Remove(presentID);
    }
}
