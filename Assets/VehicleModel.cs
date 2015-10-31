public class VehicleModel {
	public SpeedModel speed = new SpeedModel();
	public SteeringModel steering = new SteeringModel();

	public void Start () {
	}
	
	public void Update (float deltaTime) {
		steering.Update(deltaTime);
		speed.Update(deltaTime);
	}
}
