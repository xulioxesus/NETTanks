using Unity.Netcode;
using UnityEngine;

public class ProjectileLauncher : NetworkBehaviour
{
    
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject serverProjectilePrefab;
    [SerializeField] private GameObject clientProjectilePrefab;

    [Header("Settings")]
    [SerializeField] private float projectileSpeed;
    private bool shouldFire;

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
        if(!IsOwner) {return; }
        if(!shouldFire) {return; }

        PrimaryFireServerRpc(projectileSpawnPoint.position, projectileSpawnPoint.up);
        SpawnDummyProjectile(projectileSpawnPoint.position, projectileSpawnPoint.up);
    }

    private void SpawnDummyProjectile(Vector3 position, Vector3 up)
    {
        GameObject projectileInstance = Instantiate(clientProjectilePrefab, position, Quaternion.identity);
        projectileInstance.transform.up = up;
    }

    [Rpc(SendTo.Server)]
    private void PrimaryFireServerRpc(Vector3 position, Vector3 up)
    {
        GameObject projectileInstance = Instantiate(serverProjectilePrefab, position, Quaternion.identity);
        projectileInstance.transform.up = up;
        SpawnDummyProjectileClientRpc(position, up); 
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void SpawnDummyProjectileClientRpc (Vector3 position, Vector3 up)
    {
        if(IsOwner) { return; }
        SpawnDummyProjectile(position, up);
    }
}
