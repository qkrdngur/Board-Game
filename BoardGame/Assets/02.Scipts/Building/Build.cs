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
    private Dictionary<int, int> buildPrice = new Dictionary<int, int>();

    public Build(GameManager game) : base(game)
    {

    }

    protected override void Initialize()
    {
        base.Initialize();
        ReSet();

        int count = GameManager.Instance.blockSO.RegionName.Count;

        for (int i = 0; i < count; i++)
        {
            int price = GameManager.Instance.blockSO.BuildPrice[i];

            GameManager.Instance.buildCount[i] = 0;
            buildPrice[i] = price;
        }
    }

    private void ReSet()
    {
        for (int i = 0; i <= (int)PlayTurn.TirAi; i++)
            GameManager.Instance.curBlock[(PlayTurn)i] = 1;
    }

    private void StateMain()
    {
        GameManager.Instance.UpdateState(GameState.Main);
    }

    protected override void OnBuild()
    {
        if (Return()) return;

        GameManager.Instance.Build();
        StateMain();
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