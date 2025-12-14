using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class TestBullet : MonoBehaviour
{
    private Vector3 direction;
    PooledObject pooledObject;
    

    private float speed = 5f;


    private void Start()
    {
        pooledObject = GetComponent<PooledObject>();
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void OnBecameInvisible()
    {
        //Return to pool when off-screen
        if (pooledObject != null)
        {
            pooledObject.ReturnToPool();
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("PooledObject component missing, destroying bullet.");
        }
    }

}
