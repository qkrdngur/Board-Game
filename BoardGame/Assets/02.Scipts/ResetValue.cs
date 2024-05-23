using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoardGame.Util;
using System;

public class ResetValue : GameComponent
{
    GameManager manager;

    private int initPrice = 10;

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

        foreach (PlayTurn play in Enum.GetValues(typeof(PlayTurn)))
        {
            manager.curBlock[play] = 0;
            manager.playerSO.Money[(int)play] = initPrice;
            manager.money[play] = initPrice;
        }

        UiManager.Instance.PlayerUISetUp(manager.playerSO.Img, manager.playerSO.Name, manager.playerSO.Money);
    }
}
