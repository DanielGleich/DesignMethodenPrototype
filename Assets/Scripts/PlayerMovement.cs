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

    [Header("Move-Parameter")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSmoothTime = 0.1f;

    [Header("Jump-Parameter")]
    [SerializeField] float jumpHeight;
    [SerializeField] float jumpCooldown;
    [SerializeField] float gravityMultiplier;
    bool isJumpCooldown = false;
    
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
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    void GetInput() {
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && cc.isGrounded && !isJumpCooldown) StartCoroutine(Jump());
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
    }

    public void TakeDamage()
    {
        hp--;
        if (hp == 0) gameObject.SetActive(false);
        else playerObj.GetComponent<MeshRenderer>().material = hpMaterials[hp];
    }

    IEnumerator Jump()
    {
        isJumpCooldown = true;
        cc.Move(transform.up * jumpHeight);
        yield return new WaitForSeconds(jumpCooldown);
        isJumpCooldown = false;
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
