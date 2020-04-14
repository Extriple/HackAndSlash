using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;
        [Range(0, 1)]
        [SerializeField] float PatrolSpeedFraction = 0.2f;
        
        //Ten ułamek jest po to ponieważ, gdy wróg patroluje to ma miejszy poziom predkości




        //Dodajemy i aplikujemy komponent
        Fighter fighter;
        GameObject player;
        Health health;
        Mover mover;

        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");

            guardPosition = transform.position;
            //Pamieć wroga
            
        }


        private void Update()
        {

            //sprawdzamy poziom życia, aby w przypadku śmierci gracza AI przestało atakować 

            if (health.IsDead()) return;

            //Funckja, która pozwala na atakowanie gracza, jeżeli znajduje sie w zasięgu wrogiej jednostki
            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                
                AttackBehaviour();
            }
            //Stan podejrzenia/wykrywalność [jeżeli nie możemy zaatakować]
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                //Zatrzymuje wroga jeżeli wyjdziemy z zasięgu walki/thearta
                PatrolBehaviour();
            }
            
            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        //Funckje tworzące mechanizm systemu patrolowania.
        //-------------------------------------------------------------------------------------//
        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    //Musimy zaktualizować po każdej rotacji
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            //Uwzględniamy zatrzymanie się strażnika na pare sec, gdy dotrze do Waypointu
            if(timeSinceArrivedAtWaypoint>waypointDwellTime)
            {
                mover.StartMoveAction(nextPosition, PatrolSpeedFraction);
            }
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
        //------------------------------------------------------------------------------------------------------//
        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
        }

            private bool InAttackRangeOfPlayer()
            {
                //Funckja odpowiedzialna za sterowanie dystansem wroga w stosunku do gracza 
                float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
                return distanceToPlayer < chaseDistance;
            }

            //rysowanie gizmosów, czyli tzn. gadżetów
            //Called by Unity
            /*private void OnDrawGizmosSelected()
            {
                Gizmos.color = Color.red;
                //Zostanie narysowana niebieska linia/strefa, która będzie pokazywała obszar, w którym strażnik reaguje na obecność  gracza
                Gizmos.DrawSphere(transform.position, chaseDistance);
            }*/

        }
}
