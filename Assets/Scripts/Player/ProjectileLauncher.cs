using Unity.Netcode;
using UnityEngine;

public class ProjectileLauncher : NetworkBehaviour
{
    
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject serverProjectilePrefab;
    [SerializeField] private GameObject clientProjectilePrefab;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private CoinWallet wallet;

    [Header("Settings")]
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float fireRate;
    [SerializeField] private float muzzleFlashDuration;
    [SerializeField] private int costToFire;


    private bool shouldFire;
    private float timer;
    private float muzzleFlashTimer;


    public override void OnNetworkSpawn()
    {
        if(!IsOwner) {return; }

        inputReader.PrimaryFireEvent += HandlePrimaryFire;
    }

    public override void OnNetworkDespawn()
    {
        if(!IsOwner) {return; }
        inputReader.PrimaryFireEvent -= HandlePrimaryFire;
    }

    private void HandlePrimaryFire(bool shouldFire)
    {
        this.shouldFire = shouldFire;

    }

    void Update()
    {
        if (muzzleFlashTimer > 0f)
        {
            muzzleFlashTimer -= Time.deltaTime;

            if (muzzleFlashTimer <= 0f)
            {
                muzzleFlash.SetActive(false);
            }
        }

        if(!IsOwner) {return; }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if(!shouldFire) {return; }

        if (timer > 0) { return; }

        if (wallet.TotalCoins.Value < costToFire) { return; }

        PrimaryFireServerRpc(projectileSpawnPoint.position, projectileSpawnPoint.up);
        SpawnDummyProjectile(projectileSpawnPoint.position, projectileSpawnPoint.up);

        timer = 1 / fireRate;

    }

    private void SpawnDummyProjectile(Vector3 position, Vector3 up)
    {
        muzzleFlash.SetActive(true);
        muzzleFlashTimer = muzzleFlashDuration;

        GameObject projectileInstance = Instantiate(clientProjectilePrefab, position, Quaternion.identity);
        projectileInstance.transform.up = up;

        Physics2D.IgnoreCollision(playerCollider, projectileInstance.GetComponent<Collider2D>());

        if (projectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.linearVelocity = rb.transform.up * projectileSpeed;
        }

    }

    [Rpc(SendTo.Server)]
    private void PrimaryFireServerRpc(Vector3 position, Vector3 up)
    {

        if (wallet.TotalCoins.Value < costToFire) { return; }

        wallet.SpendCoins(costToFire);

        GameObject projectileInstance = Instantiate(
            serverProjectilePrefab,
            position,
            Quaternion.identity);
        projectileInstance.transform.up = up;

        Physics2D.IgnoreCollision(playerCollider, projectileInstance.GetComponent<Collider2D>());

        if (projectileInstance.TryGetComponent<DealDamageOnContact>(out DealDamageOnContact dealDamage))
        {
            dealDamage.SetOwner(OwnerClientId);
        }

        if (projectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.linearVelocity = rb.transform.up * projectileSpeed;
        }

        SpawnDummyProjectileClientRpc(position, up);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void SpawnDummyProjectileClientRpc (Vector3 position, Vector3 up)
    {
        if(IsOwner) { return; }
        SpawnDummyProjectile(position, up);
    }
}
