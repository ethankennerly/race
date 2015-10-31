using UnityEngine;
using System.Collections;

public class VehicleController : MonoBehaviour {

	public GameObject competitorPrefab;
	public GameObject[] competitors;
	private RaceModel model;

	void Start () {
		model = RaceModel.getInstance();
		competitors = new GameObject[model.competitorCount];
		for (int c = 0; c < model.competitorCount; c++) {
			Vector3 position = new Vector3(model.lanes[c], 0.25f, model.competitors[c].z);
			competitors[c] = (GameObject) Instantiate(
				competitorPrefab, position, Quaternion.identity);
			competitors[c].SetActive(true);
			// Debug.Log("VehicleController.Start: " + c + " position " + competitors[c].transform.position);
		}
	}

	/**
	 * Array road segment quads.  While behind camera, recycle forward.
	 */
	void FixedUpdate () {
		for (int c = 0; c < model.competitors.Length; c++) {
			SpeedModel competitor = model.competitors[c];
			GameObject gameObject = competitors[c];
			Vector3 position = gameObject.transform.position;
			position.z = competitor.z;
			gameObject.transform.position = position;
		}
	}
}
