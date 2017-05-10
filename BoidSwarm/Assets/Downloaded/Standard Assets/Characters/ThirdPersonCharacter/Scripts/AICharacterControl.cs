using System;
using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target;                                    // target to aim for

		int iterator = 0;
        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();
			Debug.Log ("Started");
	        agent.updateRotation = false;
	        agent.updatePosition = true;
        }


        private void Update()
        {
			if (target != null && iterator > 500)

				agent.SetDestination(target.position);
			

            if (agent.remainingDistance > agent.stoppingDistance)
                character.Move(agent.desiredVelocity, false, false);
            else
                character.Move(Vector3.zero, false, false);
			iterator += 1;
		}


        public void SetTarget(Transform target)
        {
            this.target = target;
        }


		private IEnumerator WaitPath(float waitTime)
		{
			Debug.Log ("waiting");
			yield return new WaitForSecondsRealtime(waitTime);

			Debug.Log ("waited");
		}

	}
}

//print