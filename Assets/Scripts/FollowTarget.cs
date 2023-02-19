using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;

    [SerializeField] float lookSensitivity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        float hAxis = Input.GetAxisRaw("Mouse X");
        float vAxis = Input.GetAxisRaw("Mouse Y");
        Vector3 roatationVec = new Vector3(-vAxis, hAxis, 0) * lookSensitivity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + roatationVec);
        UpdatePosition();
    }

    // Update is called once per frame
    //void LateUpdate()
    //{
    //    UpdatePosition();
    //}


    void UpdatePosition()
    {
        transform.position = target.position + offset;
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        UpdatePosition();
    }
#endif
}
