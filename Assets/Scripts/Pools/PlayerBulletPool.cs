
//!Using TestBullet while not real bullet
public class PlayerBulletPool : GenericObjectPool<TestBullet>
{
    public static PlayerBulletPool Instance { get; private set; }
    void Awake()
    {
        //Singleton Pattern Implementation
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        InitializePool();
    }
    protected override void AssignPoolToItem(TestBullet item)
    {
        item.SetPool(this);
    }
    
}
