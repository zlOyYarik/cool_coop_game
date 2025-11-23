using UnityEngine;

public class playerController : MonoBehaviour
{
    private Joint joint;
    private Rigidbody rb;
    private Vector3 velocity;

    [SerializeField] private float pickUpDistance;
    [SerializeField] private float throwForce;
    
    public LayerMask whatIsPickUp;
    public Camera playerCam;

    void Start()
    {
        joint = GetComponentInChildren<Joint>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) PickUp();
        if (Input.GetMouseButtonUp(0)) Drop();
        if (Input.GetMouseButtonDown(1)) Drop(true);
    }

    private void PickUp()
    {
        if (!Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out RaycastHit hit, pickUpDistance, whatIsPickUp)) return;
        {
            rb = hit.collider.gameObject.GetComponent<Rigidbody>();
            rb.linearDamping = 15;

            joint.gameObject.transform.position = rb.gameObject.transform.position;
            joint.connectedBody = rb;
        }
    }

    private void Drop(bool isThrow = false)
    {
        if (rb == null) return;

        joint.connectedBody = null;
        rb.linearVelocity = velocity;

        if (isThrow) rb.AddForce(playerCam.transform.forward * throwForce, ForceMode.Impulse);
        rb.linearDamping = 0;
        rb = null;
    }
}
