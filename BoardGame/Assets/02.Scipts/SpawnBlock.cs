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
    private PoolObjectType poolType = PoolObjectType.block01;

    private float routineNum = 40;

    private Vector3 savePos = Vector3.zero;

    public SpawnBlock(GameManager game) : base(game)
    {

    }

    protected override void OnRunning()
    {
        CreateGround();

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

    private GameObject CreateGround()
    {
        var newObj = ObjectPool.instance.GetObject(PoolObjectType.ground);
        newObj .transform.position = new Vector3(25, -2, 25);
        return newObj;
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

        GameManager.Instance.blockPos.Add(newObj.transform);
    }

    private PoolObjectType BlockType(PoolObjectType type)
    {
        if(type == 0)
            type = PoolObjectType.block02;
        else
            type = PoolObjectType.block01;

        return poolType = type;
    }
}
