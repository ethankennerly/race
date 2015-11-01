using UnityEngine;  // Mathf

public class RaceModel {
	public static RaceModel instance;

	public SpeedModel speed = new SpeedModel();
	public SteeringModel steering = new SteeringModel();
	public SpeedModel[] competitors;
	public int competitorCount = 	// 19;
					49;
					// 100;
					// 10;  
					// 1;
					// 2;
					// 0;
	public float[] lanes;
	public int playerRank;
	public string playerRankText;

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
		competitors = SpeedModel.constructCompetitors(competitorCount);
		lanes = new float[competitorCount];
		for (int c = 0; c < competitors.Length; c++) {
			lanes[c] = Mathf.Floor(Random.value * 3.0f) - 1.0f;
		}
		playerRank = competitorCount + 1;
		playerRankText = formatPlayerRankText(playerRank);
		time = 0;
		passInterval = 0;
	}

	/**
	 * For another language, format could be extended and elements translated.
	 * https://msdn.microsoft.com/en-us/library/system.string.format(v=vs.110).aspx
	 */
	public string formatPlayerRankText(int rank) {
		string cardinal;
		int lastDigit = rank % 10;
		if (1 == lastDigit) {
			cardinal = "st";
		}
		else if (2 == lastDigit) {
			cardinal = "nd";
		}
		else if (3 == lastDigit) {
			cardinal = "rd";
		}
		else {
			cardinal = "th";
		}
		return string.Format("{0}{1}\nplace", rank, cardinal);
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
		if (rank != playerRank) {
			playerRankText = formatPlayerRankText(rank);
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
