using UnityEngine;

/**
 * Read which place at end.
 * http://wiki.unity3d.com/index.php?title=3DText
 */
public class RaceController : MonoBehaviour {
	public GameObject competitorPrefab;
	public bool isVerbose;
	public bool isShort;
	public int playerRank;
	public float playerSpeed;

	private RaceModel model = new RaceModel();
	private GameObject finish;
	private TextMesh finishText;
	private GameObject restart;
	private GameObject player;
	private RoadController road;
	private GameObject playerCamera;
	private GameObject[] competitors;

	public void Start () {
		player = GameObject.Find("Player");
		playerCamera = GameObject.Find("Camera");
		SpeedModel.player.setup(player.transform.position.z, playerCamera.transform.position.z);
		SpeedModel.setIsShort(isShort);
		model.Start();
		road = GameObject.Find("Road").GetComponent<RoadController>();
		road.model = model;
		ConstructCompetitors(model, competitorPrefab);
		finish = GameObject.Find("Finish"); 
		finish.transform.position = Vector3.forward * SpeedModel.finishZ;
		finishText = (TextMesh) GameObject.Find("FinishText").GetComponent<TextMesh>();
		restart = GameObject.Find("RestartText");
		restart.SetActive(SpeedModel.isRestartEnabled());
	}

	public GameObject[] ConstructCompetitors (RaceModel model, GameObject competitorPrefab) {
		int c;
		if (null != competitors) {
			for (c = 0; c < competitors.Length; c++) {
				Destroy(competitors[c]);
			}
		}
		competitors = new GameObject[SpeedModel.competitorCount];
		for (c = 0; c < SpeedModel.competitorCount; c++) {
			Vector3 position = new Vector3(model.lanes[c], 0.25f, SpeedModel.competitors[c].z);
			competitors[c] = (GameObject) Instantiate(
				competitorPrefab, position, Quaternion.identity);
			competitors[c].SetActive(true);
			if (isVerbose) Debug.Log("RaceController.Construct: " + c + " position " + competitors[c].transform.position);
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

	private void SetRankText(TextMesh mesh) {
		mesh.text = model.playerRankText;
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
		playerSpeed = SpeedModel.player.speed;
		SetCompetitorPosition(SpeedModel.competitors);
		SetRankText(finishText);
		SetPosition(player.transform, model.steering.x, SpeedModel.player.z);
		SetPosition(playerCamera.transform, model.steering.cameraX, SpeedModel.player.cameraZ);
		restart.SetActive(SpeedModel.isRestartEnabled());
		if (SpeedModel.isRestart) {
			SpeedModel.isRestart = false;
			if (isVerbose) Debug.Log("RaceModel.Update: Restart");
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
