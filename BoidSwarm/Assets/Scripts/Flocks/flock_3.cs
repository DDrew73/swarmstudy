using UnityEngine;
using System.Collections;

public class flock_3 : MonoBehaviour {

	public float speed = 0.01f;
	public float rotationSpeed = 4.0f;

	//standard = 3.0
	public float neighbourDistance = 5.0f;

	public float avoidDist = 2.0f;

	Vector3 averageHeading;
	Vector3 averagePosition;

	bool turning = false;

	// Use this for initialization
	void Start () 
	{
		speed = Random.Range(0.5f,1);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Vector3.Distance(transform.position, Vector3.zero) >= globalFlock_3.tankSize)
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
					                                  rotationSpeed * Time.deltaTime);
			speed = Random.Range(0.5f,1);
		}
		else
		{
			if(Random.Range(0,5) < 1)
				ApplyRules();
			
		}
		transform.Translate(0, 0, Time.deltaTime * speed);
	}

	void ApplyRules()
	{
		GameObject[] gos;
		gos = globalFlock_3.allFish;
		
		Vector3 vcentre = Vector3.zero;
		Vector3 vavoid = Vector3.zero;
		float gSpeed = 0.1f;
	
		Vector3 goalPos = globalFlock_3.goalPos;

		float dist;

		int groupSize = 0;
		foreach (GameObject go in gos) 
		{
			if(go != this.gameObject)
			{
				dist = Vector3.Distance(go.transform.position,this.transform.position);
				if(dist <= neighbourDistance)
				{
					vcentre += go.transform.position;	
					groupSize++;	
					
					if(dist < avoidDist)		
					{
						vavoid = vavoid + (this.transform.position - go.transform.position);
					}
					
					flock_3 anotherFlock = go.GetComponent<flock_3>();
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
					                                  rotationSpeed * Time.deltaTime);
		
		}
	}
}
