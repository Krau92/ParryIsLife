using UnityEngine;
using System.Collections;

public class Tongue : MonoBehaviour
{
    Transform player;
    public Transform shootPoint;
    public float tongueSpeed;
    public float tongueStopTime;
    public float rotationSpeed;
    Collider2D col;
    bool isSeeking;
    bool isAttacking;
    public GameObject seekingMark;
    ShootingPatternSO pattern;
    NewTestBullet bullet;
    Quaternion initialRot;
    Vector3 initialScale;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        col = GetComponent<Collider2D>();
        initialRot = transform.rotation;
        initialScale = transform.localScale;
        if(seekingMark != null)
        {
            seekingMark.SetActive(false);
        }
    }
    void Update()
    {
        if(isSeeking)
        {
            Quaternion targetRot = Quaternion.LookRotation(Vector3.forward, player.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            if(seekingMark != null)
            {
                seekingMark.transform.position = player.position;
            }
        }
        if(isAttacking)
        {
            
            Quaternion targetRot = Quaternion.LookRotation(Vector3.forward, seekingMark.transform.position - transform.position);
            transform.rotation = targetRot;
            transform.localScale += Vector3.up  * tongueSpeed;
            if(shootPoint.position.y <= seekingMark.transform.position.y)
            {
                StartCoroutine(pattern.Shoot(shootPoint.transform.position, player.position, bullet));
                StopAttacking();
            }
        }
    }

    public void StartSeeking()
    {
        isSeeking = true;
        if(seekingMark != null)
        {
            seekingMark.transform.position = player.position;
            seekingMark.SetActive(true);
        }
    }

    public void StopSeeking()
    {
        isSeeking = false;
        Quaternion targetRot = Quaternion.LookRotation(Vector3.forward, player.position - transform.position);
        transform.rotation = targetRot;
    }

    public void EnableCollider()
    {
        col.enabled = true;
    }

    public void Attack(ShootingPatternSO pattern, NewTestBullet bullet)
    {
        isAttacking = true;
        this.pattern = pattern;
        this.bullet = bullet;
    }

    public void StopAttacking()
    {
        isAttacking = false;
        col.enabled = false;
        if(seekingMark != null)
        {
            seekingMark.SetActive(false);
        }
        StartCoroutine(ResetTongue());
    }

    IEnumerator ResetTongue()
    {
        yield return new WaitForSeconds(tongueStopTime);
        while(transform.localScale.y > initialScale.y)
        {
            transform.localScale -= Vector3.up * tongueSpeed;
            yield return null;
        }
        while(transform.rotation != initialRot)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, initialRot, rotationSpeed * Time.deltaTime);
            yield return null;
        }
        Vector3 scale = transform.localScale;
        transform.localScale = new Vector3(scale.x, initialScale.y, scale.z);
    }
}
