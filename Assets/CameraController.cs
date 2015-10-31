using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}

	// Blend
	void Update () {
		Vector3 position = transform.position;
		position.x = SteeringModel.cameraX;
		transform.position = position;
	}
}
