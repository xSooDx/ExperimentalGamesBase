using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;

    [SerializeField]
    [Range(0f, 1f)]
    float lookSensitivity;
    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
    }

    private void Update()
    {
        float hAxis = Input.GetAxisRaw("Mouse X") * Screen.width;
        float vAxis = Input.GetAxisRaw("Mouse Y") * Screen.height;
        Vector3 roatationVec = new Vector3(-vAxis, hAxis, 0) * lookSensitivity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + roatationVec);
        UpdatePosition();
    }

    private void OnApplicationFocus(bool focus)
    {
        Cursor.lockState = focus ? CursorLockMode.Locked : CursorLockMode.None;
    }


    void UpdatePosition()
    {
        transform.position = target.position + offset;
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        if(target) UpdatePosition();
    }
#endif
}
