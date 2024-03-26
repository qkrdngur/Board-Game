using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoardGame.Util;

public class ResetValue : GameComponent
{
    private int initPrice = 3000;

    public ResetValue(GameManager game) : base(game)
    {
    }

    protected override void Initialize()
    {
        base.Initialize();

        int count = GameManager.Instance.blockSO.RegionName.Count;

        for (int i = 0; i < count; i++)
        {
            GameManager.Instance.buildingPrice[i] = 0;
            GameManager.Instance.BuildingOwner[i] = PlayTurn.player;
            GameManager.Instance.buildCount[i] = 0;
        }

        for (int i = 0; i <= (int)PlayTurn.TirAi; i++)
        {
            GameManager.Instance.curBlock[(PlayTurn)i] = 1;
            GameManager.Instance.money[(PlayTurn)i] = initPrice;
        }
    }
}
