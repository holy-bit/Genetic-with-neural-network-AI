using UnityEngine;
using System.Collections;


public class CameraController : MonoBehaviour
{

    public float lookRateHorizontal = 2f;
    public float lookRateVertical = 2f;

    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private float yaw;

    [SerializeField]
    private float pitch;

    void Update()
    {
        //Look
        if (Input.GetMouseButton(1))
        {
            yaw   += lookRateHorizontal * Input.GetAxis("Mouse X");
            pitch -= lookRateVertical * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        }
        //Fly
        transform.Translate(0, 0, Input.GetAxis("Vertical") * movementSpeed, Space.Self);
        transform.Translate(Input.GetAxis("Horizontal")* movementSpeed, 0, 0 , Space.Self);
    }
}