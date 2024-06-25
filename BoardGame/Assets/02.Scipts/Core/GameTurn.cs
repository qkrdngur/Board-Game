using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoardGame.Util;

public class GameTurn : GameComponent
{
    GameManager manager;

    public GameTurn(GameManager game) : base(game)
    {
        manager = GameManager.Instance;
    }

    protected override void OnMain()
    {
        base.OnMain();

        CalcMoney();
        UiManager.Instance.PlayerUISetUp(manager.playerSO.Img, manager.playerSO.Name, manager.playerSO.Money);
    }

    private void CalcMoney()
    {
        for(int i = 0; i < manager.playerSO.Money.Count; i++)
            manager.playerSO.Money[i] = manager.money[(PlayTurn)i];
    }
}
