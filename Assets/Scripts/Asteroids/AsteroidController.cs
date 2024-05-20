using UnityEngine;

public class AsteroidController : MonoBehaviour, IPooledObject
{
    #region Variables
    //public static Action onDestroy;
    private AsteroidsSpawner spawner;
    private AsteroidsHealth healthController;

    public int damageAmount;

    // Movement
    private MovementController mController;
    private Vector2 direction;

    // Pooled object
    public poolTags _tag;
    public poolTags Tag
    {
        get { return _tag; }
    }
    #endregion

    private void Awake()
    {
        mController = GetComponent<MovementController>();
        healthController = GetComponent<AsteroidsHealth>();
        spawner = AsteroidsSpawner.Instance;
        direction = new Vector2(0f, 1f);

        healthController.Killed += OnProjectileDestroy;
        healthController.Killed += spawner.OnAsteroidDestroy;
    }

    private void OnDestroy()
    {
        healthController.Killed -= OnProjectileDestroy;
        healthController.Killed -= spawner.OnAsteroidDestroy;
    }

    private void FixedUpdate()
    {
        mController.MovementUpdate(direction);
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamagable damagable = collision.gameObject.GetComponentInParent<IDamagable>();

        if (damagable != null)
        {
            damagable.Damage(damageAmount);
            Destroy();
        }
    }

    public void OnProjectileDestroy()
    {
        switch (Tag)
        {
            case poolTags.bigAsteroid:
                spawner.SpawnAsteroidOnDestroy(poolTags.mediumAsteroid, this.transform);
                spawner.SpawnAsteroidOnDestroy(poolTags.mediumAsteroid, this.transform);
                spawner.SpawnAsteroidOnDestroy(poolTags.mediumAsteroid, this.transform);
                break;
            case poolTags.mediumAsteroid:
                spawner.SpawnAsteroidOnDestroy(poolTags.smallAsteroid, this.transform);
                spawner.SpawnAsteroidOnDestroy(poolTags.smallAsteroid, this.transform);
                break;
            default:
                break;
        }
        Destroy();
    }

    private void Destroy()
    {
        // Here spawn particle system
        ObjectPooler.Instance.ReturnObjectToPool(this.gameObject);
    }

    public void OnObjectSpawn()
    {

    }

}
