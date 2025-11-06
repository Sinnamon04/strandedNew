using Platformers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Platformers
{

    public class NormalAI : MonoBehaviour
    {

        // References to the NavMeshAgent and player transform
        [Header("References for AI")]
        [SerializeField] private NavMeshAgent navAgent;
        [SerializeField] private Transform playerTransform;


        // Layer masks for terrain and player detection
        [Header("Layers")]
        [SerializeField] private LayerMask terrainLayer;
        [SerializeField] private LayerMask playerLayerMask;

        // Patrol settings
        [Header("Patrols")]
        [SerializeField] private float patrolRadius = 10f;
        private Vector3 currentPatrolPoint;
        private bool hasPatrolPoint;


        

        [Header("Player Detection")]
        [SerializeField] private float visionRange = 20f;
        [SerializeField] private float engagementRange = 10f;

        // Internal state variables to check player visibility and range
        private bool isPlayerVisible;
        private bool isPlayerInRange;


        // Initialization if playerTransform or navAgent are not assigned in the inspector
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

        // Main update loop to detect player and update AI behavior
        private void Update()
        {
            DetectPlayer();
            UpdateBehaviourState();
        }


        // Visualize detection ranges in the editor
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, engagementRange);


            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, visionRange);
        }

        // Method to detect if the player is visible or within engagement range
        private void DetectPlayer()
        {
            isPlayerVisible = Physics.CheckSphere(transform.position, visionRange, playerLayerMask);
            isPlayerInRange = Physics.CheckSphere(transform.position, engagementRange, playerLayerMask);
        }

        // Method to find a random patrol point within the patrol radius

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

        // Method to update AI behavior based on player detection
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