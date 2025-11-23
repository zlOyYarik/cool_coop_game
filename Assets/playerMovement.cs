using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;

public class playerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    
    public float groundDrug;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool isGrounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        MyInput();
        SpeedControl();

        //ground check:
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2F, whatIsGround);

        if (isGrounded)
            rb.linearDamping = groundDrug;
        else
            rb.angularDamping = 0;

    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void Movement()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }
}
