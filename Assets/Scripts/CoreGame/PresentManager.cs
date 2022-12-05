using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentManager : MonoBehaviour
{
    [SerializeField] PresentIndicatorManager presentIndicatorManager;
    Dictionary<int, Present> presents;
    // Start is called before the first frame update
    void Start()
    {
        presents = new Dictionary<int, Present>();
        Present[] presentsArray = GetComponentsInChildren<Present>();
        for(int presentID=0; presentID < presentsArray.Length; ++presentID){
            Present present =  presentsArray[presentID];
            presents[presentID] = present;
            present.PresentID = presentID;
            presentIndicatorManager.RegisterPresent(presentID, present);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollectPresent(int presentID)
    {
        presents.Remove(presentID);
        presentIndicatorManager.OnCollectPresent(presentID);
    }

    public bool CollectedAllPresents()
    {
        return presents.Count == 0;
    }
}
