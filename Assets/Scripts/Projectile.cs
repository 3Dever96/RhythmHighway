using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody body;
    public float speed;
    public float lifeTime;
    private float currentTime;

    private void Start()
    {
        body = GetComponent<Rigidbody>();

        body.linearVelocity = transform.forward * speed;

        currentTime = lifeTime;
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
