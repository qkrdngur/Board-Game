using BoardGame.Util;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [HideInInspector] public List<Transform> blockPos;
    [HideInInspector] public List<Transform> towerPos;
    [HideInInspector] public List<PlayTurn> blockRot;

    public CurTower tower;
    public PlayTurn pTurn;
    public GameState State;
    public BlockName blockSO;

    public List<GameObject> building;
    public Dictionary<PlayTurn, GameObject> player = new Dictionary<PlayTurn, GameObject>();
    public Dictionary<int, PlayTurn> BuildingOwner = new Dictionary<int, PlayTurn>();
    public Dictionary<int, CurTower> buildCount = new Dictionary<int, CurTower>();
    public Dictionary<PlayTurn, int> curBlock = new Dictionary<PlayTurn, int>();
    public Dictionary<PlayTurn, int> money = new Dictionary<PlayTurn, int>();


    public Dictionary<int, int> buildingPrice = new Dictionary<int, int>();
    public Dictionary<int, int> built = new Dictionary<int, int>();
    private readonly List<IGameComponent> _components = new();

    public readonly int GRADE = 12;
    public int jumpCount;

    private void Awake()
    {
        _components.Add(new SpawnBlock(this));
        _components.Add(new PlayerMovement(this));
        _components.Add(new Dice(this));
        _components.Add(new Build(this));
        _components.Add(new ResetValue(this));
    }

    void Start()
    {
        UpdateState(GameState.Init);
    }

    void Update()
    {
        if (State == GameState.Main)
            foreach (var component in _components)
                component.OnRoutine();
    }

    public void UpdateState(GameState state)
    {
        State = state;

        foreach (var component in _components)
            component.UpdateState(state);
    }

    public void NextTurn(PlayTurn turn)
    {
        int next = (int)++turn % 4;

        pTurn = (PlayTurn)next;
    }

    #region Build
    public void Build()
    {
        BuildingOwner[curBlock[pTurn]] = pTurn;
        buildCount[curBlock[pTurn]] = tower;

        BuildTower();
        CalcPrice(true);
    }

    private void BuildTower()
    {
        for (int i = 0; i < (int)tower; i++)
        {
            if (i < built[curBlock[pTurn]]) continue;

            GameObject obj = ObjectPool.instance.GetObject(PoolObjectType.Build1 + i);
            obj.transform.position = BuildingPos(i).position;
        }

        built[curBlock[pTurn]] = (int)buildCount[curBlock[pTurn]];
    }

    private Transform BuildingPos(int idx)
    {
        Transform child = blockPos[curBlock[pTurn]].GetChild(1);
        return child.GetChild(idx);
    }
    #endregion

    #region Price
    public void CalcPrice(bool value)
    {
        buildingPrice[curBlock[pTurn]] = blockSO.BuildPrice[curBlock[pTurn]];

        for (int i = 0; i <= (int)PlayTurn.TirAi; i++)
        {
            PlayTurn curTurn = (PlayTurn)i;

            int price = buildingPrice[curBlock[curTurn]];
            price *= (int)tower + 1;

            if (price == 0 || (pTurn == curTurn)) continue;

            if (BuildingOwner[curBlock[pTurn]] == curTurn)
            {
                if (built[curBlock[pTurn]] != 0 && value)
                    money[curTurn] += price;

                money[pTurn] -= price;
            }
        }
    }
    #endregion
}
