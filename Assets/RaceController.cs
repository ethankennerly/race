using UnityEngine;  // Mathf

public class RaceController {
	private static RaceController instance;
	private RaceModel model;

	public static RaceController getInstance() {
		if (null == instance) {
			instance = new RaceController();
			instance.Start();
		}
		return instance;
	}

	public void Start () {
		model = RaceModel.getInstance();
	}

	/**
	 * http://answers.unity3d.com/questions/46107/in-what-order-are-all-the-update-functions-called.html
	 */
	public void Update () {
		model.Update(Time.deltaTime);
	}
}
