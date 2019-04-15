using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerController : MonoBehaviour {

    public float maxSpeed = 4f;
    private float speed;
    public float sprintSpeedMultiplier = 2f;
    public float crouchSpeedMultiplier = 0.5f;
    public float crouchSmooth = 1.0f;
    public float jumpVelocity = 5f;

    [Space]
    [Header("References")]
    public Transform floorDetector;
    public Transform itemHolder;
    public Camera cam;

    private Rigidbody rb;
    private GameObject heldItem;
    private CapsuleCollider cc;
    private Animator camAnim;
    private bool canJump = true;

    void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
        cc = GetComponent<CapsuleCollider>();
        camAnim = cam.GetComponent<Animator>();
        speed = maxSpeed;
    }

    void Update() {
        Crouch();
    }

    void FixedUpdate() {
        HandleMovement();
        HandleLook();
        // Hardcoding just for dev purposes calm down
        if (Input.GetKeyDown("escape"))
            Cursor.lockState = CursorLockMode.None;

    }

    private void HandleLook() {
        float y = Input.GetAxisRaw("Mouse X");
        float x = -Input.GetAxisRaw("Mouse Y");
        transform.Rotate(0, y, 0);
        cam.transform.Rotate(x, 0, 0);
        cam.transform.localRotation = ClampRotationAroundXAxis(cam.transform.localRotation);
    }

    private void HandleMovement() {
        float forward;
        float strafe;


        if (Input.GetButton("Sprint")) {
            forward = Input.GetAxisRaw("Horizontal") * speed * sprintSpeedMultiplier;
            strafe = Input.GetAxisRaw("Vertical") * speed * sprintSpeedMultiplier;
        }
        else {
            forward = Input.GetAxisRaw("Horizontal") * speed;
            strafe = Input.GetAxisRaw("Vertical") * speed;
        }

        Vector3 move = new Vector3(forward, rb.velocity.y, strafe);
        rb.velocity = transform.TransformDirection(move);
        camAnim.SetFloat("Speed", Mathf.Max(maxSpeed * 0.9f, Mathf.Abs(forward), Mathf.Abs(strafe)));

        if (IsGrounded()) {
            // For head bobbing animation. Only plays when player is on the ground
            camAnim.SetBool("isGrounded", true);

            if (Input.GetAxisRaw("Jump") > 0 && canJump) {
                rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);
            }
        } else {
            camAnim.SetBool("isGrounded", false);
        }

    }

    private void Crouch() {
        if (Input.GetButtonDown("Crouch")) {
            canJump = false;
            cc.height = 1;
            cc.center = new Vector3(0, -0.5f, 0);
            camAnim.SetTrigger("Crouch");
            speed = maxSpeed * crouchSpeedMultiplier;
        }
        if (Input.GetButtonUp("Crouch")) {
            canJump = true;
            cc.height = 2;
            cc.center = Vector3.zero;
            camAnim.SetTrigger("Uncrouch");
            speed = maxSpeed;
        }
    }

    // Raycast down to see if player is standing on a collider
    public bool IsGrounded() {
        if (Physics.Raycast(floorDetector.position, Vector3.down, 0.05f))
            return true;
        else
            return false;
    }


    // Don't ask me how this works it just keeps the camera from rotationg 360 degrees on the x-axis
    Quaternion ClampRotationAroundXAxis(Quaternion q) {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, -90F, 90F);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

}


[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor {

    public override void OnInspectorGUI() {
        DrawDefaultInspector();
    }

    private void OnSceneGUI() {
        PlayerController player = (PlayerController)target;

        // Draw a line in scene view to represent pickup distance and direction
        Handles.color = Color.red;
        // Handles.DrawLine(player.camera.transform.position, player.camera.transform.position + new Vector3(0, 0, player.pickupDistance));
    }

}