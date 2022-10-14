using System.Collections.Generic;
using UnityEngine;
using ExtensionsMain;

namespace Utils
{
    public static class BaseUtils
    {
        public static bool Roll(int chance)
        {
            int roll = Random.Range(0, 101);
            return roll <= chance;
        }
    }
}


