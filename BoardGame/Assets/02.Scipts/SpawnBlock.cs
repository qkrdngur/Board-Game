using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoardGame.Util;

public class SpawnBlock : MonoBehaviour
{
    [SerializeField] private GameObject[] block;
    private PoolObjectType poolType;
    private bool isGround = true;
    private Vector3 pos;
    private void Awake()
    {
        poolType = PoolObjectType.ground01;
        InstBlock();
    }

    void Start()
    {
            pos = block[0].transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InstBlock()
    {
        for (int i = 0; i < 10; i++)
        {
            Instantiate(block[BlockType(isGround)], pos, Quaternion.identity);
            isGround = !isGround;
            pos.z += 2.5f;
        }
        for (int i = 0; i < 10; i++)
        {
            Instantiate(block[BlockType(isGround)], pos, Quaternion.identity);
            isGround = !isGround;
            pos.x += 2.5f;
        }
        for (int i = 0; i < 10; i++)
        {
            Instantiate(block[BlockType(isGround)], pos, Quaternion.identity);
            isGround = !isGround;
            pos.z -= 2.5f;
        }
        for (int i = 0; i < 10; i++)
        {
            Instantiate(block[BlockType(isGround)], pos, Quaternion.identity);
            isGround = !isGround;
            pos.x -= 2.5f;
        }
    }

    private int BlockType(bool isG)
    {
        if (isG)
            poolType = PoolObjectType.ground02;
        else
            poolType = PoolObjectType.ground01;

        return (int)poolType;
    }
}
