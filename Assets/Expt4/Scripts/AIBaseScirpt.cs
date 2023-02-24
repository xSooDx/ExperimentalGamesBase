using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class AIBaseScirpt : MonoBehaviour
{
    int _state;
    public Transform attackTransform;
    public float attackRate = 1.5f;
    public float attackRadius = 1f;
    public LayerMask attackLayerMask;
    public float attackDamage = 5f;

    public LayerMask sensorLayerMask;
    public float sensorRange = 50f;

    public delegate void OnSenseDelegate(GameObject sensedObject);

    public OnSenseDelegate OnSenseCallback;

    bool targetSetThisFrame = false;

    Transform target = null;

    NavMeshAgent navAgent;
    Animator animator;

    float attackTimer;

    int State // 0-idle, 1-move, 2-Attack
    {
        get { return _state; }
        set
        {
            _state = value;
            if (_state == 0)
            {
                target = null;
            }
            animator.SetInteger("State", _state);
        }
    }

    protected virtual void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        attackTimer = attackRate;
        State = 0;
        StartCoroutine(UpdateTargetTracking());
        StartCoroutine(AttackUpdate());
    }

    IEnumerator UpdateTargetTracking()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            if (target && State == 1)
            {
                if (Vector3.Distance(transform.position, target.position) > navAgent.stoppingDistance)
                {
                    navAgent.destination = target.position;
                }
                else
                {
                    navAgent.isStopped = true;
                    transform.LookAt(target);
                    State = 2;
                }
            }
            else if (State == 0)
            {
                DoSense();
            }
        }
    }

    IEnumerator AttackUpdate()
    {
        while (true)
        {
            if (State == 2 && target)
            {
                FaceTarget(target.position);
                if (attackTimer <= 0f)
                {
                    //DoAttack();
                    attackTimer = attackRate;
                }
            }
            attackTimer -= attackTimer > 0 ? Time.deltaTime : 0f;
            if (!target)
            {
                State = 0;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, navAgent.angularSpeed * Time.deltaTime);
    }

    public virtual void DoAttack()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.red, 0.25f);
        Collider[] hits = Physics.OverlapSphere(attackTransform.position, attackRadius, attackLayerMask);
        foreach (Collider hit in hits)
        {
            if (hit.attachedRigidbody)
            {
                hit.attachedRigidbody.GetComponent<Health>()?.TakeDamage(attackDamage);
            }
        }
    }

    public virtual void MoveTo(Vector3 target)
    {
        navAgent.destination = target;
        this.target = null;
        State = 1;
    }

    public virtual void MoveTo(Transform targetTransform)
    {
        target = targetTransform;
        navAgent.destination = target.position;
        targetSetThisFrame = true;
        State = 1;
    }

    public void DoSense()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, sensorRange, sensorLayerMask);
        Array.Sort(hits, (x, y) =>
        {
            return (transform.position - x.transform.position).sqrMagnitude.CompareTo((transform.position - y.transform.position).sqrMagnitude);
        });
        foreach (Collider hit in hits)
        {
            if (hit.attachedRigidbody)
            {
                if (OnSenseTarget(hit.attachedRigidbody.transform))
                {
                    break;
                }
            }
            else
            {
                if (OnSenseTarget(hit.transform))
                {
                    break;
                }
            }
        }
    }

    bool OnSenseTarget(Transform target)
    {
        MoveTo(target);
        return true;
    }

    private void OnDrawGizmos()
    {

        if (navAgent && navAgent.hasPath)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, navAgent.destination);
            Gizmos.DrawWireSphere(navAgent.destination, navAgent.stoppingDistance < .25f ? .25f : navAgent.stoppingDistance);
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sensorRange);
        if (attackTransform)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackTransform.position, attackRadius);
        }
    }
}
