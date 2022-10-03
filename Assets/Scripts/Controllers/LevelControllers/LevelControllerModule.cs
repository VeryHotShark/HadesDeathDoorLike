using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class LevelControllerModule : ChildBehaviour<LevelController> {
        /// <summary>
        /// Called by LevelController after it finish its initialization so all reference are setup
        /// </summary>
        public virtual void MyEnable() { }
        
        /// <summary>
        /// Same as MyEnable but Disable
        /// </summary>
        public virtual void MyDisable() { }
    }
}
