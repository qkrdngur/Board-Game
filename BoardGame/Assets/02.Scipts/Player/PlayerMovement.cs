using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using BoardGame.Util;

public class PlayerMovement : GameComponent
{
    public float jumpPower = 2f;
    public float jumpDuration = 0.5f;

    private bool isJumping = false;
    private int jumpNum = 0;

    public PlayerMovement(GameManager game) : base(game)
    {

    }

    protected override void OnRunning()
    {
        base.OnSetting();

        GameObject player = ObjectPool.instance.GetObject(PoolObjectType.Player);

        player.transform.position += new Vector3(0, 5, 0);

        GameManager.Instance.player.Add("player", player);
    }

    //protected override void OnRunning()
    //{
    //}

    protected override void OnUpdate()
    {
        base.OnUpdate();

        // 점프 입력을 받으면
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            Jump();
        }
    }

    void Jump()
    {
        Transform pTrm;
        int jumpCount;

        isJumping = true;

        pTrm = GameManager.Instance.player["player"].transform;
        jumpCount = jumpNum++ % GameManager.Instance.blockPos.Count;

        pTrm.DOJump(GameManager.Instance.blockPos[jumpCount].position + new Vector3(0, 3, 0), jumpPower, 1, jumpDuration)
                .SetEase(Ease.OutQuad)
                .SetLoops(1, LoopType.Restart)
                .OnComplete(() => isJumping = false);
    }
}
