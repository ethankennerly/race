using UnityEngine;  // Mathf

/**
 * Lower ideal speed, raise ideal converge rate, lower competitor speeds.  Test case:  2015-11-01 Expect to intercept 49 competitors interpolated between 1.3 sec and 0.3 seconds, each number going down.  Previously trend was increasing.
 * 
 */
public class SpeedModel {

	public static SpeedModel[] competitors;
	public static SpeedModel player = new SpeedModel();
	public static int competitorCount = 	// 19;
					49;
					// 100;
					// 10;  
					// 1;
					// 2;
					// 0;
	public static float finishZ;
	public static int restartCount = 0;
	public static float NOT_SET = -10000.0f;
	public static bool isEnabled = true;
	public static bool isRestart = false;
	public static bool isShort;
	public static float postZInFrontOfSign = 15.0f;

	public static float convergeRateStart = 0.125f;
	public static float idealConvergeRateStart = 
						// 0.0025f;
						// 0.005f;
						0.01f;
						// 0.05f;
						// 0.1f;
						// 0.5f;

	public static int level = 0;
	public static int[] competitorCounts = {
		49,
		59,
		69,
		79,
		89,
		99
	};
	public static float[] idealSpeeds = {
		80.0f,
		90.0f,
		100.0f,
		110.0f,
		120.0f,
		130.0f
	};

	public static void setIsShort(bool isShort) {
		SpeedModel.isShort = isShort;
		if (isShort) {
			SpeedModel.finishZ = 20.0f;
		}
		else {
			SpeedModel.finishZ = 260.0f;
		}
	}

	public static void Start() {
		if (SpeedModel.isShort) {
			competitorCount = 9;
			player.convergeRate = 2.5f;
		}
		else {
			competitorCount = competitorCounts[level];
			player.convergeRate = SpeedModel.convergeRateStart;
		}
		player.idealConvergeRate = SpeedModel.idealConvergeRateStart;
		player.idealSpeed = idealSpeeds[level];
		player.targetSpeed = player.targetSpeedStart;
		player.speed = 0.0f;
		Debug.Log("SpeedModel.Start: " + competitorCount 
			+ " competitors. Ideal player speed: " 
			+ player.idealSpeed
			+ ", targetSpeed " + player.targetSpeed
			+ ", speed " + player.speed
		);
		SpeedModel.competitors = SpeedModel.constructCompetitors(competitorCount);
	}

	public static void Updates(float deltaTime) {
		player.Update(deltaTime);
		for (int c = 0; c < competitors.Length; c++) {
			SpeedModel competitor = competitors[c];
			competitor.Update(deltaTime);
		}
	}

	public static bool isRestartEnabled() {
		return SpeedModel.isEnabled 
			&& !player.IsActive() 
			&& player.speed < player.restart;
	}

	public static bool isRestarting() {
		isRestart = false;
		if (!SpeedModel.isEnabled) {
			SpeedModel.isEnabled = true;
		}
		else if (SpeedModel.isRestartEnabled()) {
			isRestart = true;
			restartCount++;
		}
		return isRestart;
	}

	public static int setNextLevel(int rank) {
		if (rank <= 1) {
			level++;
			int max = Mathf.Min(competitorCounts.Length, idealSpeeds.Length) - 1;
			if (max < level) {
				level = max;
			}
		}
		return level;
	}

	public float cameraZ = 0.0f;
	public float restart = 0.5f;
	public float cameraZStart = NOT_SET;
	public float idealSpeed = 	
					// 50.0f;
					80.0f;
					// 100.0f;
					// 200.0f;
					// 400.0f;
	public float postZ = 12.0f;
	public float speed = 0.0f;
	public float targetSpeed = 5.0f;
	public float targetSpeedStart = 5.0f;
	public float z = 0.0f;
	public float zStart = NOT_SET;
	public float convergeRate = convergeRateStart;
	public float idealConvergeRate = idealConvergeRateStart;

	public static SpeedModel[] constructCompetitors(int competitorCount)
	{
		SpeedModel[] competitors = new SpeedModel[competitorCount];
		float competitorCountf = (float) competitorCount;
		float idealPerCompetitor = 5.0f;
						// 10.0f;
						// 20.0f;  
						// 100.0f;
		idealPerCompetitor /= competitorCountf;
		float targetPerCompetitor = 5.0f;
						// 5.0f;
						// 10.0f;
		targetPerCompetitor /= competitorCountf;
		float firstCompetitorDistance = 5.0f;
						// 1.0f;
		for (int c = 0; c < competitors.Length; c++) {
			competitors[c] = new SpeedModel();
			SpeedModel competitor = competitors[c];
			float cf = (float) competitors.Length - c - 1;
			competitor.idealSpeed = 5.0f + idealPerCompetitor * cf;
			competitor.targetSpeed = 5.0f + targetPerCompetitor * cf;
			competitor.z = firstCompetitorDistance + cf;
			competitor.calculatePostZ(c + 1.5f);
		}
		return competitors;
	}

	public float calculatePostZ(float rank) {
		float perRank = 0.5f;
		postZ = postZInFrontOfSign - perRank * rank;
		return postZ;
	}

	public void setup(float zSetup, float cameraZSetup) {
		if (restartCount <= 0) {
			zStart = zSetup;
			cameraZStart = cameraZSetup;
		}
		z = zStart;
		cameraZ = cameraZStart;
	}

	/**
	 * After finish, slow down and stop in front of result billboard.
	 * Before start, do not move.
	 */
	public float Update (float deltaTime) {
		if (!isEnabled) {
			cameraZ = z + cameraZStart;
			return z;
		}
		if (!IsActive()) {
			idealSpeed = (finishZ + postZ - z);
			idealConvergeRate = 10.0f;
			convergeRate = 10.0f;
		}
		targetSpeed += (idealSpeed - targetSpeed) * deltaTime * idealConvergeRate;
		speed += (targetSpeed - speed) * deltaTime * convergeRate;
		z += speed * deltaTime;
		cameraZ = z + cameraZStart;
		return z;
	}

	public bool IsActive() {
		return z < finishZ;
	}

	private bool isColliding = false;

	/**
	 * Do not double-count contiguous frames of collision.
	 */
	public void UpdateCollision(bool isCollidingNow) {
		if (isCollidingNow && IsActive() && !isColliding) {
			speed *= 0.5f;
			// targetSpeed *= 0.975f;
			// idealConvergeRate *= 1.125f;
			Debug.Log("SpeedModel.UpdateCollision: targetSpeed " + targetSpeed);
		}
		isColliding = isCollidingNow && IsActive();
	}
}
