using System;
using UnityEngine;

namespace BoardGame.Util
{
    [Serializable]
    public class ObjectPoolData
    {
        public GameObject prefabs;
        public PoolObjectType poolType;
        public int poolCount;
    }
}

