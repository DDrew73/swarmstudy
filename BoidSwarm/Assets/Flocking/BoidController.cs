using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// these define the flock's behavior
/// </summary>
public class BoidController : MonoBehaviour
{
	//User-controlled flocking variables
	public float minVelocity = 5;
	public float maxVelocity { get; set; }
	public float randomness = 1;
	public float flockSize { get; set; }
	public float neighbourDistance = 5.0f;
	public float rotationSpeed = 10.0f;
	public float avoidDist = 1.0f;

	public int tankSize = 15;

	public Vector3 goalPos = Vector3.zero; 

	//To be changed to keep everyone happy....
	int flockNum = 100;
	public static int spawnSize = 5;

	//Instantiation helpers
	//public GameObject prefab;
	public BoidFlocking prefab;
	public Transform target;

	internal Vector3 flockCenter;
	internal Vector3 flockVelocity;

	public List<BoidFlocking> boids = new List<BoidFlocking>();
	//public List<GameObject> boids = new List<GameObject>();

	void Start()
	{
		//GameObject[] flockObjs = new GameObject[500];

		//flockNum = (int)flockSize;
		flockSize = 100;
		maxVelocity = 1;
		Debug.Log ("flockNum=" + flockNum);
		Debug.Log ("flockSize=" + flockSize);
		for (int i = 0; i < flockNum; i++)
		{
			Vector3 pos = new Vector3(Random.Range(goalPos.x-spawnSize,goalPos.x+spawnSize),
				Random.Range(goalPos.y-spawnSize,goalPos.y+spawnSize),
				Random.Range(goalPos.z-spawnSize,goalPos.z+spawnSize));
			
			BoidFlocking boid = Instantiate(prefab, pos, Quaternion.identity) as BoidFlocking;
			//GameObject boid = Instantiate(prefab, transform.position, transform.rotation);
			//boid.transform.parent = transform;
			/*
			boid.transform.localPosition = new Vector3(
							Random.value * GetComponent<Collider>().bounds.size.x,
							Random.value * GetComponent<Collider>().bounds.size.y,
							Random.value * GetComponent<Collider>().bounds.size.z) - GetComponent<Collider>().bounds.extents;
							*/
			boid.controller = this;
			boids.Add(boid);
		}
	}

	void Update()
	{
		//Debug.Log ("flockNum=" + flockNum);
		//Debug.Log ("flockSize=" + flockSize);
		goalPos = target.position;

		flockRespawn ();

		Vector3 center = Vector3.zero;
		Vector3 velocity = Vector3.zero;
		foreach (BoidFlocking boid in boids)
		{
			center += boid.gameObject.transform.localPosition;
			velocity += boid.GetComponent<Rigidbody>().velocity;
		}
		flockCenter = center / flockNum;
		flockVelocity = velocity / flockNum;
	}

	void flockRespawn()
	{
		if (flockSize > flockNum) {
			int flockDiff = (int)flockSize - flockNum;
			for (int i = 0; i < flockDiff; i++) {
				BoidFlocking boid = Instantiate (prefab, transform.position, transform.rotation) as BoidFlocking;
				boid.transform.parent = transform;
				boid.transform.localPosition = flockCenter;
				boid.controller = this;
				boids.Add (boid);
			}
			flockNum = (int)flockSize;
		}

		if ((int)flockSize < flockNum) {
			int flockDiff = flockNum - (int)flockSize;
		
			for (int i = 0; i < flockDiff; i++) {
				GameObject kill = GameObject.Find("boid5" + "(Clone)");
				Destroy (kill);
			}
		
			boids.RemoveRange (0,flockDiff);

			flockNum = (int)flockSize;
		}


	}
}