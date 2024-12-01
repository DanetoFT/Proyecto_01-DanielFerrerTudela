using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    public float moveSpeed;
    float sprintSpeed = 30f;
    public float jumpForce = 10f;
    bool isMoving = false;
    public float mouseSensibility;
    public Transform cameraTransform;
    public Animator playerAnim;
    Rigidbody playerRb;
    public Transform respawn;
    bool isOnGround = true;

    [Header("Camera Settings")]
    public float cameraHeight;
    public float cameraDistance;
    [SerializeField] float cameraSmootSpeed;
    public float smoothSpeedMax;
    float cooldown = 0f;
    float finalTime = 3f;
    float cameraPitch; //Control vertical de la inclinacion de la camara

    [Header("Coleccionables")]
    public TextMeshProUGUI[] textos;
    public Animator[] iconAnimators;
    int gema1 = 0;
    int gema2 = 0;
    int gema3 = 0;
    int gema4 = 0;


    private void Start()
    {
        textos[0].text = gema1.ToString();
        textos[1].text = gema2.ToString();
        textos[2].text = gema3.ToString();
        textos[3].text = gema4.ToString();
        playerRb = GetComponent<Rigidbody>();
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

    //Volver de la animacion de salto
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Ground")
        {
            isOnGround = true;
            playerAnim.SetBool("Jump", false);
        }
    }

    //Recoger diferentes gemas con diferentes valores
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Gema1")
        {
            Destroy(other.gameObject);
            gema1++;
            textos[0].text = gema1.ToString();
            textos[0].gameObject.SetActive(true);
            iconAnimators[0].SetTrigger("Pick");
        }

        if(other.gameObject.tag == "Gema2")
        {
            Destroy(other.gameObject);
            gema2 += 2;
            textos[1].text = gema2.ToString();
            textos[1].gameObject.SetActive(true);
            iconAnimators[1].SetTrigger("Pick");
        }

        if (other.gameObject.tag == "Gema3")
        {
            Destroy(other.gameObject);
            gema3 += 3;
            textos[2].text = gema3.ToString();
            textos[2].gameObject.SetActive(true);
            iconAnimators[2].SetTrigger("Pick");
        }

        if (other.gameObject.tag == "Gema4")
        {
            Destroy(other.gameObject);
            gema4 += 4;
            textos[3].text = gema4.ToString();
            textos[3].gameObject.SetActive(true);
            iconAnimators[3].SetTrigger("Pick");
        }
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
            isMoving = true;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            playerAnim.SetFloat("Speed", 0f);
            isMoving = false;
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveZ = -1f;
            playerAnim.SetFloat("Speed", 1f);
            isMoving = true;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            playerAnim.SetFloat("Speed", 0f);
            isMoving = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
            playerAnim.SetFloat("Speed", 1f);
            isMoving = true;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            playerAnim.SetFloat("Speed", 0f);
            isMoving = false;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveX = +1f;
            playerAnim.SetFloat("Speed", 1f);
            isMoving = true;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            playerAnim.SetFloat("Speed", 0f);
            isMoving = false;
        }

        //salto
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            playerRb.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);
            playerAnim.SetBool("Jump", true);
            isOnGround = false;
        }

        //Sprint (al sprintar el jugador atraviesa los colliders, he comprobado que es por ir demasiado rápido)
        if (Input.GetKeyDown(KeyCode.LeftShift) && isMoving == true)
        {
            moveSpeed = sprintSpeed;
            playerAnim.SetBool("Run", true);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = 10f;
            playerAnim.SetBool("Run", false);
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
