using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformers
{
    public class Projectile : MonoBehaviour
    {
        public int damage; // The amount of damage the projectile will do

        public void Destroys()
        {
            Destroy(this.gameObject);
        }


    }
}
