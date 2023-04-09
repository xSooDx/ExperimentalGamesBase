using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class SaviorController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    CharacterController characterController;
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0,Input.GetAxisRaw("Vertical")).normalized;
        characterController.SimpleMove(moveDir * moveSpeed);
        
    }
}
