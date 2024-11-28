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

    [Header("Coleccionables")]
    public TextMeshProUGUI[] gemas;
    public GameObject aguaCollider;

    float cameraPitch; //Control vertical de la inclinacion de la camara

    private void Start()
    {
        
    }
    private void Update()
    {
        PlayerMovementWASD();
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
}
