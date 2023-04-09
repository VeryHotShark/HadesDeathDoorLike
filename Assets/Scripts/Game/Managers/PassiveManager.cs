using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class PassiveManager : Singleton<PassiveManager> {
        private List<PassiveSO> _runtimePassives = new();
        public static List<PassiveSO> RuntimePassives => Instance._runtimePassives;

        public static void AddPassives(params PassiveSO[] passivesSO) {
            foreach (var passiveSO in passivesSO) 
                AddPassive(passiveSO);   
        }
        
        public static void AddPassive(PassiveSO passiveSO) {
            if(!Instance._runtimePassives.Contains(passiveSO)) 
                Instance._runtimePassives.Add(passiveSO);
        }
        
    }
}
