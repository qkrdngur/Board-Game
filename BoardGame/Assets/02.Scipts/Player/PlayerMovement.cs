using BoardGame.Util;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : GameComponent
{
    public float jumpPower = 1.5f;
    public float jumpDuration = 0.1f;

    private Dictionary<PlayTurn, int> JumpNum = new Dictionary<PlayTurn, int>();

    private GameObject player = null;
    private GameManager manager;

    private Vector3[] pos =
        {
            new Vector3(0.5f, 5, 0.5f),
            new Vector3(0.5f, 5, -0.5f),
            new Vector3(-0.5f, 5, 0.5f),
            new Vector3(-0.5f, 5, -0.5f)
        };

    public PlayerMovement(GameManager game) : base(game)
    {
        manager = GameManager.Instance;
    }

    protected override void OnRunning()
    {
        if (player != null) return;

        base.OnSetting();

        UiManager.Instance.playerUIActive(true);

        //나중에 ui로 게임 플레이 인원 정하여 인원만큼 for문 돌수있게
        for (int i = 0; i < 4; i++)
        {
            player = ObjectPool.instance.GetObject((PoolObjectType)(i + 3));
            player.transform.position += pos[i];

            JumpNum[(PlayTurn)i] = 0;
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
        manager.curBlock[manager.pTurn] = (JumpNum[manager.pTurn] % manager.blockPos.Count);
    }

    private void Jump()
    {
        Transform pTrm = manager.player[manager.pTurn].transform;
        Sequence seq = DOTween.Sequence();

        Vector3 jumpHigh = pos[(int)manager.pTurn];

        for (int i = 0; i < manager.jumpCount; i++)
            seq.Append(pTrm.DOJump(manager.blockPos[++JumpNum[manager.pTurn] % manager.blockPos.Count].position + jumpHigh, jumpPower, 1, jumpDuration)
                    .SetEase(Ease.OutQuad));

        seq.OnComplete(() => manager.UpdateState(GameState.Select));
    }
}
