using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoardGame.Util;

public class Select : GameComponent
{
    GameManager manager;

    public Select(GameManager game) : base(game)
    {
        manager = GameManager.Instance;
    }

    protected override void OnSelect()
    {
        if ((manager.curBlock[manager.pTurn]) % 10 == 0) StateMain();

        //PayMoney();

        UiManager.Instance.ShowUI();
        UiManager.Instance.UndoImg();
    }

    private void PayMoney()
    {
        int price = manager.buildingPrice[manager.curBlock[manager.pTurn]];
        price *= (int)manager.tower;

        for (int i = 0; i < 4; i++)
            manager.DicMoney((PlayTurn)i, price, true);
    }

    private void StateMain()
    {
        manager.NextTurn(manager.pTurn);
        manager.UpdateState(GameState.Main);
    }
}
