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
    CharacterController cc;

    [Header("Health")]
    [SerializeField] int hp = 3;
    [SerializeField] List<Material> hpMaterials;

    [Header("Gravity")]
    [SerializeField] float velocity;
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
    Vector3 moveVector;

    float targetAngle;

    float turnSmoothVelocity;
    float verticalVel;
    IEnumerator LoadCheckState() {
        yield return null;
        CheckPoint cp = GameObject.FindGameObjectWithTag("CheckpointManager").GetComponent<CheckpointManager>().currentState;
        transform.position = cp.PlayerSpawnPoint.position;
        transform.eulerAngles = cp.PlayerSpawnPoint.eulerAngles;
    }

    void Start()
    {
        cc = GetComponent<CharacterController>();
        StartCoroutine(LoadCheckState());
        cam = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        setupJumpVariables();
    }

    // Update is called once per frame
    void Update()
    {
        isJumpPressed = Input.GetKey(KeyCode.Space);
        targetAngle = cam.eulerAngles.y;
        GetInput();
        RotatePlayer();
        MovePlayer();

        if (cc.isGrounded)
            verticalVel -= 0;
        else
            verticalVel -= 1;

        moveVector = new Vector3(0, verticalVel * .2f * Time.deltaTime, 0);
        cc.Move(moveVector);

        handleGravity();
    }

    void GetInput() {
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");

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
        Vector3 desiredMoveDirection = forward * inputZ + right * inputX;

        cc.Move(desiredMoveDirection * Time.deltaTime * moveSpeed);
        handleGravity();
        handleJump();
    }

        void handleGravity() {
        if (cc.isGrounded)
            moveVector.y = groundedGravity;
        else
            moveVector.y = gravity;
    }


    public void TakeDamage()
    {
        hp--;
        if (hp == 0) gameObject.SetActive(false);
        else playerObj.GetComponent<MeshRenderer>().material = hpMaterials[hp];
    }

    void setupJumpVariables() {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    void handleJump() {
        Debug.Log(cc.isGrounded);
        if (!isJumping && cc.isGrounded && isJumpPressed) {
            isJumping = true;
            moveVector.y = initialJumpVelocity;
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

}
