using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public interface IHittable {
        void Hit(HitData hitData); 
        GameObject gameObject { get; }
    }
}
