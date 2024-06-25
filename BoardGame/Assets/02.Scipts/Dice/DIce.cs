using BoardGame.Util;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : GameComponent
{
    GameManager manager;
    UiManager uiManager;

    private List<List<Transform>> diceChildList = new List<List<Transform>>();

    private GameObject White = null;
    private GameObject Red = null;

    private Vector3 rotationAmount;

    private float jumpDuration = 1f;
    private float rotDuration = 1f;
    private float jumpValue = 10f;
    private float rotValue = 600;
    private float jumpPower;
    private float rotPower;

    public Dice(GameManager game) : base(game) 
    {
        manager = GameManager.Instance;
        uiManager = UiManager.Instance;
    }

    protected override void OnMain()
    {
        base.OnMain();

        uiManager.DiceActive(true);

        //여기가 문제인거 같기도
        if (manager.pTurn != PlayTurn.player)
            uiManager.AiPlayerRoutine();

        if (Red != null || White != null) return;

        CreateDice();
        Reset();

        ChildObj(Red);
        ChildObj(White);
    }

    protected override void OnBuild()
    {
        base.OnBuild();

        DeleteDice();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (uiManager.isSpin)
            DiceAnim();
    }

    #region Set
    private void Reset()
    {
        diceChildList.Clear();

        Red.transform.position = UiManager.Instance.dicePoint.position - new Vector3(-3, 0, 3);
        White.transform.position = UiManager.Instance.dicePoint.position - new Vector3(3, 0, -3);
    }

    private void CreateDice()
    {
        Red = ObjectPool.instance.GetObject(PoolObjectType.redDice);
        White = ObjectPool.instance.GetObject(PoolObjectType.whiteDice);
    }

    private void DeleteDice()
    {
        ObjectPool.instance.ReturnObject(PoolObjectType.whiteDice, White);
        ObjectPool.instance.ReturnObject(PoolObjectType.redDice, Red);

        White = null;
        Red = null;
    }

    private void ChildObj(GameObject parent)
    {
        List<Transform> children = new List<Transform>();

        foreach (Transform child in parent.transform)
            children.Add(child);

        diceChildList.Add(children);
    }
    #endregion

    #region DICE
    private void DiceAnim()
    {

        GameObject[] obj = { Red, White };
        float[] value = { 800, 700 };

        ThrowDice(obj, value);
    }

    private void ValueAdjust(float value)
    {
        rotValue = value;

        float rPower = rotValue / 2;
        float jPower = jumpValue - 3;

        float saveRot = rPower / manager.GRADE;
        float saveJump = jPower / manager.GRADE;

        rotPower = rPower + saveRot * uiManager.grade;
        jumpPower = 3 + saveJump * uiManager.grade;
    }

    private void ThrowDice(GameObject[] dice, float[] value)
    {
        Sequence seq1 = DOTween.Sequence();
        Sequence seq2 = DOTween.Sequence();

        ValueAdjust(value[0]);
        DotAnim(dice[0].transform, seq1);

        ValueAdjust(value[1]);
        DotAnim(dice[1].transform, seq2);

        seq1.AppendInterval(2f).OnComplete(() => DiceCount());

        uiManager.isSpin = false;
    }

    private void DotAnim(Transform diceTrm, Sequence seq)
    {
        rotationAmount = new Vector3(rotPower, 0, rotPower);
        diceTrm.rotation = Quaternion.identity;

        seq.Append(diceTrm.DORotate(rotationAmount, rotDuration, RotateMode.FastBeyond360).SetEase(Ease.Linear))
            .Join(diceTrm.DOMoveY(diceTrm.position.y + jumpPower, jumpDuration).SetEase(Ease.OutQuad))
            .Append(diceTrm.DOMoveY(diceTrm.position.y, jumpDuration).SetEase(Ease.InQuad))
            .Join(diceTrm.DORotate(rotationAmount, rotDuration, RotateMode.FastBeyond360).SetEase(Ease.Linear))
            .SetLoops(1, LoopType.Restart);
    }

    private void DiceCount()
    {
        int count = 0;

        foreach (var list in diceChildList)
        {
            float highY = float.MinValue;
            int save = 0;

            foreach (var item in list)
            {
                if (highY < item.position.y)
                {
                    highY = item.position.y;
                    int.TryParse(item.name, out save);
                }
            }

            count += save;
        }

        uiManager.DiceActive(false);

        manager.jumpCount = count;
        manager.UpdateState(GameState.Move);
    }
    #endregion
}
