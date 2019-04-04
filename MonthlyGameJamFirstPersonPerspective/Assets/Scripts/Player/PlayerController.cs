using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed = 4f;
    public float sprintSpeedMultiplier = 2f;
	public float jumpVelocity = 5f;
    [Space]
	public Transform floorDetector;

	private Rigidbody rb;
	private HeadBobber hb;

	void Awake() {
		Cursor.lockState = CursorLockMode.Locked;
		rb = GetComponent<Rigidbody>();
		hb = GetComponentInChildren<HeadBobber>();
	}

	void Update() {
		if (IsGrounded())
			hb.enabled = true;
		else
			hb.enabled = false;
	}

	void FixedUpdate() {
        HandleMovement();

        // Hardcoding just for dev purposes calm down
        if (Input.GetKeyDown("escape"))
            Cursor.lockState = CursorLockMode.None;

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

}
