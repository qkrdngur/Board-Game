using BoardGame.Util;
using DG.Tweening;
using UnityEngine;

public class PlayerMovement : GameComponent
{
    public float jumpPower = 1.5f;
    public float jumpDuration = 0.1f;

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

        GameManager.Instance.player.Add(PlayTurn.player, player);
    }

    protected override void OnMove()
    {
        base.OnMove();

        Jump();
        CurentBlock();
    }

    private void CurentBlock()
    {
        GameManager.Instance.curBlock[PlayTurn.player] = (jumpNum % GameManager.Instance.blockPos.Count);
    }

    private void Jump()
    {
        Transform pTrm = GameManager.Instance.player[PlayTurn.player].transform;
        Sequence seq = DOTween.Sequence();

        for (int i = 0; i < GameManager.Instance.jumpCount; i++)
            seq.Append(pTrm.DOJump(GameManager.Instance.blockPos[++jumpNum % GameManager.Instance.blockPos.Count].position + new Vector3(0, 3, 0), jumpPower, 1, jumpDuration)
                    .SetEase(Ease.OutQuad));

        seq.OnComplete(() => GameManager.Instance.UpdateState(GameState.Select));
    }
}
