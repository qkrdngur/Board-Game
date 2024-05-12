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
            if (manager.buildCount[manager.curBlock[manager.pTurn]] != CurTower.none)
                uiManager.ChooseActive(false);
            else
                uiManager.ChooseActive(true);

            if (manager.pTurn == PlayTurn.player)
                uiManager.ShowUI();
            else
            {
                CurTower tower = manager.buildCount[manager.curBlock[manager.pTurn]];
                int buildMoney = manager.buildingPrice[manager.curBlock[manager.pTurn]];
                int money = manager.money[manager.pTurn];

                if ((int)tower * buildMoney <= money)
                    manager.tower = tower;
                else if((int)tower * buildMoney * 2 <= money)
                    uiManager.isBuyBuilding = true;
                else
                {
                    //그리고 파산 신청 상황
                }

                manager.UpdateState(GameState.Build);
            }

            uiManager.UndoImg();
            manager.isChangeColor = false;
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
