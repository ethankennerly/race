using UnityEngine;  // Mathf;

public class SteeringModel {
	public static float laneStep = 1.0f;
	public static float laneLeft = -1.0f;
	public static float laneRight = 1.0f;

	public bool isChanging = false;
	public bool isInputLeft = false;
	public bool isInputRight = false;
	public float speed = 5.0f;
	public float x = 0.0f;

	private float laneTarget = 0.0f;
	private float xDifference = 0.0f;
	private float tolerance = 0.001f;

	// Use this for initialization
	public void Start () {
	
	}
	
	// Update is called once per frame
	public void Update (float deltaTime) {
		if (isInputLeft && isInputRight) {
		}
		else if (isInputLeft) {
			// Debug.Log("SteeringModel.update: Left");
			laneTarget -= laneStep;
			isChanging = true;
		}
		else if (isInputRight) {
			// Debug.Log("SteeringModel.update: Right");
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
	}
}
