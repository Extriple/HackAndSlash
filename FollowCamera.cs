
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
                   //podłączamy kamera z naszym targetem ==> W Unity przerzucamy main camera do follow camery ! oraz wrzucamy follow camera do Game
        [SerializeField] Transform target;

        void LateUpdate()
        {
            transform.position = target.position;
        }
    }
}

/* 
 * UNITY --- ustawienia kamery 
 * 1.Kopiujemy połozenia gracza/postaci i wklejemy je do follow camery
 * 2. Main Camery  ustawiamy na aling with view
 */            
