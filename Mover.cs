using RPG.Combat;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField]
        Transform target;
        [SerializeField] float maxSpeed = 6f;

        NavMeshAgent navMeshAgent;
        Health health;

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            //Metoda, która zakazuje nieżywym AI, biegać za postacią
            navMeshAgent.enabled = !health.IsDead();

            //Anulowanie walki
            //GetComponent<Fighter>().Cencel();
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float SpeedFraction)
        {
            //Funkcja odpowiedzialna za zależności między poruszaniem się a walką. Albo Walka albo poruszanie się
            GetComponent<ActionScheduler>().StartAction(this);
           // GetComponent<Fighter>().Cancel(); usuwamy jak mamy interface
            MoveTo(destination,SpeedFraction);
        }

        public void MoveTo(Vector3 destination,float SpeedFraction)
        {
            //Wyodrębniamy nową funckję - patrz wyżej funckja MoveTo
           navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(SpeedFraction);
            navMeshAgent.isStopped = false;
        }
        //Implementuje funckje, która odnosi się do Interfacu IAction
        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        //Animacje postaci
        private void UpdateAnimator()
        {
            //Vector3 velocity = GetComponent<NavMeshAgent>().velocity;

            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            //Funkcja resetowania pozycyji/ stanów
            SerializableVector3 position = (SerializableVector3)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}