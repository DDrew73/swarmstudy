using UnityEngine;
using System.Collections;

public class globalFlock_3 : MonoBehaviour {

	public GameObject fishPrefab;
	public static int tankSize = 15;
	public static int spawnSize = 3;

	static int numFish = 100;
	//static int minHeight = 5;

	public static GameObject[] allFish = new GameObject[numFish];
	public Transform target;

	//old goalpos
	public static Vector3 goalPos = Vector3.zero; 

	// Use this for initialization
	void Start () 
	{
		//OLD
		/*
		goalPos = new Vector3(Random.Range(-tankSize,tankSize),
			Random.Range(minHeight,tankSize),
			Random.Range(-tankSize,tankSize));
		*/
		goalPos = target.position;
		for(int i = 0; i < numFish; i++)
		{
			Vector3 pos = new Vector3(Random.Range(goalPos.x-spawnSize,goalPos.x+spawnSize),
				Random.Range(goalPos.y-spawnSize,goalPos.y+spawnSize),
				Random.Range(goalPos.z-spawnSize,goalPos.z+spawnSize));
			allFish[i] = (GameObject) Instantiate(fishPrefab, pos, Quaternion.identity);
		}
	}

	// Update is called once per frame
	void Update () 
	{
		goalPos = target.position;
		/* RANDOM FLOCKING
		if(Random.Range(0,10000) < 50)
		{
			goalPos = new Vector3(Random.Range(-tankSize,tankSize),
				                  Random.Range(minHeight,tankSize),
				                  Random.Range(-tankSize,tankSize));
		}
		*/
	}
}
