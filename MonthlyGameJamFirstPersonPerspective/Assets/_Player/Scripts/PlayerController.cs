using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerController : MonoBehaviour {

    public float speed = 4f;
    public float sprintSpeedMultiplier = 2f;
    public float jumpVelocity = 5f;

    [Space]
    [Header("References")]
    public Transform floorDetector;
    public Transform itemHolder;
    public Camera camera;

    private Rigidbody rb;
    private HeadBobber hb;
    private GameObject heldItem;

    void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        hb = GetComponentInChildren<HeadBobber>();
        camera = GetComponentInChildren<Camera>();
    }

    void Update() {
        if (IsGrounded())
            hb.enabled = true;
        else
            hb.enabled = false;
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
        camera.transform.Rotate(x, 0, 0);
        camera.transform.localRotation = ClampRotationAroundXAxis(camera.transform.localRotation);
    }

    private void HandleMovement() {
        float forward;
        float strafe;

        if (Input.GetButton("Sprint")) {
            hb.speedMult = sprintSpeedMultiplier;
            forward = Input.GetAxisRaw("Horizontal") * speed * sprintSpeedMultiplier;
            strafe = Input.GetAxisRaw("Vertical") * speed * sprintSpeedMultiplier;
        }
        else {
            hb.speedMult = 1;
            forward = Input.GetAxisRaw("Horizontal") * speed;
            strafe = Input.GetAxisRaw("Vertical") * speed;
        }

        Vector3 move = new Vector3(forward, rb.velocity.y, strafe);
        rb.velocity = transform.TransformDirection(move);

        if (Input.GetAxisRaw("Jump") > 0) {
            if (IsGrounded()) {
                rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);
            }

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