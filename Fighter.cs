using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float TimeBetweenAttack = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null; //defaultWeapon --> broń domyślna




        Health target;
        //Ostatni atak jest większy, niż czas pomiędzy atakami
        float TimeSinceLastAttack = Mathf.Infinity;
        Weapon currentWeapon = null;


        private void Start()
        {
            EquipWeapon(defaultWeapon);     //Funkcja odpowiedzialna za pojawienie sie broni naszej postaci (domyślna broń)

        }
        private void Update()
        {
            //Mierzymy czas od ostatniego ataku
            TimeSinceLastAttack += Time.deltaTime;

            //Funkcja odpowiedzialna za ruch do wroga w momencie gdy mamy zaznaczony target
            if (target == null) return;

            if (target.IsDead()) return;
            if(!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position,1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                //Ustawiamy zależności, która bedzie odpowiedzialna ruch/animacje walki ----- wyciągamy metodę z  GetComponent<Animator>().SetTrigger("attack");
                AttackBehaviour();
            }
        }


        public void EquipWeapon(Weapon weapon)
        {
            //if (weapon == null) return;
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);  //Automatyczny kontroler animacji przypisany do broni 
        }


        private void AttackBehaviour()
        {
            // podczas ataku postać patrzy się w strone przeciwika
            transform.LookAt(target.transform);

            if (TimeSinceLastAttack > TimeBetweenAttack)
            {
                //This will trigger the Hit() event
                TriggetAttack();
                TimeSinceLastAttack = 0;

            }
        }

        private void TriggetAttack()
        {
            GetComponent<Animator>().SetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        //Animation Event
        void Hit()
            {
            //Element zdrowotny
            if (target == null) { return; }
            target.TakeDamage(currentWeapon.GetDamage());
        }

         

        private bool GetIsInRange()
        {
            //Sprawdzamy czy wróg jest w zasięgu
            //Wyodrębniamy metodę  dla Vector3.Distance(transform.position, target.position) < weaponRange;
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetRange();


        }

        public bool CanAttack(GameObject combatTarget)
        {
            if(combatTarget == null) { return false; }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            //Funkcja odpowiedzialna za zależności między poruszaniem się a walką. Albo Walka albo poruszanie się
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
        }

        private void StopAttack()
        {
            GetComponent<Animator>().SetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

    }
}



//Jeżeli chcemy stworzyć animacje dla broni to w Unity  w prefabach broni tworzymy Animation override controller!!!
