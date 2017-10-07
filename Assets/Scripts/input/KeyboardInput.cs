using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : InputHandler {

	public void update(){
		//do nothing for this one.
	}

	public float getYaw() {
		return 0f;
	}

	public float getPitch() {
		float pitch = 0f;
		if(Input.GetKey("up")){
			pitch += 1f;
		}
		if(Input.GetKey("down")){
			pitch -= 1f;
		}
		return pitch;
	}

	public float getRoll() {
		float roll = 0f;
		if(Input.GetKey("left")){
			roll += 1f;
		}
		if(Input.GetKey("right")){
			roll -= 1f;
		}
		return roll;
	}
}
