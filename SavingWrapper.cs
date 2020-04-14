using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";


        void Update()
        {
            //Jeżeli wcisniesz literkę L wówczasz gra się wczyta
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            //Jeżeli wcisniesz literkę S wówczasz gra się zapisze
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }

        }
        private void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }
       private void Load()
        {
            //Wywołuje system wczytywania
            GetComponent<SavingSystem>().Load(defaultSaveFile);    
        }
    }
}
