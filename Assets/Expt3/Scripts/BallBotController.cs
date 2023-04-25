using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallBotController : MonoBehaviour
{
    public float maxRollForce = 20f;
    public float dashForce = 20f;
    public float upwardDashForce = 25f;
    public float jumpForce = 15f;

    [SerializeField] GameObject m_body;
    [SerializeField] GameObject m_turret;
    [SerializeField] GameObject m_legs;
    [SerializeField] Transform m_projectileSpawn;
    [SerializeField] GameObject m_projectilePrefab;
    [SerializeField] Transform m_stopEffectSpawn;
    [SerializeField] ParticleSystem m_stopEffect;
    [SerializeField] SphereCollider m_ballCollider;
    [SerializeField] LayerMask m_floorLayerMask;
    [SerializeField] Transform m_followTarget;
    [SerializeField] float m_minDamageSpeed = 6f;
    [SerializeField] TrailRenderer m_trailRenderer;
    [SerializeField] float speedDamageMultiplier;
    [SerializeField] Health m_health;
    [SerializeField] float downForce = 50f;

    bool m_isRolling = true;
    bool wasRollingChanged = false;
    bool isJumping = false;
    bool isDashing = false;
    bool isGripable = false;

    public bool IsRolling
    {
        get
        {
            return m_isRolling;
        }
        set
        {
            m_isRolling = value;
            wasRollingChanged = true;
            OnIsRollingChanged();
        }
    }

    Rigidbody m_rigidbody;
    Transform cameraReference;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_health = GetComponent<Health>();
    }

    void Start()
    {
        cameraReference = Camera.main.transform;
        OnIsRollingChanged();
    }

    // Update is called once per frame
    void Update()
    {

        Collider[] colliders = Physics.OverlapSphere(transform.position, m_ballCollider.radius + 0.01f, m_floorLayerMask);


        isGripable = colliders.Length > 0;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (IsRolling)
            {
                if (isGripable)
                {
                    IsRolling = false;
                }
            }
            else
            {
                isDashing = true;
                IsRolling = true;
            }
        }
        Debug.DrawRay(transform.position, Vector3.down, Color.red, 5f);
        if (Input.GetKeyDown(KeyCode.Space) && Physics.SphereCast(transform.position, .2f, Vector3.down, out RaycastHit hitInfo, 1f, m_floorLayerMask))
        {
            if (!IsRolling)
            {
                IsRolling = true;
                //isJumping = true;
            }
            //else
            //{
            //    if (isGrounded)
            //    {
            //        IsRolling = false;
            //    }
            //}
            isJumping = true;
        }

        if (!m_isRolling && Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        Vector3 inputMoveVec = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        inputMoveVec = Quaternion.AngleAxis(cameraReference.rotation.eulerAngles.y, Vector3.up) * inputMoveVec;

        Vector3 rollVec = inputMoveVec.normalized;

        // Roll
        if (m_isRolling)
        {
            if (rollVec != Vector3.zero)
            {
                m_rigidbody.AddForce(maxRollForce * rollVec * Time.deltaTime, ForceMode.Acceleration);
            }
        }

        if (isGripable)
        {
            m_rigidbody.AddForce(Vector3.down * downForce * Time.deltaTime, ForceMode.Acceleration);
        }

        if (!m_isRolling)
        {
            if (Physics.Raycast(cameraReference.position, cameraReference.forward, out RaycastHit hit))
            {
                m_turret.transform.LookAt(hit.point);
            }
            else
            {
                m_turret.transform.forward = cameraReference.forward;
            }
        }

        // Dash & Jump
        if (wasRollingChanged)
        {
            if (m_isRolling)
            {
                //if (isJumping)
                //{
                //    Vector3 jumpVec = transform.up * jumpForce;
                //    m_rigidbody.AddForce(jumpVec, ForceMode.Impulse);
                //    isJumping = false;
                //}
                //else
                if (isDashing)
                {
                    Vector3 dashVector = dashForce * inputMoveVec;
                    dashVector.y = upwardDashForce;
                    m_rigidbody.AddForce(dashVector, ForceMode.Impulse);
                    isDashing = false;
                }
            }
            wasRollingChanged = false;
        }

        if (isJumping)
        {
            Vector3 jumpVec = Vector3.up * jumpForce;
            m_rigidbody.AddForce(jumpVec, ForceMode.Impulse);
            isJumping = false;
        }


        float currentSpeed = m_rigidbody.velocity.magnitude;
        HUDManager.Instance.UpdateSpeedUI(currentSpeed);

        m_trailRenderer.enabled = currentSpeed > m_minDamageSpeed;

        
    }

    public void OnHealthChange()
    {
        HUDManager.Instance.UpdateHealth(m_health.currentHealth, m_health.maxHealth);
    }

    void Shoot()
    {
        Instantiate(m_projectilePrefab, m_projectileSpawn.position, m_projectileSpawn.rotation);
    }

    private void OnApplicationFocus(bool focus)
    {
        //Cursor.lockState = focus ? CursorLockMode.Locked : CursorLockMode.None;
    }

    void OnIsRollingChanged()
    {
        //m_rigidbody.isKinematic = !m_isRolling;
        m_rigidbody.constraints = m_isRolling ? RigidbodyConstraints.None : RigidbodyConstraints.FreezeAll;
        m_turret.SetActive(!m_isRolling);
        m_legs.SetActive(!m_isRolling);
        transform.up = Vector3.up;
        if (!m_isRolling)
        {
            Instantiate(m_stopEffect, m_stopEffectSpawn.transform.position, Quaternion.identity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TriggerEnter " + other.name);
        float speed = m_rigidbody.velocity.magnitude;
        if (speed > m_minDamageSpeed
            && other.attachedRigidbody != null
            && other.attachedRigidbody.TryGetComponent(out Health healthComponent))
        {
            float damage = Mathf.Abs(speed - m_minDamageSpeed) * speedDamageMultiplier;
            Debug.Log("Damage " + damage);
            healthComponent.TakeDamage(damage);
        }
    }
}
