using UnityEngine;  // Mathf

public class RaceModel {
	public static RaceModel instance;

	public SpeedModel speed = new SpeedModel();
	public SteeringModel steering = new SteeringModel();
	public SpeedModel[] competitors;
	public int competitorCount = 	19;
					// 100;
					// 10;  
					// 1;
					// 2;
					// 0;
	public float[] lanes;
	public int playerRank;

	private float time;
	private float passInterval;

	public static RaceModel getInstance() {
		if (null == instance) {
			instance = new RaceModel();
			instance.Start();
		}
		return instance;
	}

	/**
	 * Sort competitors from slowest to fastest.
	 */
	public void Start () {
		competitors = new SpeedModel[competitorCount];
		lanes = new float[competitorCount];
		float idealPerCompetitor = 20.0f;  // 100.0f;
		idealPerCompetitor /= ((float) competitorCount);
		float targetPerCompetitor = 10.0f;
		targetPerCompetitor /= ((float) competitorCount);
		for (int c = 0; c < competitors.Length; c++) {
			competitors[c] = new SpeedModel();
			SpeedModel competitor = competitors[c];
			float cf = (float) competitors.Length - c - 1;
			competitor.idealSpeed = 5.0f + idealPerCompetitor * cf;
			competitor.targetSpeed = 5.0f + targetPerCompetitor * cf;
			competitor.z = 5.0f + cf;
			lanes[c] = Mathf.Floor(Random.value * 3.0f) - 1.0f;
		}
		playerRank = competitorCount + 1;
		time = 0;
		passInterval = 0;
	}

	/**
	 * @return	Promoted or demoted rank.
	 * @param	competitors	Expects sorted by z.
	 */
	private int UpdatePlayerRank(int playerRank, float playerZ, SpeedModel[] competitors) {
		SpeedModel competitor;
		int rank = playerRank;
		if (2 <= rank && null != competitors) {
			competitor = competitors[rank - 2];
			while (2 <= rank && competitor.z < playerZ) {
				passInterval = time - passInterval;
				Debug.Log("RaceModel.UpdatePlayerRank: " + rank + " interval " + passInterval + " playerZ " + playerZ + " competitor.z " + competitor.z);
				passInterval = time;
				rank--;
				if (2 <= rank) 
					competitor = competitors[rank - 2];
			}
		}
		if (null != competitors && rank <= competitors.Length) {
			competitor = competitors[rank - 1];
			while (playerZ < competitor.z && rank <= competitors.Length) {
				rank++;
				if (rank <= competitors.Length)
					competitor = competitors[rank - 1];
			}
		}
		return rank;
	}

	public void Update (float deltaTime) {
		time += deltaTime;
		steering.Update(deltaTime);
		speed.Update(deltaTime);
		for (int c = 0; c < competitors.Length; c++) {
			SpeedModel competitor = competitors[c];
			competitor.Update(deltaTime);
		}
		playerRank = UpdatePlayerRank(playerRank, speed.z, competitors);
	}
}
