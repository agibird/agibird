using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InputHandler{

	void update();
	float getYaw();
	float getPitch();
	float getRoll();
}
