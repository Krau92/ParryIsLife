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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        col = GetComponent<Collider2D>();
        initialRot = transform.rotation;
        seekingMark.SetActive(false);
    }
    void Update()
    {
        if(isSeeking)
        {
            Quaternion targetRot = Quaternion.LookRotation(Vector3.forward, player.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            seekingMark.transform.position = player.position;
        }
        if(isAttacking)
        {
            transform.localScale += Vector3.up  * tongueSpeed;
            Debug.Log(shootPoint.position.y + " " + seekingMark.transform.position.y);
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
        seekingMark.SetActive(true);
        Debug.Log("Started seeking with tongue");
    }

    public void StopSeeking()
    {
        isSeeking = false;
        Debug.Log("Stopped seeking with tongue");
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
        Debug.Log("Started attacking with tongue");
    }

    public void StopAttacking()
    {
        isAttacking = false;
        Debug.Log("Stopped attacking with tongue");
        col.enabled = false;
        seekingMark.SetActive(false);
        StartCoroutine(ResetTongue());
    }

    IEnumerator ResetTongue()
    {
        yield return new WaitForSeconds(tongueStopTime);
        while(transform.localScale.y > 1)
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
        transform.localScale = new Vector3(scale.x, 1, scale.z);
    }
}
