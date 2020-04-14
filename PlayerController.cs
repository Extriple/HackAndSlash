using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{


    public class PlayerController : MonoBehaviour
    {
        Health health;

        private void Start()
        {
            health = GetComponent<Health>();
        }
        private void Update()
        {
            if (health.IsDead()) return; ;
            //Poniżej znajduje się wyodrębniona metoda InteractWithMovement

            //Tworzymy nową fukcję, która będzie wchodzić w interakcję z tzn "combatem"
            if  (InteractWithCombat()) return;
            //Jesli "ruch" wróci do wartości true pozostajemy w stanie  out-combat
           if  (InteractWithMovement()) return;
         
        }

        private bool InteractWithCombat()
        {
            //Funcka, któa ustawia przeciwka jako cel priorytetowy. Może zdażyć się, że przeciwnik bedzie np za przeszkodzą i go nie widzimy, wtedy nasza postać wykrywa go i atakuje
           RaycastHit[] hits =  Physics.RaycastAll(GetMouseRay());
            //Dla każdego hita stowrzy tzn recast.               Jeden hit --> cw hitsach :D ??
            foreach(RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                //Sprawdzamy czy występuje znaznaczony GameObject
                if (target == null) continue;

                if (!GetComponent<Fighter>().CanAttack(target.gameObject))
                {
                    continue;
                }
                // Prawym kliknięciem myszy rozpoczyna atakować wroga
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
              
                }
                //Zwracany true poza petlą ponieważ chcemy, żeby została zwrócona wartość true, jesli zakończymy walkę
                return true;
            }
            //Po upuszczeniu pętli zwracamy false poniewąż znaleźlismy więcej celów
            return false;
        }

        private bool InteractWithMovement()
        {
            //Wyodrębniamy metodę 
           // Ponieżej znajduje się wyodrębniona metoda dla Camera.main.ScreenPointToRay(Input.mousePosition);
            
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point,1f);
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            //Wyodrębniamy nową metodę dla: Camera.main.ScreenPointToRay(Input.mousePosition);
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}