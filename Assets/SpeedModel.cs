/**
 * Lower ideal speed, raise ideal converge rate, lower competitor speeds.  Test case:  2015-11-01 Expect to intercept 49 competitors interpolated between 1.3 sec and 0.3 seconds, each number going down.  Previously trend was increasing.
 * 
 */
public class SpeedModel {

	public static float finishZ;

	public static void setFinishZ(bool isShort) {
		if (isShort) {
			SpeedModel.finishZ = 20.0f;
		}
		else {
			SpeedModel.finishZ = 240.0f;
		}
	}

	public float cameraZ = 0.0f;
	public float cameraZStart = 0.0f;
	public float idealSpeed = 	
					// 50.0f;
					80.0f;
					// 100.0f;
					// 200.0f;
					// 400.0f;
	public float speed = 0.0f;
	public float targetSpeed = 5.0f;
	public float z = 0.0f;

	private float convergeRate = 0.125f;
	private float idealConvergeRate = 
						// 0.0025f;
						// 0.005f;
						0.01f;
						// 0.05f;
						// 0.1f;
						// 0.5f;

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
		for (int c = 0; c < competitors.Length; c++) {
			competitors[c] = new SpeedModel();
			SpeedModel competitor = competitors[c];
			float cf = (float) competitors.Length - c - 1;
			competitor.idealSpeed = 5.0f + idealPerCompetitor * cf;
			competitor.targetSpeed = 5.0f + targetPerCompetitor * cf;
			competitor.z = 1.0f + cf;
		}
		return competitors;
	}

	/**
	 * After finish, slow down and stop in front of result billboard.
	 */
	public float Update (float deltaTime) {
		if (SpeedModel.finishZ < z) {
			idealSpeed = 0.0f;
			idealConvergeRate = 10.0f;
			convergeRate = 2.0f;
		}
		targetSpeed += (idealSpeed - targetSpeed) * deltaTime * idealConvergeRate;
		speed += (targetSpeed - speed) * deltaTime * convergeRate;
		z += speed * deltaTime;
		cameraZ = z + cameraZStart;
		return z;
	}
}
