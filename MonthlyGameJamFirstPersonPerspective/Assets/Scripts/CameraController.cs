using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject player;
    public float sensitivity = 5.0f;

	void Start () {
		player = transform.parent.gameObject;
	}
	

	void FixedUpdate () {
		float y = Input.GetAxisRaw("Mouse X");
		float x = -Input.GetAxisRaw("Mouse Y");
		player.transform.Rotate(0, y, 0);
		transform.Rotate(x, 0, 0);
	}
		
}
