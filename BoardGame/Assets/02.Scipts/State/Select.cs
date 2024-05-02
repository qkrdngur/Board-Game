using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoardGame.Util;

public class Select : GameComponent
{
    GameManager manager;
    UiManager uiManager;

    public Select(GameManager game) : base(game)
    {
        manager = GameManager.Instance;
        uiManager = UiManager.Instance;
    }

    protected override void OnSelect()
    {
        if (manager.curBlock[manager.pTurn] % 10 == 0) StateMain();
        else
        {
            if (manager.buildCount[manager.curBlock[manager.pTurn]] == CurTower.tower03)
                uiManager.ChooseActive(false);
            else
                uiManager.ChooseActive(true);

            if (manager.pTurn == PlayTurn.player)
                uiManager.ShowUI();
            else
            {
                int money = manager.money[manager.pTurn];
                int buildMoney = manager.buildingPrice[manager.curBlock[manager.pTurn]];

                if (money >= buildMoney * 3)
                    manager.tower = CurTower.tower03;
                else if (money >= buildMoney * 2)
                    manager.tower = CurTower.tower02;
                else if (money >= buildMoney)
                    manager.tower = CurTower.tower01;
                //그리고 파산 신청 상황

                manager.UpdateState(GameState.Build);
            }
            uiManager.UndoImg();
        }
        //PayMoney();

    }

    private void PayMoney()
    {
        int price = manager.buildingPrice[manager.curBlock[manager.pTurn]];
        price *= (int)manager.tower;

        for (int i = 0; i < 4; i++)
            manager.DicMoney((PlayTurn)i, price);
    }

    private void StateMain()
    {
        manager.NextTurn(manager.pTurn);
        manager.UpdateState(GameState.Main);
    }
}
