using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MSavioti.UnityExtensionMethods;

public class TankEnemyTutorial : Enemy
{
    public Dialogue tutorialDialogueEng;
    public Dialogue tutorialDialoguePTBR;
    public AudioClip damageSound;
    public int health = 3;
    public float knockbackStrength = 1f;
    public float minXPos = -2.5f;
    public float maxXPos = 2.5f;
    public float maxYPos = 3f;
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

        health--;
        if (health == 2)
        {
            FindObjectOfType<DialogueManager>().PlayUnscriptedDialogue(
                DataController.Instance.GetIsEnglish() ? tutorialDialogueEng : tutorialDialoguePTBR);
        }

        Vector3 newPos = new Vector3(Random.Range(minXPos, maxXPos), 
            ownTransform.position.y + knockbackStrength, 
                ownTransform.position.z + knockbackStrength);

        if (newPos.y > maxYPos) newPos.y = maxYPos;
        if (newPos.z > 0) newPos.z = 0;

        if (health > 0)
        {
            audioSource.ForcePlay(damageSound);
            Restore(newPos);
        }
        else
        {
            audioSource.ForcePlay(deathSound);
            Invoke(nameof(OnDeath), timeUntilVanish);
        }
    }
}
