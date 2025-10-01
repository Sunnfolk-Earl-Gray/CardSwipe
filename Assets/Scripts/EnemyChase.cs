using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed;
    [SerializeField] private float attackDistance;
    [SerializeField] private Rigidbody2D rb;

    private void Start()
    {
        //target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        var heading = target.position - transform.position;
        var distance = heading.magnitude;
        heading = heading / distance;
        if (distance >= attackDistance) rb.linearVelocity = heading * speed * distance;
        else rb.linearVelocity = new Vector3(0, 0, 0);
    }
}

