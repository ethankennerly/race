using UnityEngine;  // Mathf;

public class SteeringModel {
	public static float laneStep = 1.0f;
	public static float laneLeft = -1.0f;
	public static float laneRight = 1.0f;

	public float cameraX = 0.0f;
	public float cameraXMultiplier = 0.75f;
	public bool isChanging = false;
	public bool isInputLeft = false;
	public bool isInputRight = false;
	private bool wasInputLeft = false;
	private bool wasInputRight = false;
	public float speed = 5.0f;
	public float x = 0.0f;

	private float laneTarget = 0.0f;
	private float xDifference = 0.0f;
	private float tolerance = 0.001f;

	// Use this for initialization
	public void Start () {
	
	}

	/**
	 * If was input left or right then ignore input this frame.  Perhaps multiple updates are being called per frame, since it is a fixed update.  Test case:  2015-10-31 In left lane.  Press right.  Expect move one lane.  Sometimes move two lanes.
	 */
	public float Update (float deltaTime) {
		if (isInputLeft && isInputRight) {
		}
		else if (isInputLeft && !wasInputLeft) {
			Debug.Log("SteeringModel.update: Left");
			laneTarget -= laneStep;
			isChanging = true;
		}
		else if (isInputRight && !wasInputRight) {
			Debug.Log("SteeringModel.update: Right");
			laneTarget += laneStep;
			isChanging = true;
		}
		if (isChanging) {
			laneTarget = Mathf.Max(laneLeft, Mathf.Min(laneRight, laneTarget));
			xDifference = laneTarget - x;
			x += xDifference * deltaTime * speed;
			if (Mathf.Abs(xDifference) <= tolerance 
			|| (xDifference < 0 && x < laneTarget)
			|| (0 < xDifference && laneTarget < x)) {
				x = laneTarget;
				isChanging = false;
			}
		}
		cameraX = x * cameraXMultiplier;
		wasInputLeft = isInputLeft;
		wasInputRight = isInputRight;
		return x;
	}
}
