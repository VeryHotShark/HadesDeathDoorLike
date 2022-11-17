using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    public sealed class IdGenerator
    {
        private static int _id = 1;
        private IdGenerator()
        {
        }

        public int Id
        {
            get { return _id++; }
        }

        public static IdGenerator Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }
            internal static readonly IdGenerator instance = new IdGenerator();
        }
    }
}