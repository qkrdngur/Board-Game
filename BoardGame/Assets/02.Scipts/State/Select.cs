using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoardGame.Util;
using System;

public class Select : GameComponent
{
    GameManager manager;
    UiManager uiManager;

    CurTower curTower;

    int buildMoney;
    int money;

    public Select(GameManager game) : base(game)
    {
        manager = GameManager.Instance;
        uiManager = UiManager.Instance;
    }

    protected override void OnSelect()
    {
        StateMain();

        if (manager.pTurn == PlayTurn.player)
        {
            UndoChoose(manager.buildCount[manager.curBlock[manager.pTurn]] == CurTower.none);
            uiManager.ShowUI();
        }
        else
        {
            //CurTower tower = manager.buildCount[manager.curBlock[manager.pTurn]];

            //int buildMoney = manager.buildingPrice[manager.curBlock[manager.pTurn]];
            //int money = manager.money[manager.pTurn];

            //if (money >= buildMoney * 3)
            //    manager.tower = CurTower.tower03;
            //else if (money >= buildMoney * 2)
            //    manager.tower = CurTower.tower02;
            //else if (money >= buildMoney)
            //    manager.tower = CurTower.tower01;

            //if ((int)tower * buildMoney <= money)
            //    manager.tower = tower;
            //else if ((int)tower * buildMoney * 2 <= money)
            //    uiManager.isBuyBuilding = true;
            //else
            //{
            //    //그리고 파산 신청 상황
            //}

            SelectBuild();
        }

        uiManager.UndoImg();
        manager.isChangeColor = false;
        //PayMoney();

    }

    private void PayMoney()
    {
        int price = manager.buildingPrice[manager.curBlock[manager.pTurn]];
        price *= (int)manager.tower;

        for (int i = 0; i < 4; i++)
            manager.DicMoney((PlayTurn)i, price);
    } //혹시 모르니 두기

    private void SelectBuild()
    {
        CurTower tower = manager.buildCount[manager.curBlock[manager.pTurn]];

        buildMoney = manager.buildingPrice[manager.curBlock[manager.pTurn]];
        money = manager.money[manager.pTurn];

        ExistBuilding(tower);
        NonExistBuilding(tower);
    }

    private void ExistBuilding(CurTower tower)
    {
        if (tower == CurTower.none) return;

        if ((int)tower * buildMoney * 2 <= money)
        {
            manager.tower = tower;
            uiManager.isBuyBuilding = true;
        }
        else if ((int)tower * buildMoney <= money)
        {
            manager.tower = tower;
        }
        else
        {
            Bankruptcy();//파산했을 때
        }

        manager.UpdateState(GameState.Build);
    }

    private void NonExistBuilding(CurTower tower)
    {
        if (tower != CurTower.none) return;

        for (int num = Enum.GetValues(typeof(CurTower)).Length - 1; num > 0; num--)
        {
            if ((CurTower)num == CurTower.none)
            {
                Bankruptcy();//파산했을 때
            }

            if (money >= buildMoney * num)
            {
                manager.tower = (CurTower)num;

                break;
            }
        }

        manager.UpdateState(GameState.Build);
    }

    private void Bankruptcy()
    {

    }

    private void UndoChoose(bool value)
    {
        uiManager.ChooseActive(value);
    }

    private void StateMain()
    {
        if (manager.curBlock[manager.pTurn] % 10 == 0)
        {
            manager.NextTurn(manager.pTurn);
            manager.UpdateState(GameState.Main);
        }
    }
}
