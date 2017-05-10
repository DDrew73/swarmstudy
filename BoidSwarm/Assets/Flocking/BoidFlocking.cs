using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoidFlocking : MonoBehaviour
{
	internal BoidController controller;

	float speed = 0.5f;
	bool turning = false;


	IEnumerator Start()
	{
		while (true)
		{

			/*
			if (controller)
			{
				GetComponent<Rigidbody>().velocity += steer() * Time.deltaTime;

				// enforce minimum and maximum speeds for the boids
				float speed = GetComponent<Rigidbody>().velocity.magnitude;
				if (speed > controller.maxVelocity)
				{
					GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * controller.maxVelocity;
				}
				else if (speed < controller.minVelocity)
				{
					GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * controller.minVelocity;
				}
			}
			float waitTime = Random.Range(0.3f, 0.5f);
			yield return new WaitForSeconds(waitTime);
			*/

			if (controller) {

				if(Vector3.Distance(transform.position, Vector3.zero) >= controller.tankSize)
				{
					turning = true;
					//Debug.Log ("Outside the tank!");
				}
				else
					turning = false;

				if (turning) {
					Vector3 direction = Vector3.zero - gameObject.transform.position;
					transform.rotation = Quaternion.Slerp (gameObject.transform.rotation,
						Quaternion.LookRotation (direction), 
						controller.rotationSpeed * Time.deltaTime);
					speed = Random.Range (0.5f, 1);
				} else {
					if (Random.Range (0, 5) < 1) {
						ApplyRules ();
					}
				}
				gameObject.transform.Translate(0, 0, Time.deltaTime * speed);
				//Debug.Log ("Speed: "+speed);
			}
			float waitTime = Random.Range(0.3f, 0.5f);
			yield return new WaitForSeconds(waitTime);
		}
	}
		
	void ApplyRules()
	{
		//GameObject[] gos;
		//gos = globalFlock.allFish;
		//gos = controller.boids;

		Vector3 vcentre = Vector3.zero;
		Vector3 vavoid = Vector3.zero;
		float gSpeed = 0.1f;
		//0.1 works but they cant follow walking NPC fast enough
		//With 1.0 they go careening away from his head whirlwind style.
		//float gSpeed = 1.0f;

		float dist;
		Vector3 goalPos = controller.goalPos;

		int groupSize = 0;
		foreach (BoidFlocking go in controller.boids) 
		{
			if(go.gameObject != this.gameObject)
			{
				//Debug.Log ("go != this.gameObject");
				dist = Vector3.Distance(go.gameObject.transform.position,this.gameObject.transform.position);
				if(dist <= controller.neighbourDistance)
				{
					vcentre += go.gameObject.transform.position;	
					groupSize++;	
					//Debug.Log ("GroupSize=" + groupSize);

					if(dist < controller.avoidDist)		
					{
						vavoid = vavoid + (this.gameObject.transform.position - go.gameObject.transform.position);
						//Debug.Log ("Avoid!");
					}

					BoidFlocking anotherFlock = go.GetComponent<BoidFlocking>();
					gSpeed = gSpeed + anotherFlock.speed;
				}
			}
		} 

		if(groupSize > 0)
		{
			//Debug.Log ("Group Size:" + groupSize);
			vcentre = vcentre/groupSize + (goalPos - this.gameObject.transform.position);
			speed = gSpeed/groupSize;
			Debug.Log ("ventre:" + vcentre);
			Vector3 direction = (vcentre + vavoid) - gameObject.transform.position;
			Debug.Log ("direction:" + direction);
			if (direction != Vector3.zero) {
				//Debug.Log ("Turning" + Time.deltaTime);
				gameObject.transform.rotation = Quaternion.Slerp (gameObject.transform.rotation,
					Quaternion.LookRotation (direction), 
					controller.rotationSpeed * Time.deltaTime);
				//Debug.Log("current:" + transform.rotation + "goal:" + Quaternion.LookRotation(direction));
			}

		}
	}

	/*
	Vector3 steer()
	{
		Vector3 randomize = new Vector3((Random.value * 2) - 1, (Random.value * 2) - 1, (Random.value * 2) - 1);
		randomize.Normalize();
		randomize *= controller.randomness;

		Vector3 center = controller.flockCenter - transform.localPosition;
		Vector3 velocity = controller.flockVelocity - GetComponent<Rigidbody>().velocity;
		Vector3 follow = controller.target.localPosition - transform.localPosition;

		return (center + velocity + follow * 2 + randomize);
	}
	*/
}