using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class BoidWatcher : MonoBehaviour
{
	public float camDist{ get; set;}
	bool origSet = false;
	public BoidController boidController;
	internal Vector3 origPos;

	void LateUpdate()
	{
		if (boidController)
		{
			if (origSet == false) {
				origPos = boidController.flockCenter;
				origPos.x = origPos.x + 100;
				origSet = true;
			}

			transform.LookAt(boidController.flockCenter + boidController.transform.position);
			transform.position = Vector3.Lerp (origPos, boidController.flockCenter, camDist);



		
		}
	}
}