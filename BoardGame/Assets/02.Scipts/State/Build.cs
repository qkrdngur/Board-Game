using BoardGame.Util;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum CurTower
{
    none = 0,
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

    protected override void OnBuild()
    {
        if (Return()) return;

        manager.Build();
        StateMain();
    }

    private void StateMain()
    {
        manager.NextTurn(manager.pTurn);
        manager.UpdateState(GameState.Main);
    }

    private bool Return()
    {
        for (int i = 0; i <= (int)PlayTurn.TirAi; i++)
        {
            if (manager.curBlock[(PlayTurn)i] % 10 == 0 && manager.pTurn == (PlayTurn)i)
            {
                StateMain();

                return true;
            }
        }

        return false;
    }
}