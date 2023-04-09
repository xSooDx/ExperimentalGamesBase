using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoodUtils;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Collider2D))]
public class AtmosphereDragEffector : MonoBehaviour
{
    public float atmosphereOutterRadius = 1;
    public float atmospherInnerRadius = 1f;
    public AnimationCurve densityGradient;
    public float dragCoeficient = 0.5f;

    void Update()
    {

    }


    private void OnTriggerStay2D(Collider2D other)
    {
        float distance = Vector2.Distance(transform.position, other.transform.position);
        float density = SampleAtmosphere(distance);
        float dargForce = density * other.attachedRigidbody.velocity.sqrMagnitude * -dragCoeficient;
        Vector2 force = dargForce * other.attachedRigidbody.velocity.normalized;
        Debug.Log("AtmosphereDragEffector " + other.name +", " + force);
        other.attachedRigidbody.AddForce(force);
        //other.attachedRigidbody.AddTorque() based on objects UpDirection
    }

    public float SampleAtmosphere(Vector3 position)
    {
        float distance = Vector2.Distance(transform.position, position);
        return SampleAtmosphere(distance);
    }

    public float SampleAtmosphere(float distance)
    {
        float sampleX = (atmosphereOutterRadius - distance) / (atmosphereOutterRadius - atmospherInnerRadius);

        return densityGradient.Evaluate(sampleX);
    }

#if UNITY_EDITOR
    [Header("Gizmo Settings")]
    public bool OnSelectedOnly = true;
    public bool showHeightMarkers;
    public int steps = 25;

    private void OnDrawGizmosSelected()
    {
        if (!OnSelectedOnly) return;
        DrawGizmos();
    }

    private void OnDrawGizmos()
    {
        if (OnSelectedOnly) return;
        DrawGizmos();
    }

    private void DrawGizmos()
    {
        if (!showHeightMarkers) return;
        Gizmos.color = Color.cyan;
        SoodGizmos.WireDisc2D(transform.position, atmosphereOutterRadius);
        SoodGizmos.WireDisc2D(transform.position, atmospherInnerRadius);
        float stepSize = (atmosphereOutterRadius - atmospherInnerRadius) / steps;
        for (float i = 1; i < steps+ 1; i += 1)
        {
            Vector3 pos = transform.up * (atmospherInnerRadius + (stepSize * i));
            Handles.Label(pos, $"{pos}: {SampleAtmosphere(pos)}");
        }
    }
#endif

}
