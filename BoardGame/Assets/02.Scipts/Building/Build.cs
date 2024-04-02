using BoardGame.Util;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum CurTower
{
    tower01,
    tower02,
    tower03
}

public class Build : GameComponent
{
    GameManager manager;

    public Build(GameManager game) : base(game)
    {
        manager = GameManager.Instance;
    }

    protected override void OnSelect()
    {
        if ((manager.curBlock[manager.pTurn]) % 10 == 0) StateMain();

        Debug.Log(GameManager.Instance.pTurn);
        UiManager.Instance.ShowUI();
        UiManager.Instance.UndoImg();
    }

    protected override void OnTakeOver()
    {
        manager.CalcPrice(false);

        manager.BuildingOwner[manager.curBlock[manager.pTurn]] = manager.pTurn;
    }

    protected override void OnBuild()
    {
        if (Return()) return;

        GameManager.Instance.Build();
        StateMain();
    }

    private void StateMain()
    {
        GameManager.Instance.NextTurn(GameManager.Instance.pTurn);
        GameManager.Instance.UpdateState(GameState.Main);
    }

    private bool Return()
    {
        for (int i = 0; i <= (int)PlayTurn.TirAi; i++)
        {
            if ((GameManager.Instance.curBlock[(PlayTurn)i]) % 10 == 0/*&& GameManager.instance.pTurn == (PlayTurn)i*/)
            {
                StateMain();

                return true;
            }
        }

        return false;
    }
}