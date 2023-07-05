using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraAnimation : MonoBehaviour
{

    public void MoveUp()
    {
        transform.DOLocalMoveY(2f, 3.5f).SetEase(Ease.Linear).SetRelative(true).SetId("MOVEUP");
    }
    public void MoveLeft()
    {
        transform.DOLocalMoveX(-4f, 2f).SetEase(Ease.Linear).SetRelative(true).SetId("MOVELEFT");
    }
    public void MoveRight()
    {
        transform.DOLocalMoveX(4f, 2f).SetEase(Ease.Linear).SetRelative(true).SetId("MOVERIGHT");
    }
    public void MoveDown()
    {
        transform.DOLocalMoveY(-2f, 3.5f).SetEase(Ease.Linear).SetRelative(true).SetId("MOVEDOWN");
    }

    public void KillAnim()
    {
        DOTween.Kill("MOVEUP");
        DOTween.Kill("MOVEDOWN");
        DOTween.Kill("MOVERIGHT");
        DOTween.Kill("MOVELEFT");

    }
}
