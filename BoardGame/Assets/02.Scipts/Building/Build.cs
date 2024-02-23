using UnityEngine;
using BoardGame.Util;
using System.Collections.Generic;

public class Build : GameComponent
{
    private Dictionary<string, int> buildCount = new Dictionary<string, int>();
    private Dictionary<string, int> buildPrice = new Dictionary<string, int>();

    public Build(GameManager game) : base(game)
    {

    }

    protected override void Initialize()
    {
        base.Initialize();

        int count = GameManager.Instance.blockSO.RegionName.Count;

        for (int i = 0; i < count; i++)
        {
            string regionName = GameManager.Instance.blockSO.RegionName[i];
            int price = GameManager.Instance.blockSO.BuildPrice[i];

            buildCount[regionName] = 0;
            buildPrice[regionName] = price;
        }
    }

    protected override void OnBuild()
    {
        ///if ((GameManager.Instance.blockPos.Count + 1) % 10 == 0)

        GameManager.Instance.UpdateState(GameState.Main);
    }
}