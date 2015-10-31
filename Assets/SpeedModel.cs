using UnityEngine;  // Mathf;

public class SpeedModel {
	public float cameraZ = 0.0f;
	public float cameraZStart = 0.0f;
	public float idealSpeed = 200.0f;
	public float speed = 0.0f;
	public float targetSpeed = 5.0f;
	public float z = 0.0f;

	private float convergeRate = 0.25f;
	private float idealConvergeRate = 0.0025f;

	public void Start () {
	}
	
	public float Update (float deltaTime) {
		targetSpeed += (idealSpeed - targetSpeed) * deltaTime * idealConvergeRate;
		speed += (targetSpeed - speed) * deltaTime * convergeRate;
		z += speed * deltaTime;
		cameraZ = z + cameraZStart;
		return z;
	}
}
