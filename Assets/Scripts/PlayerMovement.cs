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
    Camera camera;


    [Header("Move-Parameter")]
    [SerializeField] private float moveAcceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float turnSmoothTime = 0.1f;
    private Vector3 vel;
    private float turnSmoothVelocity;
    [Header("Jump-Parameter")]
    [SerializeField] bool isGrounded = true;
    [SerializeField] float jumpHeight;
    [SerializeField] float airMovMultiplier;

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
        Debug.Log(rb.velocity);
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

        if (Input.GetKeyDown(KeyCode.Space)) { 
            if (isGrounded)
            {
                rb.AddForce(rb.transform.up * jumpHeight, ForceMode.Impulse);
                isGrounded = false;
            }
        }

        moveDirection = new Vector3(axisX, 0, axisZ).normalized;

        Vector3 viewDir = player.position - new Vector3(camera.transform.position.x, player.position.y, camera.transform.position.z);
        orientation.forward = viewDir.normalized;

        Vector3 inputDir = orientation.forward * axisZ + orientation.right * axisX;
        if (inputDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }

        Vector3 direction = Camera.main.transform.position - player.position;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        playerObj.rotation = Quaternion.Slerp(player.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        moveDirection = Quaternion.AngleAxis(camera.transform.eulerAngles.y, Vector3.up) * moveDirection;

        Vector3 mov = moveDirection * moveAcceleration * Time.deltaTime;
        if (!isGrounded) mov = mov * airMovMultiplier;
        rb.AddForce(mov, ForceMode.Force);

        // Beschleunigung runterdämpfen, wenn keine Tasten gedrückt
        if (!Input.GetKey("w") && !Input.GetKey("s"))
        {
            rb.velocity = Vector3.SmoothDamp(rb.velocity, new Vector3(rb.velocity.x, rb.velocity.y, 0), ref vel, .2f);
        }

        if (!Input.GetKey("a") && !Input.GetKey("d"))
        {
            rb.velocity = Vector3.SmoothDamp(rb.velocity, new Vector3(0, rb.velocity.y, rb.velocity.z), ref vel, .2f);
        }

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    public void SetGrounded(bool grounded)
    {
        isGrounded = grounded;
    }
}
