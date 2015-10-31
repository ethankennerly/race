using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;
	private RaceModel model;

	// Use this for initialization
	void Start () {
		model = RaceModel.getInstance();
	}

	/**
	 * FixedUpdate instead of Update.  Test case:  2015-10-31 Expect player accelerates smoothly.  Got small jitter.
	 * Listen to input.  Update model.  Get position from steering and speed.
	 *
	 * http://answers.unity3d.com/questions/27369/how-to-move-an-object-from-point-a-to-point-b-with.html
	 * http://docs.unity3d.com/ScriptReference/Input.GetKeyDown.html
	 */
	void FixedUpdate () {
		model.steering.isInputLeft = Input.GetKeyDown(KeyCode.LeftArrow);
		model.steering.isInputRight = Input.GetKeyDown(KeyCode.RightArrow);
		model.Update(Time.deltaTime);
		Vector3 position = transform.position;
		position.x = model.steering.x;
		position.z = model.speed.z;
		transform.position = position;
		speed = model.speed.speed;
	}
}
