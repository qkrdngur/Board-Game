using BoardGame.Util;
using System.Collections.Generic;
using UnityEngine;

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
        ReSet();

        int count = GameManager.Instance.blockSO.RegionName.Count;

        for (int i = 0; i < count; i++)
        {
            string regionName = GameManager.Instance.blockSO.RegionName[i];
            int price = GameManager.Instance.blockSO.BuildPrice[i];

            buildCount[regionName] = 0;
            buildPrice[regionName] = price;
        }
    }

    private void ReSet()
    {
        for (int i = 0; i <= (int)PlayTurn.TirAi; i++)
            GameManager.Instance.curBlock[(PlayTurn)i] = 1;
    }

    protected override void OnBuild()
    {
        if (Return()) return;

        GameObject obj = ObjectPool.instance.GetObject(PoolObjectType.Build1);
        Debug.Log(GameManager.Instance.curBlock[PlayTurn.player]);
        obj.transform.position = GameManager.Instance.blockPos[GameManager.Instance.curBlock[PlayTurn.player]].position;

        GameManager.Instance.BuildingOwner[GameManager.Instance.curBlock[PlayTurn.player]] = PlayTurn.player;

        GameManager.Instance.UpdateState(GameState.Main);
    }

    private bool Return()
    {
        for (int i = 0; i <= (int)PlayTurn.TirAi; i++)
        {
            if ((GameManager.Instance.curBlock[(PlayTurn)i]) % 10 == 0/*&& GameManager.instance.pTurn == (PlayTurn)i*/)
            {
                Debug.Log("ssssssss");
                GameManager.Instance.UpdateState(GameState.Main);
                return true;
            }
        }

        return false;
    }
}