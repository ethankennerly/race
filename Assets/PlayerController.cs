using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private SteeringModel steering = new SteeringModel();

	// Use this for initialization
	void Start () {
		steering.Start();
	}

	// http://docs.unity3d.com/ScriptReference/Input.GetKeyDown.html
	// http://answers.unity3d.com/questions/27369/how-to-move-an-object-from-point-a-to-point-b-with.html
	// Update is called once per frame
	void Update () {
		steering.isInputLeft = Input.GetKeyDown(KeyCode.LeftArrow);
		steering.isInputRight = Input.GetKeyDown(KeyCode.RightArrow);
		steering.Update(Time.deltaTime, true);
		Vector3 position = transform.position;
		position.x = steering.x;
		transform.position = position;
	}
}
