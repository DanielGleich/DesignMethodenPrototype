using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform cam;
    [SerializeField] Transform playerObj;
    [SerializeField] GameObject reflector;
    [SerializeField] GameObject gameOverUI;
    CharacterController cc;

    [Header("Health")]
    [SerializeField] int hp = 3;
    [SerializeField] List<Material> hpMaterials;

    [Header("Gravity")]
    [SerializeField] float gravity;
    [SerializeField] float groundedGravity;

    [Header("Move-Parameter")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSmoothTime = 0.1f;

    [Header("Jump-Parameter")]
    [SerializeField] bool isJumpPressed;
    [SerializeField] float initialJumpVelocity;
    [SerializeField] float maxJumpHeight;
    [SerializeField] float maxJumpTime;
    [SerializeField] bool isJumping;
    
    [Header("Reflect-Parameter")]
    [SerializeField] bool isReflecting = false;
    [SerializeField] float reflectDuration;
    [SerializeField] float reflectCooldown;

    float inputX;
    float inputZ;
    Vector3 jumpVector;
    float targetAngle;
    float turnSmoothVelocity;

    IEnumerator LoadCheckState() {
        yield return null;
        hp = CheckpointState.playerHp;
    }

    void Start()
    {
        cc = GetComponent<CharacterController>();
        StartCoroutine(LoadCheckState());
        cam = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cc.providesContacts = true;
        SetupJumpVariables();
    }

    // Update is called once per frame
    void Update()
    {
        targetAngle = cam.eulerAngles.y;
        GetInput();
        RotatePlayer();
        MovePlayer();
        HandleGravity();
    }

    void GetInput() {
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");
        isJumpPressed = Input.GetKey(KeyCode.Space);

        if (Input.GetMouseButtonDown(0) && !isReflecting) StartCoroutine(Reflect());
    }
    public void RotatePlayer()
    {
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    void MovePlayer()
    {
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");

        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();
        Vector3 desiredMoveDirection = forward * inputZ + right * inputX + jumpVector;

        cc.Move(desiredMoveDirection * Time.deltaTime * moveSpeed);
        handleJump();
    }

    void HandleGravity() {
        bool isFalling = cc.velocity.y <= 0.0f;
        float fallMultiplier = 2.0f;
        if (cc.isGrounded) {
            jumpVector.y = groundedGravity;
        }
        else if (isFalling)
        {
            float previousYVelocity = jumpVector.y;
            float newYVelocity = jumpVector.y + (gravity * fallMultiplier * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * .5f;
            jumpVector.y = nextYVelocity;
        }
        else
        {
            float previousYVelocity = jumpVector.y;
            float newYVelocity = jumpVector.y + (gravity * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * .5f;
            jumpVector.y = nextYVelocity;
        }
    }


    public void TakeDamage()
    {
        hp--;
        if (hp == 0) {
            Die();
        } 
        else playerObj.GetComponent<MeshRenderer>().material = hpMaterials[hp];
    }

    void SetupJumpVariables() {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        //initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    void handleJump() {
        if (!isJumping && cc.isGrounded && isJumpPressed)
        {
            isJumping = true;
            jumpVector.y = initialJumpVelocity;
            cc.Move(jumpVector);

        }
        else if (!isJumpPressed && isJumping && cc.isGrounded) { 
            isJumping= false;
        }
    }

    IEnumerator Reflect() { 
        isReflecting = true;
        reflector.SetActive(true);
        yield return new WaitForSeconds(reflectDuration);
        reflector.SetActive(false);
        yield return new WaitForSeconds(reflectCooldown);
        isReflecting = false;
    }

    public void SaveHP() {
        CheckpointState.playerHp = hp;
    }

    public void Die()
    {
        CheckpointState.playerHp = 3;
        gameOverUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameObject.SetActive(false);
    }
}
