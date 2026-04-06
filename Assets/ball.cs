using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    [SerializeField] private float intialVelocity = 4f;
    [SerializeField] private float velocityMultiplier = 1.2f;
    [SerializeField] private float restartDelay = 3f;

    private Rigidbody2D ballRb;

    void Start()
    {
        ballRb = GetComponent<Rigidbody2D>();
        Launch();
    }

    private void Launch()
    {
        float xVelocity = Random.Range(0, 2) == 0 ? 1 : -1;
        float yVelocity = Random.Range(0, 2) == 0 ? 1 : -1;
        ballRb.linearVelocity = new Vector2(xVelocity, yVelocity) * intialVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            ballRb.linearVelocityY *= velocityMultiplier;
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayPaddleHit();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        ballRb.linearVelocity = Vector2.zero;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGoal();
        }

        if (collision.gameObject.CompareTag("Goal1"))
        {
            GameManager.Instance.Paddle2Scored();
            GameManager.Instance.Restart();
        }
        else
        {
            GameManager.Instance.Paddle1Scored();
            GameManager.Instance.Restart();
        }

        if (GameManager.Instance.GameOver)
        {
            return;
        }

        StartCoroutine(LaunchAfterDelay());
    }

    private IEnumerator LaunchAfterDelay()
    {
        yield return new WaitForSeconds(restartDelay);

        if (!GameManager.Instance.GameOver)
        {
            Launch();
        }
    }
}
