using UnityEngine;
using System.Collections;

public class flock : MonoBehaviour {
	internal globalFlock controller;
	/*
	public float speed = 0.01f;
	public float rotationSpeed = 4.0f;
	public float neighbourDistance = 5.0f;
	public float avoidDist = 0.1f;
	*/

	float speed = 0.01f;

	Vector3 averageHeading;
	Vector3 averagePosition;

	bool turning = false;

	// Use this for initialization
	void Start () 
	{
		speed = Random.Range(0.5f,1);
	}
	
	// Update is called once per frame
	void Update () {

	if (controller)
	{
			Debug.Log ("If controller");
		if(Vector3.Distance(transform.position, Vector3.zero) >= controller.tankSize)
		{
			turning = true;
		}
		else
			turning = false;

		if(turning)
		{
			Vector3 direction = Vector3.zero - transform.position;
			transform.rotation = Quaternion.Slerp(transform.rotation,
					                                  Quaternion.LookRotation(direction), 
					                                  controller.rotationSpeed * Time.deltaTime);
			speed = Random.Range(0.5f,1);
		}
		else
		{
			if(Random.Range(0,5) < 1)
				ApplyRules();
			
		}
		transform.Translate(0, 0, Time.deltaTime * speed);
	}
	}
	void ApplyRules()
	{
		GameObject[] gos;
		gos = globalFlock.allFish;
		//gos = controller.boids;

		Vector3 vcentre = Vector3.zero;
		Vector3 vavoid = Vector3.zero;
		float gSpeed = 0.1f;
		//0.1 works but they cant follow walking NPC fast enough
		//With 1.0 they go careening away from his head whirlwind style.
		//float gSpeed = 1.0f;

		Vector3 goalPos = globalFlock.goalPos;

		float dist;

		int groupSize = 0;
		foreach (GameObject go in gos) 
		{
			if(go != this.gameObject)
			{
				dist = Vector3.Distance(go.transform.position,this.transform.position);
				if(dist <= controller.neighbourDistance)
				{
					vcentre += go.transform.position;	
					groupSize++;	
					
					if(dist < controller.avoidDist)		
					{
						vavoid = vavoid + (this.transform.position - go.transform.position);
					}
					
					flock anotherFlock = go.GetComponent<flock>();
					gSpeed = gSpeed + anotherFlock.speed;
				}
			}
		} 
		
		if(groupSize > 0)
		{
			vcentre = vcentre/groupSize + (goalPos - this.transform.position);
			speed = gSpeed/groupSize;
			
			Vector3 direction = (vcentre + vavoid) - transform.position;
			if(direction != Vector3.zero)
				transform.rotation = Quaternion.Slerp(transform.rotation,
					                                  Quaternion.LookRotation(direction), 
					                                  controller.rotationSpeed * Time.deltaTime);
		
		
		}
	}

	void Pulse()
	{


	}

}
