using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject eqippedWeapon = null;
        [SerializeField] float WeaponDamage = 5f;
        [SerializeField] float weaponRange = 2f; // Odległość jaką nasza postać zachowa od wroga w momencie walki
        [SerializeField] bool isRightHanded = true;

        public void Spawn(Transform righHandTransform, Transform leftHandTransform, Animator animator)
        {

            if (eqippedWeapon != null)
            {
                //Tworzymy broń
                Transform handTransform;
                //Warunek sprawdzenia, w której rece jest br
                if (isRightHanded) handTransform = righHandTransform;
                else handTransform = leftHandTransform;
                Instantiate(eqippedWeapon, righHandTransform);
            }
            if (animatorOverride != null )
            //Jeżeli animacja nie jest "pusta" to dodajemy ją 
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
        }


        public float GetDamage()
        {
            return WeaponDamage;
        }
        public float GetRange()
        {
            return weaponRange; 
        }

    }
}