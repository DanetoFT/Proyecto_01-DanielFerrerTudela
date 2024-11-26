using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    public float moveSpeed;
    public float mouseSensibility;
    public Transform cameraTransform;
    [Header("Camera Settings")]
    public float cameraHeight;
    public float cameraDistance;
    public float cameraSmootSpeed;

    float cameraPitch; //Control vertical de la inclinacion de la camara

    private void Start()
    {
        CursorSetUp();
    }
    private void Update()
    {
        PlayerMovementWASD();
        CameraFollow();
    }
    private void LateUpdate()
    {
        CameraSetUp();
    }

    void CursorSetUp()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void PlayerMovementWASD()
    {
        float moveX = 0f;
        float moveZ = 0f;

        if (Input.GetKey(KeyCode.W)) moveZ = +1f;
        if (Input.GetKey(KeyCode.S)) moveZ = -1f;
        if (Input.GetKey(KeyCode.A)) moveX = -1f;
        if (Input.GetKey(KeyCode.D)) moveX = +1f;


        Vector3 directionMovement = (transform.right * moveX + transform.forward * moveZ).normalized;
        transform.position += directionMovement * moveSpeed * Time.deltaTime;
    }

    void CameraFollow()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);


    }
    void CameraSetUp()
    {
        Vector3 newPos = transform.position - transform.forward * cameraDistance + Vector3.up * cameraHeight;

        cameraTransform.position = Vector3.Lerp(cameraTransform.position, newPos, Time.deltaTime * cameraSmootSpeed);

        cameraTransform.LookAt(transform.position + Vector3.up * cameraHeight * 0.5f);
    }
}
