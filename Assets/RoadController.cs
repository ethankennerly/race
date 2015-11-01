using UnityEngine;
using System.Collections;

public class RoadController : MonoBehaviour {

	private Transform childTransform;
	private RaceModel model;
	private Transform[] transforms;
	private int index;
	private int count;
	private float length;
	private float segmentLength;

	/**
	 * If this were a hotspot, recyling could be optimized by sorting children by Z.
	 * http://answers.unity3d.com/questions/594210/get-all-children-gameobjects.html
	 */
	void Start () {
		model = RaceModel.getInstance();
		count = transform.childCount;
		transforms = new Transform[count];
		for (index = 0; index < count; index++) {
			transforms[index] = transform.GetChild(index);
		}
		segmentLength = transforms[0].localScale.z;
		length = segmentLength * count;
	}

	/**
	 * Array road segment quads.  While behind camera, recycle forward.
	 */
	void Update () {
		float offscreen = model.speed.cameraZ;
		for (index = 0; index < count; index++) {
			childTransform = transforms[index];
			if (childTransform.position.z < offscreen) {
				childTransform.position += Vector3.forward * length;
			}
		}
	}
}
