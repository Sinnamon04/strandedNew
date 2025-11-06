using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformers
{
    public abstract class BaseXPTranslations : ScriptableObject
    {

        public int CurrentXP { get; protected set; } = 0;
        public int CurrentLevel { get; protected set; } = 1;
        public int Level { get; protected set; } = 1;

        public bool IsMaxLevel { get; protected set; } = false;

        public int XPToNextLevel => GetXRequiredForNextLevel();


        protected abstract int GetXRequiredForNextLevel();

   

        public abstract bool AddXP(int amount);

        public abstract void SetLevel(int level);   




    }
}
