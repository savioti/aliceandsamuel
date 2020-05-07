using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MSavioti.UnityExtensionMethods;

public class Boy : MonoBehaviour
{
    #region Variables
    [Header("gameplay values")]
    public float shootingCooldown = 0.75f;
    public float shotImpactDelay = 0.15f;
    public float castRadius = 0.25f;
    public float leftArmAngleLimit = 130f;
    public float rightArmAngleLimit = 30f;
    public string enemyLayerName = "Enemy";
    [Header("References")]
    public AudioClip[] pistolSounds;
    public Transform rightArm;
    public Transform leftArm;
    public SpriteRenderer restingRightArm;
    public SpriteRenderer restingLeftArm;
    public SpriteRenderer activeRightArmUpper;
    public SpriteRenderer activeRightArmLower;
    public SpriteRenderer activeLeftArmUpper;
    public SpriteRenderer activeLeftArmLower;
    public Transform rightHand;
    public Transform leftHand;
    public ParticleSystem shotParticles;
    private Transform particlesTransform;
    private Transform currentArm;
    private Camera mainCam;
    private AudioSource audioSource;
    private bool canShoot = true;
    private int enemyDetectMask;
    private Transform ownTransform;
    #endregion
    private void Awake() 
    {
        audioSource = GetComponent<AudioSource>();
        ownTransform = transform;
        particlesTransform = shotParticles.transform;
        enemyDetectMask = 1 << LayerMask.NameToLayer(enemyLayerName);
    }
    private void Start() 
    {
        DialogueManager.OnDialogueStart += CeaseShooting;
        DialogueManager.OnDialogueEnd += AllowShooting;
        GameManager.OnGameOver += CeaseShooting;
        mainCam = Camera.main;
        leftArm.gameObject.SetActive(false);
        rightArm.gameObject.SetActive(false);
        restingLeftArm.enabled = true;
        restingRightArm.enabled = true;
    }
    private void Update() 
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Shoot());
        }    
    }
    private void OnDisable() 
    {
        DialogueManager.OnDialogueStart -= CeaseShooting;
        DialogueManager.OnDialogueEnd -= AllowShooting;
        GameManager.OnGameOver -= CeaseShooting;
    }
    public IEnumerator Shoot()
    {
        if (canShoot)
        {
            // enemyTransform = GameManager.Instance.GetClosestEnemy();
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = mainCam.transform.position.z;
            mousePos = mainCam.ScreenToWorldPoint(mousePos);

            DisablePointingArms();

            if (mousePos.x > ownTransform.position.x)
            {
                rightArm.gameObject.SetActive(true);
                leftArm.gameObject.SetActive(false);
                restingRightArm.enabled = false;
                restingLeftArm.enabled = true;
                currentArm = rightArm;

                if (PointArmToTarget(currentArm, mousePos) < rightArmAngleLimit)
                    activeRightArmLower.gameObject.SetActive(true);
                else 
                    activeRightArmUpper.gameObject.SetActive(true);

                particlesTransform.position = rightHand.transform.position;
            }
            else
            {
                rightArm.gameObject.SetActive(false);
                leftArm.gameObject.SetActive(true);
                restingRightArm.enabled = true;
                restingLeftArm.enabled = false;
                currentArm = leftArm;
                
                if (PointArmToTarget(currentArm, mousePos) > leftArmAngleLimit)
                    activeLeftArmLower.gameObject.SetActive(true);
                else
                    activeLeftArmUpper.gameObject.SetActive(true);

                particlesTransform.position = leftHand.transform.position;
            }

            particlesTransform.LookAt(mousePos, Vector3.up);
            Collider2D collider = Physics2D.OverlapCircle(mousePos, castRadius, enemyDetectMask);
            bool enemyDetected = false;

            audioSource.ForcePlay(pistolSounds[Random.Range(0, pistolSounds.Length)]);

            if (collider)
                enemyDetected = true;

            shotParticles.Play();
            StartCoroutine(CooldownShooting());

            if (enemyDetected)
            {
                yield return new WaitForSeconds(shotImpactDelay);
                collider.GetComponent<Enemy>().OnHit();
            }
        }
    }
    private void DisablePointingArms()
    {
        activeRightArmUpper.gameObject.SetActive(false);
        activeRightArmLower.gameObject.SetActive(false);
        activeLeftArmUpper.gameObject.SetActive(false);
        activeLeftArmLower.gameObject.SetActive(false);
    }
    public float PointArmToTarget(Transform arm, Vector3 tgtPos)
    {
        Vector3 rotation = Vector3.zero;
        float angle = Mathf.Atan2(tgtPos.y - arm.position.y, tgtPos.x - arm.position.x) * Mathf.Rad2Deg;
        rotation.z = angle; 
        arm.rotation = Quaternion.Euler(rotation);
        return angle;
    }
    public void AllowShooting()
    {
        canShoot = true;
    }
    public void CeaseShooting()
    {
        canShoot = false;
    }
    public IEnumerator CooldownShooting()
    {
        CeaseShooting();
        yield return new WaitForSeconds(shootingCooldown);
        AllowShooting();
    }
}
