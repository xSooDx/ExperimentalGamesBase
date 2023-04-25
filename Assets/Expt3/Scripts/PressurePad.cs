using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePad : MonoBehaviour
{
    public float pressureDuration;
    public Transform body;
    public UnityEvent onPadCaptured;

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
        if (other.attachedRigidbody && other.attachedRigidbody.CompareTag("Player"))
        {
            currentPressure -= Time.deltaTime;
            body.localScale = startScale * currentPressure / pressureDuration;
            if (body.localScale.sqrMagnitude < 0.01f)
            {
                body.localScale = startScale * 0.1f;
            }
            if (currentPressure <= 0) {
                onPadCaptured.Invoke();
                Destroy(this.gameObject); 
            }
        }
    }
}
