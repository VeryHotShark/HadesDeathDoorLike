using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public static class LayerManager {
        public static class Layers {
            public const int DEFAULT = 0;
            public const int PLAYER = 7;
            public const int NPC = 8;
        }

        public static class Masks {
            public static LayerMask NPC = 1 << Layers.NPC;
            public static LayerMask PLAYER = 1 << Layers.PLAYER;
            public static LayerMask DEFAULT = 1 << Layers.DEFAULT;
            public static LayerMask ACTORS = NPC | PLAYER;
            public static LayerMask DEFAULT_AND_NPC = DEFAULT | NPC;
            public static LayerMask DEFAULT_AND_ACTORS = DEFAULT | ACTORS;
        }
    }
}