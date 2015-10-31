using UnityEngine;  // Mathf

public class RaceModel {
	public static RaceModel instance;

	public SpeedModel speed = new SpeedModel();
	public SteeringModel steering = new SteeringModel();
	public SpeedModel[] competitors;
	public int competitorCount = 10;  
					// 1;
					// 2;
					// 0;
	public float[] lanes;

	public static RaceModel getInstance() {
		if (null == instance) {
			instance = new RaceModel();
			instance.Start();
		}
		return instance;
	}

	public void Start () {
		competitors = new SpeedModel[competitorCount];
		lanes = new float[competitorCount];
		for (int c = 0; c < competitors.Length; c++) {
			competitors[c] = new SpeedModel();
			SpeedModel competitor = competitors[c];
			float cf = (float) c;
			competitor.idealSpeed = 5.0f + 10.0f * cf;
			competitor.targetSpeed = 5.0f + 1.0f * cf;
			competitor.z = 1.0f + cf;
			lanes[c] = Mathf.Floor(Random.value * 3.0f) - 1.0f;
		}
	}
	
	public void Update (float deltaTime) {
		steering.Update(deltaTime);
		speed.Update(deltaTime);
		for (int c = 0; c < competitors.Length; c++) {
			SpeedModel competitor = competitors[c];
			competitor.Update(deltaTime);
		}
	}
}
