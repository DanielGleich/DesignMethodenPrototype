using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform orientation;
    [SerializeField] Transform player;
    [SerializeField] Transform playerObj;
    [SerializeField] Rigidbody rb;
    //[SerializeField] CinemachineFreeLook camera;
    Camera camera;


    [Header("Parameters")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    // Start is called before the first frame update

    Vector3 moveDirection;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Vorne Hinten ; Vertical
        int axisZ = 0, axisX = 0;
        if (Input.GetKey("w"))
            axisZ = 1;
        else if (Input.GetKey("s"))
            axisZ = -1;

        // Links Rechts ; Horizontal
        if (Input.GetKey("a"))
            axisX = -1;
        else if (Input.GetKey("d"))
            axisX = 1;

        moveDirection = new Vector3(axisX, 0, axisZ).normalized;

        Vector3 viewDir = player.position - new Vector3(camera.transform.position.x, player.position.y, camera.transform.position.z);
        orientation.forward = viewDir.normalized;

        Vector3 inputDir = orientation.forward * axisZ + orientation.right * axisX;
        if (inputDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }

        Vector3 direction = Camera.main.transform.position - player.position;
        direction.y = 0; // Ignoriere die vertikale Komponente

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        //Interpoliere zwischen der aktuellen Rotation des Spielers und der Zielrotation
        playerObj.rotation = Quaternion.Slerp(player.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        moveDirection = Quaternion.AngleAxis(camera.transform.eulerAngles.y, Vector3.up) * moveDirection;
        Debug.Log(moveDirection);
        rb.AddForce(moveDirection * moveSpeed * Time.deltaTime, ForceMode.Force);

        SpeedControl();
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}
