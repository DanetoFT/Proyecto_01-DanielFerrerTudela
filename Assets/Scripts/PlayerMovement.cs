using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    public float moveSpeed;
    public float mouseSensibility;
    public Transform cameraTransform;
    public Animator playerAnim;
    [Header("Camera Settings")]
    public float cameraHeight;
    public float cameraDistance;
    [SerializeField] float cameraSmootSpeed;
    public float smoothSpeedMax;

    float cooldown = 0f;
    float finalTime = 3f;

    float cameraPitch; //Control vertical de la inclinacion de la camara

    private void Start()
    {
        CursosSetUp();
        cameraSmootSpeed = .5f;
    }
    private void Update()
    {
        PlayerMovementWASD();
        CameraFollow();
    }
    private void LateUpdate()
    {
        CameraSetUp();
        CoolDown();
    }


    //Esto es para el pequeño paneo del principio, me pareció que quedaba chulo
    void CoolDown()
    {
        cooldown += Time.deltaTime;

        if(cooldown >= finalTime)
        {
            Invoke("VelocidadCamara", 0f);
        }
    }

    //Esto es para el pequeño paneo del principio, me pareció que quedaba chulo
    void VelocidadCamara()
    {
        cameraSmootSpeed += Time.deltaTime;

        if(cameraSmootSpeed >= smoothSpeedMax)
        {
            cameraSmootSpeed = smoothSpeedMax;
        }
    }

    void CursosSetUp()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void PlayerMovementWASD()
    {
        float moveX = 0f;
        float moveZ = 0f;

        //pido perdón por tantos ifs y elses pero no encontraba otra forma de hacer las animaciones de movimiento

        if (Input.GetKey(KeyCode.W))
        {
            moveZ = +1f;
            playerAnim.SetFloat("Speed", 1f);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            playerAnim.SetFloat("Speed", 0f);
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveZ = -1f;
            playerAnim.SetFloat("Speed", 1f);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            playerAnim.SetFloat("Speed", 0f);
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
            playerAnim.SetFloat("Speed", 1f);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            playerAnim.SetFloat("Speed", 0f);
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveX = +1f;
            playerAnim.SetFloat("Speed", 1f);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            playerAnim.SetFloat("Speed", 0f);
        }

        

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
