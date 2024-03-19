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
    private bool isDone = false;

    private Vector3 savePos = Vector3.zero;

    public SpawnBlock(GameManager game) : base(game)
    {

    }

    protected override void OnRunning()
    {
        if (isDone) return;

        CreateGround();

        for (int i = 0; i < routineNum / 4; i++)
        {
            CreateBlock(5, 90, Direction.zPos);
        }
        for (int i = 0; i < routineNum / 4; i++)
        {
            CreateBlock(5, 180, Direction.xPos);
        }
        for (int i = 0; i < routineNum / 4; i++)
        {
            CreateBlock(-5, 270, Direction.zPos);
        }
        for (int i = 0; i < routineNum / 4; i++)
        {
            CreateBlock(-5, 360, Direction.xPos);
        }

        isDone = true;

        base.Initialize();
    }

    private GameObject CreateGround()
    {
        var newObj = ObjectPool.instance.GetObject(PoolObjectType.ground);
        newObj.transform.position = new Vector3(25, -2, 25);

        return newObj;
    }

    private void CreateBlock(int sign, int rotate, Direction dir)
    {
        var newObj = ObjectPool.instance.GetObject(BlockType(poolType));

        if ((GameManager.Instance.blockPos.Count) % 10 == 0)
        {
            Transform child = newObj.transform.GetChild(0);

            child.Rotate(new Vector3(0, 0, 45));
            child.position = new Vector3(0.7f, 2f, 1.7f);
        }

        SaveTowerPos(newObj.transform.GetChild(1));
        PositionAdj(dir, sign, newObj);

        newObj.transform.Rotate(new Vector3(0, rotate, 0));

        GameManager.Instance.blockPos.Add(newObj.transform);
    }

    private void SaveTowerPos(Transform parent)
    {
        foreach(var child in parent)
        {
            //
        }
    }

    private void PositionAdj(Direction value, int sign, GameObject newObj)
    {
        if (value == Direction.zPos)
        {
            newObj.transform.position = savePos;
            savePos += new Vector3(0, 0, sign);
        }
        else
        {
            newObj.transform.position = savePos;
            savePos += new Vector3(sign, 0, 0);
        }
    }

    private PoolObjectType BlockType(PoolObjectType type)
    {
        if (type == 0)
            type = PoolObjectType.block02;
        else
            type = PoolObjectType.block01;

        return poolType = type;
    }
}
