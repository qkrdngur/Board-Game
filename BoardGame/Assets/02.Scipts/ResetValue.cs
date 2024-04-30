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
            manager.BuildingOwner[i] = PlayMoney.None;
            manager.buildCount[i] = 0;
            manager.curTower[i] = new List<GameObject>();
        }

        for (int i = 0; i <= (int)PlayTurn.TirAi; i++)
        {
            manager.curBlock[(PlayTurn)i] = 0;
            manager.playerSO.Money[i] = initPrice;
            manager.money[(PlayTurn)i] = initPrice;
        }

        UiManager.Instance.PlayerUISetUp(manager.playerSO.Img, manager.playerSO.Name, manager.playerSO.Money);
    }
}
