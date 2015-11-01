using UnityEngine;

/**
 * Read which place at end.
 * http://wiki.unity3d.com/index.php?title=3DText
 */
public class RaceController : MonoBehaviour {
	public GameObject competitorPrefab;
	public bool isVerbose;
	public int playerRank;
	public float playerSpeed;

	private RaceModel model;
	private GameObject finish;
	private GameObject player;
	private GameObject playerCamera;
	private GameObject[] competitors;

	public void Start () {
		model = RaceModel.getInstance();
		player = GameObject.Find("Player");
		playerCamera = GameObject.Find("Camera");
		model.speed.cameraZStart = playerCamera.transform.position.z;
		competitors = ConstructCompetitors(model, competitorPrefab);
		finish = GameObject.Find("Finish");
		finish.transform.position += Vector3.forward * model.finishZ;
	}

	public GameObject[] ConstructCompetitors (RaceModel model, GameObject competitorPrefab) {
		GameObject[] competitors = new GameObject[model.competitorCount];
		for (int c = 0; c < model.competitorCount; c++) {
			Vector3 position = new Vector3(model.lanes[c], 0.25f, model.competitors[c].z);
			competitors[c] = (GameObject) Instantiate(
				competitorPrefab, position, Quaternion.identity);
			competitors[c].SetActive(true);
			// Debug.Log("RaceController.Construct: " + c + " position " + competitors[c].transform.position);
		}
		return competitors;
	}

	/**
	 * Listen to input.  Update model.  Get position from steering and speed.
	 *
	 * http://answers.unity3d.com/questions/27369/how-to-move-an-object-from-point-a-to-point-b-with.html
	 * http://docs.unity3d.com/ScriptReference/Input.GetKeyDown.html
	 */
	private void UpdateInput(SteeringModel steering) {
		steering.isInputLeft = Input.GetKeyDown(KeyCode.LeftArrow);
		steering.isInputRight = Input.GetKeyDown(KeyCode.RightArrow);
		steering.isVerbose = isVerbose;
		if (isVerbose) {
			if (model.steering.isInputLeft) {
				Debug.Log("PlayerController.Update: Left");
			}
			if (model.steering.isInputRight) {
				Debug.Log("PlayerController.Update: Right");
			}
		}
	}

	private void SetPosition (Transform transform, float x, float z) {
		Vector3 position = transform.position;
		position.x = x;
		position.z = z;
		transform.position = position;
	}

	public void SetCompetitorPosition (SpeedModel[] competitorSpeeds) {
		if (null == competitorSpeeds) {
			return;
		}
		for (int c = 0; c < competitorSpeeds.Length; c++) {
			Transform transform = competitors[c].transform;
			SetPosition (transform, transform.position.x, 
				competitorSpeeds[c].z);
		}
	}

	/**
	 * Controller explicitly orders updates:  All models then all views.
	 * Was updating on each prefab's component script.  Also tried fixed update.
	 * It turns out the order of updates is arbitrary so one view can update before the model and another afterward.
	 * http://answers.unity3d.com/questions/46107/in-what-order-are-all-the-update-functions-called.html
	 * Test case:  2015-11-01 Expect camera smoothly follows player and each keyboard input is processed exactly once.  Got player position jitters or keyboard input not processed every time or processed twice.
	 */
	public void Update () {
		UpdateInput(model.steering);
		model.Update(Time.deltaTime);
		playerRank = model.playerRank;
		playerSpeed = model.speed.speed;
		SetCompetitorPosition(model.competitors);
		SetPosition(player.transform, model.steering.x, model.speed.z);
		SetPosition(playerCamera.transform, model.steering.cameraX, model.speed.cameraZ);
	}
}
