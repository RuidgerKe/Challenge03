using UnityEngine;

public class ExtendedThirdPersonMovement : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float crouchSpeed = 1.5f;
    public float crawlSpeed = 1f;
    public float rotationSpeed = 700f;
    public float crouchHeight = 0.5f;
    public float standingHeight = 2f;

    public float off = 0f;
    public float on = 1f;
    private int CrouchIndexLayer;

    private CharacterController characterController;
    private float targetHeight;
    private bool isCrouching = false;
    Animator animator;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        targetHeight = characterController.height;
        animator = GetComponent<Animator>();
        Debug.Log(animator);
        CrouchIndexLayer = animator.GetLayerIndex("Crouch");
    }

    void Update()
    {
        HandleMovementInput();

        // Toggle crouch
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCrouch();
            animator.SetBool("isCrouching", true);
            animator.SetLayerWeight(CrouchIndexLayer, on);
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            
            animator.SetBool("isCrouching", false);
            animator.SetLayerWeight(CrouchIndexLayer, off);
        }

        // Toggle crawl
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            ToggleCrawl();
            animator.SetBool("isCrawling", true);
        }
       

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetBool("isRunning", true);
        }
    }

    void HandleMovementInput()
    {
        float speed = GetSpeed();

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        Vector3 moveDirection = Quaternion.Euler(0f, transform.eulerAngles.y, 0f) * Vector3.forward;
        characterController.Move(moveDirection.normalized * speed * Time.deltaTime);
    }

    float GetSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            return isCrouching ? crawlSpeed : runSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            return crawlSpeed;
        }
        else if (isCrouching)
        {
            return crouchSpeed;
        }
        else
        {
            return walkSpeed;
        }
    }

    void ToggleCrouch()
    {
        isCrouching = !isCrouching;
        targetHeight = isCrouching ? crouchHeight : standingHeight;
    }

    void ToggleCrawl()
    {
        if (isCrouching)
        {
            isCrouching = false;
            targetHeight = standingHeight;
        }
    }

    void LateUpdate()
    {
        // Adjust the height smoothly
        float height = Mathf.Lerp(characterController.height, targetHeight, 5f * Time.deltaTime);
        characterController.height = height;
    }
}
