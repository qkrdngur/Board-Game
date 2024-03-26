using BoardGame.Util;
using DG.Tweening;
using UnityEngine;

public class PlayerMovement : GameComponent
{
    public float jumpPower = 1.5f;
    public float jumpDuration = 0.1f;

    private int jumpNum = 0;

    private GameObject player = null;
    private GameManager manager;

    public PlayerMovement(GameManager game) : base(game)
    {

    }

    protected override void Initialize()
    {
        base.Initialize();

        manager = GameManager.Instance;
    }

    protected override void OnRunning()
    {
        if (player != null) return;

        base.OnSetting();

        Vector3[] pos =
        {
            new Vector3(0, 5, 0),
            new Vector3(1, 5, 0),
            new Vector3(0, 5, 1),
            new Vector3(1, 5, 1)
        };

        //나중에 ui로 게임 플레이 인원 정하여 인원만큼 for문 돌수있게
        for (int i = 0; i < 4; i++)
        {
            player = ObjectPool.instance.GetObject((PoolObjectType)(i + 3));
            player.transform.position += pos[i];

            manager.player.Add((PlayTurn)i, player);
        }
    }

    protected override void OnMove()
    {
        base.OnMove();

        Jump();
        CurentBlock();
    }

    private void CurentBlock()
    {
        manager.curBlock[PlayTurn.player] = (jumpNum % manager.blockPos.Count);
    }

    private void Jump()
    {
        Transform pTrm = manager.player[manager.pTurn].transform;
        Sequence seq = DOTween.Sequence();

        Vector3 jumpHigh = new Vector3(0, 3, 0);

        for (int i = 0; i < manager.jumpCount; i++)
            seq.Append(pTrm.DOJump(manager.blockPos[++jumpNum % manager.blockPos.Count].position + jumpHigh, jumpPower, 1, jumpDuration)
                    .SetEase(Ease.OutQuad));

        seq.OnComplete(() => manager.UpdateState(GameState.Select));
    }
}
