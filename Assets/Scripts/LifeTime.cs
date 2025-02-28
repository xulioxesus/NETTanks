using UnityEngine;

public class LifeTime : MonoBehaviour
{
    [SerializeField] private float lifeTime = 1f;
    void Start()
    {
        Invoke("DestroyAfterTime", lifeTime);
    }

    private void DestroyAfterTime()
    {
        Destroy(gameObject);
    }
}
