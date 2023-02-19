using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePad : MonoBehaviour
{
    public float pressureDuration;
    public Transform body;

    float currentPressure;
    Vector3 startScale;
    // Start is called before the first frame update
    void Start()
    {
        currentPressure = pressureDuration;
        startScale = body.localScale;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.attachedRigidbody && other.attachedRigidbody.CompareTag("Player"))
        {
            currentPressure -= Time.deltaTime;
            body.localScale = startScale * currentPressure / pressureDuration;
            if (currentPressure <= 0) Destroy(this.gameObject);
        }
    }
}
