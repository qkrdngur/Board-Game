using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoardGame.Util;

public class ResetValue : GameComponent
{
    GameManager manager;

    private int initPrice = 3000;

    public ResetValue(GameManager game) : base(game)
    {
        manager = GameManager.Instance;
    }

    protected override void Initialize()
    {
        base.Initialize();

        int count = manager.blockSO.RegionName.Count;

        for (int i = 0; i < count; i++)
        {
            manager.buildingPrice[i] = manager.blockSO.BuildPrice[i];
            //manager.BuildingOwner[i] = PlayTurn.player;
            manager.buildCount[i] = 0;
        }

        for (int i = 0; i <= (int)PlayTurn.TirAi; i++)
        {
            manager.curBlock[(PlayTurn)i] = 0;
            manager.money[(PlayTurn)i] = initPrice;
        }
    }
}
