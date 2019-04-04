using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobber : MonoBehaviour {

	private float timer = 0.0f;
	public bool headBob = true;
	public float bobbingSpeed = 0.18f;
	public float bobbingAmount = 0.2f;
	public float midpoint = 2.0f;
	[HideInInspector]
	public float speedMult = 1;

	void Update() {
		if (!headBob)
			return;
		
		float waveslice = 0.0f;
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		Vector3 cSharpConversion = transform.localPosition; 

		if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0) {
			timer = 0.0f;
		} else {
			waveslice = Mathf.Sin(timer);
			timer = timer + bobbingSpeed * speedMult;
			if (timer > Mathf.PI * 2) {
				timer = timer - (Mathf.PI * 2);
			}
		}
		if (waveslice != 0) {
			float translateChange = waveslice * bobbingAmount;
			float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
			totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
			translateChange = totalAxes * translateChange;
			cSharpConversion.y = midpoint + translateChange;
		} else {
			cSharpConversion.y = midpoint;
		}


        // If this is true, then we are at the bottom of the bob. Use for footstep sounds
        // if (waveslice < -0.9f)
        
		transform.localPosition = cSharpConversion;
	}
}