using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SoodUtils;

public enum OrbitalDirection {
    CounterClockWise = -1,
    ClockWise = 1
}

public class GravityManager2D : GenericSingletonClass<GravityManager2D>
{
    // Start is called before the first frame update

    [Range(0.1f, 10f)] public float timeScale = 1;

    [SerializeField] List<GravitySource> gravitySources = new List<GravitySource>();
    [SerializeField] List<GravityTarget2D> gravityTargets = new List<GravityTarget2D>();
    [SerializeField] float _gravConstant = 1;

    public static float GravitationalConstant
    {
        get { return Instance._gravConstant; }
    }

    void FixedUpdate()
    {
        //Debug.Log("GravityManager2D FixedUpdate");
        foreach (GravitySource source in gravitySources)
        {
            //Debug.Log("GravityManager2D source " + source.name);
            float maxRangeSq = source.maxRange * source.maxRange;
            foreach (GravityTarget2D target in gravityTargets)
            {
                //Debug.Log("GravityManager2D targets " + target.name);
                if (!target.isAffectedByGravity) continue;

                Vector2 dir = source.Position - target.Position;
                float distSq = dir.sqrMagnitude;

                if (distSq > maxRangeSq)
                    continue;

                float acceleration = source.GetAcceleration2(distSq);
                target.ApplyGravity2D(dir.normalized * acceleration * Time.fixedDeltaTime);
            }
        }
    }

    public static void AddGravityTarget(GravityTarget2D target)
    {
        instance?.gravityTargets.Add(target);
    }

    public static void RemoveGravityTarget(GravityTarget2D target)
    {
        instance?.gravityTargets.Remove(target);
    }

    public static void AddGravitySource(GravitySource target)
    {
        instance?.gravitySources.Add(target);
    }

    public static void RemoveGravitySource(GravitySource target)
    {
        instance?.gravitySources.Remove(target);
    }

    private void OnValidate()
    {
        Time.timeScale = timeScale;
    }
}
