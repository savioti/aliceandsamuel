using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MSavioti.UnityExtensionMethods;

public class Girl : MonoBehaviour
{
    public float chanceToCelebrate = 60f;
    public AudioClip[] happySounds;
    public AudioClip crySound;
    public Transform girlArm;
    public Transform closestEnemy;
    private AudioSource audioSource;
    private void Start() 
    {
        audioSource = GetComponent<AudioSource>();
        Enemy.OnDefeated += Celebrate;
        Enemy.OnReachedDestination += Cry;
    }
    private void OnDisable() 
    {
        Enemy.OnDefeated -= Celebrate;
    }
    private void Update()
    {
        FindClosestEnemy();
    }
    private void LateUpdate()
    {
        PointToEnemy();
    }
    private void FindClosestEnemy()
    {
        closestEnemy = GameManager.Instance.GetClosestEnemy();
    }
    private void PointToEnemy()
    {
        if (closestEnemy)
        {
            Vector3 rotation = Vector3.zero;
            rotation.z = Mathf.Atan2(closestEnemy.position.y - girlArm.position.y,
                closestEnemy.position.x - girlArm.position.x) * Mathf.Rad2Deg;

            girlArm.rotation = Quaternion.Euler(rotation);
        }
    }
    private void Celebrate()
    {
        if (Random.Range(0, 100) > chanceToCelebrate) return;
        audioSource.ForcePlay(happySounds[Random.Range(0, happySounds.Length)]);
    }
    private void Cry()
    {
        audioSource.ForcePlay(crySound);
    }
}
