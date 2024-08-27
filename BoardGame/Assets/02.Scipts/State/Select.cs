using BoardGame.Util;
using System;
using UnityEngine;

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
        if (StateMain())
        {
            manager.NextTurn();
            manager.UpdateState(GameState.Main);

            return;
        }

        SelectBuild();
        uiManager.UndoImg();

        if (manager.pTurn == PlayTurn.player)
        {
            int buildingPrice = manager.buildingPrice[manager.curBlock[manager.pTurn]];

            bool isPay = buildingPrice <= money;
            bool isBuy = buildingPrice * 2 <= money;
            bool isEmpty = manager.buildCount[manager.curBlock[manager.pTurn]] == CurTower.none;
            bool isActive = isPay ? isEmpty : isPay ? isBuy : isPay; //��

            UndoChoose(isActive, isPay);
            uiManager.ShowUI();
        }
        else
        {
            manager.UpdateState(GameState.Build);
        }

        manager.isChangeColor = false;
    }

    private void PayMoney()
    {
        int price = manager.buildingPrice[manager.curBlock[manager.pTurn]];
        price *= (int)manager.tower;

        for (int i = 0; i < 4; i++)
            manager.DicMoney((PlayTurn)i, price);
    } //Ȥ�� �𸣴� �α�

    private void SelectBuild()
    {
        CurTower tower = manager.buildCount[manager.curBlock[manager.pTurn]];

        buildMoney = manager.buildingPrice[manager.curBlock[manager.pTurn]];
        money = manager.money[manager.pTurn];
        Debug.Log($"({tower.GetType()}, {tower}), {buildMoney}, {money}");
        ExistBuilding(tower);
        NonExistBuilding(tower);
    }

    private void ExistBuilding(CurTower tower)
    {
        if (tower == CurTower.none) return;

        if ((int)tower * buildMoney * 2 <= money)
        {
            Debug.Log("�ǹ� ����");
            manager.tower = tower;
            uiManager.isBuyBuilding = true;
        }
        else if ((int)tower * buildMoney <= money)
        {
            Debug.Log("�����");
            manager.tower = tower;
        }
        else
        {
            Debug.Log("����");
            Bankruptcy();//�Ļ����� ��
        }
    }

    private void NonExistBuilding(CurTower tower)
    {
        if (tower != CurTower.none) return;
        Debug.Log("sdfghgfdsdfghjhgfdscvcbcvhxhjxr");
        for (int num = Enum.GetValues(typeof(CurTower)).Length - 1; num > 0; num--)
        {
            if ((CurTower)num == CurTower.none)
            {
                Debug.Log("�� �Ļ�");
                Bankruptcy();//�Ļ����� ��
            }

            if (money >= buildMoney * num)
            {
                manager.tower = (CurTower)num;
                Debug.Log("�ǹ� ��");
                Debug.Log($"�ǹ� ���� : {buildMoney}");
                break;
            }
        }
    }

    private void Bankruptcy()
    {
        Debug.Log("�Ļ�");
        //Enum�� ���Ÿ� �̿��ؼ� ������� Enum�������� �Ļ��� �÷��̾ ������.
        ObjectPool.instance.ReturnObject(PoolObjectType.player, manager.player[manager.pTurn]);
    }

    private void UndoChoose(bool value, bool active)
    {
        uiManager.ChooseActive(value, active);
    }

    private bool StateMain() => manager.curBlock[manager.pTurn] % 10 == 0;
}
