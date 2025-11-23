using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PickUpItemLeft : MonoBehaviour
{
    public GameObject playerCam;
    GameObject currentItem;

    public float distance;
    public float throwForce;
    private float chargeTime = 0f;
    private float maxChargeTime = 2f;

    bool canPickUp;
    bool pickedUp;

    private void Start()
    {
        pickedUp = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !pickedUp)
        {
            PickUp();
            Debug.Log("лев взял");
        }
        if (Input.GetMouseButton(0) && pickedUp)
        {
            chargeTime += Time.deltaTime;
            chargeTime = Mathf.Min(chargeTime, maxChargeTime);
        }
        if (Input.GetMouseButtonUp(0) && pickedUp)
        {
            Throw(chargeTime / maxChargeTime);
            chargeTime = 0f;
        }
    }

    public void PickUp()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, distance))
        {
            if (hit.transform.tag == "Item")
            {
                if (canPickUp) Drop();

                currentItem = hit.transform.gameObject;
                currentItem.GetComponent<Rigidbody>().isKinematic = true;
                currentItem.GetComponent<BoxCollider>().enabled = false;
                currentItem.transform.parent = transform;
                currentItem.transform.localPosition = Vector3.zero;
                currentItem.transform.localEulerAngles = Vector3.zero;
                canPickUp = true;
                Debug.Log("лев корутина началась");
                StartCoroutine(WaitToPickedUp());

            }
        }
    }

    public void Drop()
    {
        currentItem.transform.parent = null;
        currentItem.GetComponent<Rigidbody>().isKinematic = false;
        currentItem.GetComponent<BoxCollider>().enabled = true;
        canPickUp = false;
        currentItem = null;
        pickedUp = false;
    }

    public void Throw(float charge)
    {
        float actualForce = throwForce * charge;

        if (currentItem != null)
        {
            currentItem.transform.parent = null;
            Rigidbody rb = currentItem.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            currentItem.GetComponent<Collider>().enabled = true;

            rb.AddForce(playerCam.transform.forward * actualForce, ForceMode.Impulse);
            Debug.Log("лев бросок с силой " + actualForce);
            canPickUp = false;
            pickedUp = false;
            currentItem = null;
        }
    }

    IEnumerator WaitToPickedUp()
    {
        yield return new WaitForSeconds(0.1f);
        pickedUp = true;
    }
}
