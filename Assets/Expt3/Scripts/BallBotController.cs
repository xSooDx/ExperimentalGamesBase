using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallBotController : MonoBehaviour
{
    public float maxRollForce = 20f;
    public float dashForce = 20f;
    public float upwardDashForce = 25f;

    [SerializeField] GameObject m_body;
    [SerializeField] GameObject m_turret;
    [SerializeField] GameObject m_legs;
    [SerializeField] Transform m_projectileSpawn;
    [SerializeField] GameObject m_projectilePrefab;
    [SerializeField] Transform m_stopEffectSpawn;
    [SerializeField] ParticleSystem m_stopEffect;
    [SerializeField] SphereCollider m_ballCollider;
    [SerializeField] LayerMask m_floorLayerMask;


    bool m_isRolling = true;
    bool wasRollingChanged = false;
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
    }

    void Start()
    {
        cameraReference = Camera.main.transform;
        OnIsRollingChanged();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (IsRolling)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, m_ballCollider.radius + 0.01f, m_floorLayerMask);
                if (colliders.Length > 0)
                {
                    IsRolling = false;
                }
            }
            else
            {
                IsRolling = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

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
        inputMoveVec.Normalize();

        if (m_isRolling && inputMoveVec != Vector3.zero)
        {
            m_rigidbody.AddForce(maxRollForce * inputMoveVec * Time.deltaTime, ForceMode.Acceleration);
        }
        if(!m_isRolling)
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

        if (wasRollingChanged)
        {
            if (m_isRolling && inputMoveVec != Vector3.zero)
            {
                Vector3 dashVector = dashForce * inputMoveVec;
                dashVector.y = upwardDashForce;
                m_rigidbody.AddForce(dashVector, ForceMode.Impulse);
            }
            wasRollingChanged = false;
        }
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
        m_rigidbody.constraints = m_isRolling? RigidbodyConstraints.None : RigidbodyConstraints.FreezeAll;
        m_turret.SetActive(!m_isRolling);
        m_legs.SetActive(!m_isRolling);
        transform.up = Vector3.up;
        if(!m_isRolling)
        {
            Instantiate(m_stopEffect, m_stopEffectSpawn.transform.position, Quaternion.identity);
        }
    }
}
