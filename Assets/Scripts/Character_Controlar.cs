using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Controlar : MonoBehaviour
{
    Rigidbody rigidbody;
    Vector3 input;
    private Animator animator;
    bool jump = false;
    bool allowJump = true;
    RaycastHit raycastHit;
    float battahSpeed = 2f;

    public Transform cameraTransform; // Reference to the camera
    // public Transform groundCheck;    // the character’s feet
    public float groundDistance = 0.1f;
    public LayerMask groundMask;
    [SerializeField]
    LayerMask layerMask;
     [SerializeField]
        float rayLength = 1f;
        [SerializeField]
        float rayLengthAddition = 0.1f;

     

    private bool isGrounded;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rigidbody.freezeRotation = true;
        
    }

    void Update()
    {
        // Read input
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Get camera-forward movement direction
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Flatten to avoid vertical movement
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // Combine input
        input = (forward * v + right * h).normalized * battahSpeed;
        input.y = rigidbody.velocity.y;

        // Ground check
        // isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        allowJump = Physics.Raycast(transform.position, Vector3.down, out raycastHit, rayLength + rayLengthAddition, groundMask);
         Debug.Log(allowJump);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
        // Animation
        if(Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("start run", true);
            battahSpeed = 5f;
        }
        else
        {
            animator.SetBool("start run", false);
            battahSpeed = 2f;
        }
        
   
    }

    void FixedUpdate()
    {
        // Face movement direction
        Vector3 moveDir = new Vector3(input.x, 0, input.z);
        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.fixedDeltaTime));
            animator.SetFloat("speed", input.magnitude);
            animator.SetBool("start move", true);
        }
        else
        {
            animator.SetFloat("speed", 0);
            animator.SetBool("start move", false);
        }

        // Apply movement
        if (jump && allowJump)
        {
            rigidbody.drag = 0.5f;
            rigidbody.AddForce(Vector3.up * 5f, ForceMode.Impulse);
            animator.SetBool("jump", true);
            jump = false;
        }
        else
        {
            animator.SetBool("jump", false);
            rigidbody.drag = 0;
            rigidbody.velocity = input;
        }
    }
}

