using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MSavioti.UnityExtensionMethods;

public class AverageEnemy : Enemy
{
    protected override Tween StartsMoving()
    {
        return ownTransform.DOMove(new Vector3(ownTransform.position.x, finalPos.y, finalPos.z), timeToReachBed, false);
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
