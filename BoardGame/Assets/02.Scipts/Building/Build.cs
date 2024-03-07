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

    protected override void OnBuild()
    {
        if (Return()) return;

        GameObject obj = ObjectPool.instance.GetObject(PoolObjectType.Build1);
        Debug.Log(GameManager.Instance.curBlock[PlayTurn.player]);
        obj.transform.position = GameManager.Instance.blockPos[GameManager.Instance.curBlock[GameManager.Instance.pTurn]].position;

        GameManager.Instance.BuildingOwner[GameManager.Instance.curBlock[GameManager.Instance.pTurn]] = GameManager.Instance.pTurn;

        Transform child = GameManager.Instance.blockPos[GameManager.Instance.curBlock[GameManager.Instance.pTurn]].GetChild(0);
        child.GetComponent<TextMeshPro>().text = "ssss";

        GameManager.Instance.buildCount[GameManager.Instance.curBlock[GameManager.Instance.pTurn]] = CurTower.tower01;

        GameManager.Instance.UpdateState(GameState.Main);
    }

    private bool Return()
    {
        for (int i = 0; i <= (int)PlayTurn.TirAi; i++)
        {
            if ((GameManager.Instance.curBlock[(PlayTurn)i]) % 10 == 0/*&& GameManager.instance.pTurn == (PlayTurn)i*/)
            {
                GameManager.Instance.UpdateState(GameState.Main);
                return true;
            }
        }

        return false;
    }
}