using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using BoardGame.Util;

public class PlayerMovement : GameComponent
{
    public float jumpPower = 2f;
    public float jumpDuration = 0.5f;

    private int jumpNum = 0;

    private GameObject player = null;

    public PlayerMovement(GameManager game) : base(game)
    {

    }

    protected override void OnRunning()
    {
        if (player != null) return;

        base.OnSetting();

        player = ObjectPool.instance.GetObject(PoolObjectType.Player);

        player.transform.position += new Vector3(0, 5, 0);

        GameManager.Instance.player.Add("player", player);
    }

    protected override void OnMove()
    {
        base.OnMove();
        Jump();
    }

    private void Jump()
    {
        Transform pTrm = GameManager.Instance.player["player"].transform;
        Debug.Log(GameManager.Instance.jumpCount);
        Sequence seq = DOTween.Sequence();
        for (int i = 0; i < GameManager.Instance.jumpCount; i++)
            seq.Append(pTrm.DOJump(GameManager.Instance.blockPos[jumpNum++ % GameManager.Instance.blockPos.Count].position + new Vector3(0, 3, 0), jumpPower, 1, jumpDuration)
                    .SetEase(Ease.OutQuad));

        seq.OnComplete(() => GameManager.Instance.UpdateState(GameState.Main));
    }
}
