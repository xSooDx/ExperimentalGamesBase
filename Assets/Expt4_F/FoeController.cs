using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIBaseScirpt))]
public class FoeController : MonoBehaviour
{
    [SerializeField] List<string> enemyTags;
    AIBaseScirpt baseScirpt;
    Transform target = null;

    private void Awake()
    {
        baseScirpt = GetComponent<AIBaseScirpt>();
        baseScirpt.OnSenseCallback = OnSense;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }


    void OnSense(GameObject sensedObject)
    {
        if(!target)
        {
            target = sensedObject.transform;
            baseScirpt.MoveTo(target);
        }
    }
}
