using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	private RaceModel model;

	void Start () {
		model = RaceModel.getInstance();
		model.speed.cameraZStart = transform.position.z;
	}

	// Follow player
	void FixedUpdate () {
		Vector3 position = transform.position;
		position.x = model.steering.cameraX;
		position.z = model.speed.cameraZ;
		transform.position = position;
	}
}
