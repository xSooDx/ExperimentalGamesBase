using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GravityTarget2D : MonoBehaviour
{
    Rigidbody2D rb2D;

    public bool isAffectedByGravity = true;

    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void OnEnable()
    {
        GravityManager2D.AddGravityTarget(this);
    }

    private void OnDisable()
    {
        GravityManager2D.RemoveGravityTarget(this);
    }

    public void ApplyGravity2D(Vector2 acceleration)
    {
        rb2D.velocity += acceleration;
        //Debug.Log($"GravityTarget2D: {this.name}, ApplyGravity2D: {acceleration}");
        //Debug.DrawLine(rb2D.position, rb2D.position + acceleration.normalized, Color.blue, 1f);
    }
}
