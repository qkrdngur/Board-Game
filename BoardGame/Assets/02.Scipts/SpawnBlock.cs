using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoardGame.Util;
using System;

public enum Direction
{
    zPos,
    xPos
}

public class SpawnBlock : GameComponent
{
    private PoolObjectType poolType = PoolObjectType.ground01;

    private float routineNum = 40;

    private Vector3 savePos = Vector3.zero;

    public SpawnBlock(GameManager game) : base(game)
    {

    }

    protected override void OnRunning()
    {
        for (int i = 0; i < routineNum / 4; i++)
        {
            CreateBlock(5, Direction.zPos);
        }
        for (int i = 0; i < routineNum / 4; i++)
        {
            CreateBlock(5, Direction.xPos);
        }
        for (int i = 0; i < routineNum / 4; i++)
        {
            CreateBlock(-5, Direction.zPos);
        }
        for (int i = 0; i < routineNum / 4; i++)
        {
            CreateBlock(-5, Direction.xPos);
        }

        base.Initialize();
    }

    private void CreateBlock(int sign, Direction dir)
    {
        var newObj = ObjectPool.instance.GetObject(BlockType(poolType));

        if (dir == Direction.zPos)
        {
            savePos += new Vector3(0, 0, sign);
            newObj.transform.position = savePos;
        }
        else
        {
            savePos += new Vector3(sign, 0, 0);
            newObj.transform.position = savePos;
        }
    }

    private PoolObjectType BlockType(PoolObjectType type)
    {
        if(type == 0)
            type = PoolObjectType.ground02;
        else
            type = PoolObjectType.ground01;

        return poolType = type;
    }
}
