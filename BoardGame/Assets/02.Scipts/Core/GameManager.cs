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
    public PlayerSO playerSO;

    [SerializeField] private List<Material> materials;

    public Dictionary<PlayTurn, GameObject> player        { get; set; } = new Dictionary<PlayTurn, GameObject>();
    public Dictionary<int, PlayMoney> BuildingOwner       { get; set; } = new Dictionary<int, PlayMoney>();
    public Dictionary<int, CurTower> buildCount           { get; set; } = new Dictionary<int, CurTower>();
    public Dictionary<int, List<GameObject>> curTower     { get; set; } = new Dictionary<int, List<GameObject>>();
    public Dictionary<PlayTurn, int> curBlock             { get; set; } = new Dictionary<PlayTurn, int>();
    public Dictionary<PlayTurn, int> money                { get; set; } = new Dictionary<PlayTurn, int>();

    public Dictionary<int, int> buildingPrice             { get; set; } = new Dictionary<int, int>();
    public Dictionary<int, int> built                     { get; set; } = new Dictionary<int, int>();

    private readonly List<IGameComponent> _components = new();

    public readonly int GRADE = 12;
    public int jumpCount { get; set; }

    private bool _buyBuilding = false;

    private void Awake()
    {
        _components.Add(new SpawnBlock(this));
        _components.Add(new PlayerMovement(this));
        _components.Add(new Dice(this));
        _components.Add(new Select(this));
        _components.Add(new Build(this));
        _components.Add(new GameTurn(this));
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
        buildCount[curBlock[pTurn]] = tower;

        BuildTower();
        CalcPrice(true);
        BuyBuilding();
    }

    private void BuyBuilding()
    {
        if (_buyBuilding)
        {
            BuildingOwner[curBlock[pTurn]] = (PlayMoney)pTurn;

            print(BuildingOwner[curBlock[pTurn]]);
        }
    }

    private void BuildTower()
    {
        for (int i = 0; i < (int)tower; i++)
        {
            if (i < built[curBlock[pTurn]])
            {
                curTower[curBlock[pTurn]][i].GetComponent<MeshRenderer>().material = materials[(int)pTurn];
                continue;
            }

            GameObject obj = ObjectPool.instance.GetObject(PoolObjectType.Build1 + i);
            curTower[curBlock[pTurn]].Add(obj);

            obj.GetComponent<MeshRenderer>().material = materials[(int)pTurn];
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
        for (int i = 0; i <= (int)PlayTurn.TirAi; i++)
        {
            PlayTurn curTurn = (PlayTurn)i;

            int price = buildingPrice[curBlock[pTurn]];
            price *= (int)buildCount[curBlock[pTurn]];

            if (price == 0 || (pTurn == curTurn)) continue;

            if (BuildingOwner[curBlock[pTurn]] == PlayMoney.None)
            {
                _buyBuilding = true;
                money[pTurn] -= price;

                break;
            }
            else
                DicMoney(curTurn, price, value);
        }
    }

    public void DicMoney(PlayTurn turn, int price, bool value)
    {
        if (BuildingOwner[curBlock[pTurn]] != (PlayMoney)turn) return;

        _buyBuilding = false; // 건물을 구매하지 않았을 때

        if (built[curBlock[pTurn]] != 0 && value)
        {
            //_buyBuilding = true; // 건물을 구매하였을 때만 owner가 되게
            money[turn] += price;
        }

        money[pTurn] -= price;
        print(money[pTurn]);
    }
    #endregion
}
