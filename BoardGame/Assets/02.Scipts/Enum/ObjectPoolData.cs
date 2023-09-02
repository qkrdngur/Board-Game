using System;
using System.Collections;
using System.Collections.Generic;
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

