using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoardGame.Util;
using DG.Tweening;

public class DIce : GameComponent
{
    private float jumpDuration = 1f;
    private float rotDuration = 1f;
    private float jumpValue = 5f;
    private float rotValue = 600;
    private float jumpPower;
    private float rotPower;

    private Vector3 rotationAmount;

    private GameObject Red;
    private GameObject White;

    public DIce(GameManager game) : base(game)
    {
    }

    protected override void OnRunning()
    {
        base.OnRunning();

        Red = ObjectPool.instance.GetObject(PoolObjectType.RedDice);
        White = ObjectPool.instance.GetObject(PoolObjectType.WhiteDice);
        Red.transform.position = UiManager.Instance.dicePoint.position - new Vector3(-3, 0, 0);
        White.transform.position = UiManager.Instance.dicePoint.position - new Vector3(3, 0, 0);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (UiManager.Instance.isSpin)
            DiceAnim();
    }

    private void DiceAnim()
    {
        ThrowDice(Red, 700);
        ThrowDice(White, 600);
    }

    private void ThrowDice(GameObject dice, float value)
    {
        ValueAdjust(value);
        
        Transform diceTrm = dice.transform;
        Sequence sequence = DOTween.Sequence();

        rotationAmount = new Vector3(rotPower, 0, rotPower);

        diceTrm.rotation = Quaternion.identity;

        sequence.Append(diceTrm.DORotate(rotationAmount, rotDuration, RotateMode.FastBeyond360).SetEase(Ease.Linear))
            .Join(diceTrm.DOMoveY(diceTrm.position.y + jumpPower, jumpDuration).SetEase(Ease.OutQuad))
            .Append(diceTrm.DOMoveY(diceTrm.position.y, jumpDuration).SetEase(Ease.InQuad))
            .Join(diceTrm.DORotate(rotationAmount, rotDuration, RotateMode.FastBeyond360).SetEase(Ease.Linear))
            .SetLoops(1, LoopType.Restart)
            .OnComplete(() => GameManager.Instance.UpdateState(GameState.Main));

        UiManager.Instance.isSpin = false;
    }

    private void ValueAdjust(float value)
    {
        rotValue = value;

        float rPower = rotValue / 2;
        float jPower = jumpValue - 1;

        float saveRot = rPower / GameManager.Instance.GRADE;
        float saveJump = jPower / GameManager.Instance.GRADE;

        rotPower = rPower + saveRot * UiManager.Instance.grade;
        jumpPower = 1 + saveJump * UiManager.Instance.grade;
    }
}
