using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MSavioti.UnityExtensionMethods;

public class NimbleEnemy : Enemy
{
    public Vector3[] pathA;
    public Vector3[] pathB;
    protected override Tween StartsMoving()
    {
        if (Random.Range(0, 2) > 0)
        {
            ownTransform.position = pathA[0];
            return ownTransform.DOPath(pathA, timeToReachBed);
        }
        else
        {
            ownTransform.position = pathB[0];
            return ownTransform.DOPath(pathB, timeToReachBed);
        }
    }
    protected override Tween StartsResizing()
    {
        return ownTransform.DOScale(finalScale, timeToReachBed);
    }
    public override void OnHit()
    {
        base.OnHit();
        audioSource.ForcePlay(deathSound);
        Invoke(nameof(OnDeath), timeUntilVanish);
    }
}
