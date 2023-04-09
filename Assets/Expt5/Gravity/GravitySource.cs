using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoodUtils;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GravitySource : MonoBehaviour
{
    public float mass = 1f;
    public float constGravityDistance;
    public float maxRange = 99999f;

    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
    }

    void OnEnable()
    {
        GravityManager2D.AddGravitySource(this);
    }

    private void OnDisable()
    {
        GravityManager2D.RemoveGravitySource(this);
    }

    public Vector3 GetOrbitalVel(Vector3 pos, OrbitalDirection dir)
    {
        Vector3 distVec = transform.position - pos;
        float dist = distVec.magnitude;
        float vel = Mathf.Sqrt(mass / (dist < constGravityDistance? constGravityDistance : dist));
        Vector3 result = Quaternion.Euler(0, 0, 90 * (int)dir) * distVec.normalized * vel;
        return result;
    }

    public float GetAcceleration(Vector3 position)
    {
        Vector2 dir = transform.position - position;
        float distSq = dir.sqrMagnitude;
        if (distSq < constGravityDistance * constGravityDistance)
        {
            return GetTrueAcceleration(constGravityDistance);
        }
        return GetTrueAcceleration2(distSq);
    }

    public float GetAcceleration(float distance)
    {
        if (distance < constGravityDistance)
        {
            return GetTrueAcceleration(constGravityDistance);
        }
        return GetTrueAcceleration(distance);
    }

    public float GetAcceleration2(float distSq)
    {
        if (distSq < constGravityDistance * constGravityDistance)
        {
            return GetTrueAcceleration(constGravityDistance);
        }
        return GetTrueAcceleration2(distSq);
    }

    private float GetTrueAcceleration(float dist)
    {
        return mass / Mathf.Max(1, (dist * dist)); // a = GM/r^2, Assumes G = 1
    }

    private float GetTrueAcceleration2(float distSq)
    {
        return mass / Mathf.Max(1, distSq); // a = GM/r^2, Assumes G = 1
    }



#if UNITY_EDITOR
        [Header("Gizmo Settings")]
    public bool showOrbitalMarkers;
    public int steps = 25;
    public float stepSize = 20f;
    public bool onSelectedOnly = true;


    private void OnDrawGizmosSelected()
    {
        if (!onSelectedOnly) return;
        
        DrawGizmos();

    }

    private void OnDrawGizmos()
    {
        if (onSelectedOnly) return;
        DrawGizmos();
    }

    void DrawGizmos()
    {
        if (!showOrbitalMarkers) return;
        Gizmos.color = Color.gray;
        SoodGizmos.WireDisc2D(transform.position, maxRange);
        SoodGizmos.WireDisc2D(transform.position, constGravityDistance);
        for (float i = 1; i < steps; i += 1)
        {
            Vector3 pos = transform.up * stepSize * i;
            Handles.Label(pos, $"{pos}: V-{GetOrbitalVel(pos, OrbitalDirection.ClockWise).magnitude}, A-{GetAcceleration(pos)}");
        }
    }
#endif
}
