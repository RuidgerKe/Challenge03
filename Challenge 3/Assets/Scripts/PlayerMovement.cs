using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 8f;
    [SerializeField] private float runSpeed = 12f;
    [SerializeField] private float defaultHeight = 3f;
    [SerializeField] private float crouchSpeed = 5f;
    [SerializeField] private float crouchHeight = 2f;
    [SerializeField] private float crawlSpeed = 2.5f;
    [SerializeField] private float crawlHeight = 1f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 2f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;

    private CharacterController controller;
    Animator animator;
    
    private float groundDistance = 0.4f;
    private bool isGrounded;

    private Vector3 velocity;
    private bool isCrouching;
    private bool isCrawling;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        controller.height = defaultHeight;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (Input.GetKeyDown(KeyCode.C)) ToggleCrouch();
        if (Input.GetKeyDown(KeyCode.LeftControl)) ToggleCrawl();

        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        
        float speed = walkSpeed;

        if (Input.GetKey(KeyCode.W))
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
        }
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            speed = runSpeed;
            print("Running");
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
        }
        else if (isCrouching)
        {
            speed = crouchSpeed;
            print("Crouching");
            animator.SetBool("isCrouching", true);
        }
        else if (isCrawling)
        {
            speed = crawlSpeed;
            print("Crawling");
            animator.SetBool("isCrawling", true);
        }
        
        controller.Move(move * speed * Time.deltaTime);
        
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void ToggleCrouch()
    {
        isCrawling = false;
        
        isCrouching = !isCrouching;
        controller.height = isCrouching ? crouchHeight : defaultHeight;
    }
    
    void ToggleCrawl()
    {
        isCrouching = false;
        
        isCrawling = !isCrawling;
        controller.height = isCrawling ? crawlHeight : defaultHeight;
    }
}