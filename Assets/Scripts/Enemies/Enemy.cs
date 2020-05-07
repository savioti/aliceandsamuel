using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MSavioti.UnityExtensionMethods;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class Enemy : MonoBehaviour
{
    [Header("Gameplay values")]
    public float timeToReachBed = 6f;
    public float timeUntilVanish = 1f;
    private Vector3 initialPos;
    private Vector3 initialScale;
    public Vector3 finalPos;
    public Vector3 finalScale;
    [Header("References")]
    public AudioClip spawnSound;
    public AudioClip deathSound;
    private SpriteRenderer spriteRenderer;
    private Collider2D mainCollider;
    public Transform ownTransform {get; private set;}
    private ParticleSystem impactParticles;
    public AudioSource audioSource {get; private set;}
    private Tween movingTween;
    private Tween scalingTween;
    public static System.Action OnReachedDestination;
    public static System.Action OnDefeated;
    private void Awake() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ownTransform = transform;
        mainCollider = GetComponent<Collider2D>();
        impactParticles = GetComponentInChildren<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        initialPos = transform.position;
        initialScale = transform.localScale;
    }
    private void OnEnable()
    {
        spriteRenderer.color = Color.white;
        spriteRenderer.enabled = false;
        mainCollider.enabled = true;
        movingTween = StartsMoving().SetEase(Ease.Linear);
        movingTween.OnComplete(ReachDestination);
        scalingTween = StartsResizing().SetEase(Ease.Linear);
        transform.localScale = initialScale;
        audioSource.ForcePlay(spawnSound, 0.35f);
    }
    private void OnDisable()
    {
        movingTween.Kill();
        scalingTween.Kill();
    }
    public virtual void OnHit()
    {
        impactParticles.ForcePlay();
        movingTween.Kill();
        scalingTween.Kill();
        ownTransform.DOShakePosition(0.25f, 0.5f, 5);
        spriteRenderer.enabled = true;
        mainCollider.enabled = false;
    }
    private void ReachDestination()
    {
        spriteRenderer.enabled = true;
        audioSource.ForcePlay(spawnSound);
        OnReachedDestination();
    }
    public void OnDeath()
    {
        if (OnDefeated != null) OnDefeated();
        StartCoroutine(FadeColor());
    }
    public void Restore(Vector3 newPos)
    {
        StartCoroutine(RestoreRoutine(newPos));
    }
    private IEnumerator RestoreRoutine(Vector3 newPos)
    {
        yield return new WaitForSeconds(timeUntilVanish);
        ownTransform.position = newPos;
        spriteRenderer.enabled = false;
        mainCollider.enabled = true;
        movingTween = StartsMoving().SetEase(Ease.Linear);
        movingTween.OnComplete(() => OnReachedDestination());
        scalingTween = StartsResizing().SetEase(Ease.Linear);
    }
    private IEnumerator FadeColor()
    {
        Color color = Color.white;
        int i = 0;

        while (i < 20)
        {
            color.a -= 0.05f;
            spriteRenderer.color = color;
            i++;
            yield return new WaitForSeconds(0.05f);
        }
        
        spriteRenderer.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    protected abstract Tween StartsMoving();
    protected abstract Tween StartsResizing();
}
