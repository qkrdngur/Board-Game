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
    public Build(GameManager game) : base(game)
    {

    }

    protected override void OnSelect()
    {
        if ((GameManager.Instance.curBlock[GameManager.Instance.pTurn]) % 10 == 0) StateMain();

        UiManager.Instance.ShowUI();
        UiManager.Instance.UndoImg();
    }
    protected override void OnBuild()
    {
        if (Return()) return;

        GameManager.Instance.Build();
        StateMain();
    }

    private void StateMain()
    {
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