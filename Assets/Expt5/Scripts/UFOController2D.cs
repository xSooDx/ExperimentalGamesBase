using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class UFOController2D : MonoBehaviour
{
    [SerializeField] float orbitalSpeed;
    [SerializeField] float minOrbitalSpeed, maxOrbitalSpeed;
    [SerializeField] float verticalAcceleration;
    [SerializeField] float timeToFire;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawn;
    [SerializeField] Transform projectileSpawn2;

    float fireTimer = 0f;

    float currentVerticalVel = 0f;

    Rigidbody2D myRigidBody2D;
    Transform orbitCenter;

    void Awake()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        orbitCenter = GameObject.FindGameObjectWithTag("OrbitCenter")?.transform;
        if (orbitCenter == null)
        {
            Debug.LogError("Need to use parent as orbit center");
        }
        myRigidBody2D.isKinematic = true;
        currentVerticalVel = 0f;
        transform.parent = orbitCenter;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (false)
        {
            float input = Input.GetAxisRaw("Vertical");
            if (input != 0f)
            {
                currentVerticalVel += input * verticalAcceleration * Time.deltaTime;
            }
            Vector3 currPos = transform.localPosition;
            currPos.y += currentVerticalVel * Time.deltaTime;
            transform.localPosition = currPos;
        }
        else
        {
            float inputV = Input.GetAxis("Vertical");
            
            Vector3 currPos = transform.localPosition;
            currPos.y += inputV * verticalAcceleration * Time.deltaTime;
            transform.localPosition = currPos;

            float inputH = Input.GetAxis("Horizontal") + 1f;
            inputH /= 2f;
            orbitalSpeed = Mathf.Lerp(minOrbitalSpeed, maxOrbitalSpeed, inputH);
        }

        orbitCenter.Rotate(Vector3.forward, orbitalSpeed * Time.deltaTime);
    }

    void Shoot()
    {
        if (fireTimer <= 0f)
        {
            Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);
            Instantiate(projectilePrefab, projectileSpawn2.position, projectileSpawn2.rotation);
            fireTimer = timeToFire;
        }

    }
}
