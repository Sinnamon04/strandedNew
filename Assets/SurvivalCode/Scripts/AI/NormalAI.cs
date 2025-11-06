using Platformers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Platformers
{

    


    public class NormalAI : MonoBehaviour
    {
        [Header("References for AI")]
        [SerializeField] private NavMeshAgent navAgent;
        [SerializeField] private Transform playerTransform;
      


        [Header("Layers")]
        [SerializeField] private LayerMask terrainLayer;
        [SerializeField] private LayerMask playerLayerMask;


        [Header("Patrols")]
        [SerializeField] private float patrolRadius = 10f;
        private Vector3 currentPatrolPoint;
        private bool hasPatrolPoint;


        

        [Header("Player Detection")]
        [SerializeField] private float visionRange = 20f;
        [SerializeField] private float engagementRange = 10f;


        private bool isPlayerVisible;
        private bool isPlayerInRange;
 


        private void Awake()
        {
            if (playerTransform == null)
            {
                GameObject playerObj = GameObject.Find("FPSController");
                if (playerObj != null)
                {
                    playerTransform = playerObj.transform;
                }
            }


            if (navAgent == null)
            {
                navAgent = GetComponent<NavMeshAgent>();
            }
        }


        private void Update()
        {
            DetectPlayer();
            UpdateBehaviourState();
        }

       

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, engagementRange);


            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, visionRange);
        }


        private void DetectPlayer()
        {
            isPlayerVisible = Physics.CheckSphere(transform.position, visionRange, playerLayerMask);
            isPlayerInRange = Physics.CheckSphere(transform.position, engagementRange, playerLayerMask);
        }

        


        

        private void FindPatrolPoint()
        {
            float randomX = Random.Range(-patrolRadius, patrolRadius);
            float randomZ = Random.Range(-patrolRadius, patrolRadius);


            Vector3 potentialPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);


            if (Physics.Raycast(potentialPoint, -transform.up, 2f, terrainLayer))
            {
                currentPatrolPoint = potentialPoint;
                hasPatrolPoint = true;
            }
        }


   

        private void PerformPatrol()
        {
            if (!hasPatrolPoint)
                FindPatrolPoint();


            if (hasPatrolPoint)
                navAgent.SetDestination(currentPatrolPoint);


            if (Vector3.Distance(transform.position, currentPatrolPoint) < 1f)
                hasPatrolPoint = false;
        }


        private void PerformChase()
        {
            if (playerTransform != null)
            {
                navAgent.SetDestination(playerTransform.position);
            }
        }


        


        private void UpdateBehaviourState()
        {
            if (!isPlayerVisible && !isPlayerInRange)
            {
                PerformPatrol();
            }
            else if (isPlayerVisible && !isPlayerInRange)
            {
                PerformChase();
            }
           
        }
    }
}