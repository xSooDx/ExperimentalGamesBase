using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;

    [Range(0f, 100f)]
    public float lookSensitivity = 0.5f;
    // Start is called before the first frame update

    void Start()
    {
        HUDManager.Instance.ft = this;
        lookSensitivity = HUDManager.Instance.currentSensitivity;
        transform.parent = null;
    }

    private void Update()
    {
        float hAxis = Input.GetAxis("Mouse X");
        float vAxis = Input.GetAxis("Mouse Y");

        //verticalRotation -= mouseY;
        //verticalRotation = Mathf.Clamp(verticalRotation, -clampAngle, clampAngle);

        //transform.localRotation = Quaternion.Euler(verticalRotation, 0.0f, 0.0f);
        //transform.parent.Rotate(Vector3.up * mouseX);

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
