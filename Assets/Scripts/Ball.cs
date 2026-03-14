using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Ball : MonoBehaviour
{
    [Header("Game Component Refs")]
    Rigidbody2D rb;
    SpriteRenderer sr;


    [SerializeField] private float speed;
    private const float speedinc = 0.1f;
    private const float maxspeed = 10f;
   
    [Header("Audio Files")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip wallSound;
    [SerializeField] AudioClip paddleSound;
    [SerializeField] AudioClip pointSound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FirstService();
    }

    Vector2 GenerateDirection(float xdir)
    {
        float angle = Random.Range(-40f, 40f) * Mathf.Deg2Rad;

        Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        dir.x = xdir;

        return dir.normalized;
    }

    void Launch(float xdir)
    {
        rb.linearVelocity = GenerateDirection(xdir) * speed;
    }

    public void FirstService()
    {
        float xdir = Random.value < 0.5f ? -1f : 1f;
        Launch(xdir);
    }

    public void Service(int player)
    {
        float xdir = player == 1 ? -1f : 1f;
        Launch(xdir);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            audioSource.PlayOneShot(wallSound);

            Vector2 v = rb.linearVelocity;

            if (Mathf.Abs(v.y) < 1.5f)
                v.y = Mathf.Sign(v.y) * 1.5f;

            rb.linearVelocity = v;
        }

        if (collision.gameObject.CompareTag("Paddle"))
        {
            audioSource.PlayOneShot(paddleSound);

            BoxCollider2D paddleCollider = collision.collider as BoxCollider2D;

            float paddleY = collision.transform.position.y;
            float ballY = transform.position.y;

            float paddleHeight = paddleCollider.bounds.size.y;

            float offset = Mathf.Clamp((ballY - paddleY) / paddleHeight, -1f, 1f);

            Vector2 dir = new Vector2(
                Mathf.Sign(rb.linearVelocity.x),
                offset
            ).normalized;

            float newSpeed = Mathf.Min(rb.linearVelocity.magnitude + speedinc, maxspeed);
            rb.linearVelocity = dir * newSpeed;
        }
    }

    public void OnBallPoint()
    {
        audioSource.PlayOneShot(pointSound);
        rb.linearVelocity = Vector2.zero;
        sr.enabled = false;
    }

    public void Reset()
    {
        transform.position = Vector3.zero;
        sr.enabled = true;
    }

}
