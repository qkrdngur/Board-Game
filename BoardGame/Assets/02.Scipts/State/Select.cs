using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoardGame.Util;
using System;

public class Select : GameComponent
{
    GameManager manager;
    UiManager uiManager;

    int buildMoney;
    int money;

    public Select(GameManager game) : base(game)
    {
        manager = GameManager.Instance;
        uiManager = UiManager.Instance;
    }

    protected override void OnSelect()
    {
        if (StateMain()) return;

        SelectBuild();
        uiManager.UndoImg();

        if (manager.pTurn == PlayTurn.player)
        {
            int buildingPrice = manager.buildingPrice[manager.curBlock[manager.pTurn]];

            bool isPay = buildingPrice <= money;
            bool isBuy = buildingPrice * 2 <= money;
            bool isEmpty = manager.buildCount[manager.curBlock[manager.pTurn]] == CurTower.none;
            bool isActive = isPay ? isEmpty : isPay ? isBuy : isPay; //건

            UndoChoose(isActive, isPay);
            uiManager.ShowUI();
        }
        else
        {
            manager.UpdateState(GameState.Build);
        }

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
            Debug.Log("없다");
            Bankruptcy();//파산했을 때
        }
    }

    private void NonExistBuilding(CurTower tower)
    {
        if (tower != CurTower.none) return;

        for (int num = Enum.GetValues(typeof(CurTower)).Length - 1; num > 0; num--)
        {
            if ((CurTower)num == CurTower.none)
            {
                Debug.Log("나 파산");
                Bankruptcy();//파산했을 때
            }

            if (money >= buildMoney * num)
            {
                manager.tower = (CurTower)num;

                break;
            }
        }
    }

    private void Bankruptcy()
    {
        //Enum값 제거를 이용해서 만들어진 Enum변수에서 파산한 플레이어를 없엔다.
        ObjectPool.instance.ReturnObject(PoolObjectType.player, manager.player[manager.pTurn]);
    }

    private void UndoChoose(bool value, bool active)
    {
        uiManager.ChooseActive(value, active);
    }

    private bool StateMain()
    {
        bool isReturn = manager.curBlock[manager.pTurn] % 10 == 0;

        if (isReturn)
        {
            manager.NextTurn(manager.pTurn);
            manager.UpdateState(GameState.Main);
        }

        return isReturn;
    }
}
