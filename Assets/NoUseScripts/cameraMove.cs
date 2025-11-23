using UnityEngine;

public class cameraMove : MonoBehaviour
{
    public Transform cameraPos;

    private void Update()
    {
        transform.position = cameraPos.position;
    }
}
