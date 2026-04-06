using UnityEngine;

public class PaddleAI : MonoBehaviour
{
    [SerializeField] private Transform ballTransform;
    [SerializeField] private Rigidbody2D ballRb;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float yBound = 3.75f;
    [SerializeField] private float idleTargetY = 0f;
    [SerializeField] private float reactionOffset = 0.15f;
    [SerializeField] private float targetDeadZone = 0.05f;

    [Header("Easy")]
    [SerializeField] private float easySpeed = 7f;
    [SerializeField] private float easyReactionOffset = 0.45f;
    [SerializeField] private float easyDeadZone = 0.2f;

    [Header("Medium")]
    [SerializeField] private float mediumSpeed = 10f;
    [SerializeField] private float mediumReactionOffset = 0.15f;
    [SerializeField] private float mediumDeadZone = 0.05f;

    [Header("Hard")]
    [SerializeField] private float hardSpeed = 13f;
    [SerializeField] private float hardReactionOffset = 0.03f;
    [SerializeField] private float hardDeadZone = 0.01f;

    private void Awake()
    {
        ApplySelectedDifficulty();

        if (ballTransform == null)
        {
            Ball ball = FindAnyObjectByType<Ball>();
            if (ball != null)
            {
                ballTransform = ball.transform;
            }
        }

        if (ballRb == null && ballTransform != null)
        {
            ballRb = ballTransform.GetComponent<Rigidbody2D>();
        }
    }

    private void ApplySelectedDifficulty()
    {
        switch (GameDifficultySettings.SelectedDifficulty)
        {
            case GameDifficulty.Easy:
                speed = easySpeed;
                reactionOffset = easyReactionOffset;
                targetDeadZone = easyDeadZone;
                break;
            case GameDifficulty.Hard:
                speed = hardSpeed;
                reactionOffset = hardReactionOffset;
                targetDeadZone = hardDeadZone;
                break;
            default:
                speed = mediumSpeed;
                reactionOffset = mediumReactionOffset;
                targetDeadZone = mediumDeadZone;
                break;
        }
    }

    private void Update()
    {
        if (ballTransform == null)
        {
            return;
        }

        Vector2 paddlePosition = transform.position;
        float targetY = GetTargetY(paddlePosition);

        if (Mathf.Abs(paddlePosition.y - targetY) <= targetDeadZone)
        {
            return;
        }

        paddlePosition.y = Mathf.MoveTowards(
            paddlePosition.y,
            targetY,
            speed * Time.deltaTime
        );

        transform.position = paddlePosition;
    }

    private float GetTargetY(Vector2 paddlePosition)
    {
        if (ballRb == null || ballRb.linearVelocity == Vector2.zero)
        {
            return Mathf.Clamp(idleTargetY, -yBound, yBound);
        }

        Vector2 ballVelocity = ballRb.linearVelocity;
        bool ballMovingTowardPaddle = Mathf.Sign(ballVelocity.x) == Mathf.Sign(paddlePosition.x - ballTransform.position.x);

        if (!ballMovingTowardPaddle || Mathf.Approximately(ballVelocity.x, 0f))
        {
            return Mathf.Clamp(idleTargetY, -yBound, yBound);
        }

        float timeToReachPaddle = (paddlePosition.x - ballTransform.position.x) / ballVelocity.x;
        if (timeToReachPaddle <= 0f)
        {
            return Mathf.Clamp(idleTargetY, -yBound, yBound);
        }

        float predictedY = ballTransform.position.y + (ballVelocity.y * timeToReachPaddle);
        predictedY = ReflectWithinBounds(predictedY, yBound);

        float adjustedTarget = predictedY + Mathf.Sign(ballVelocity.y) * reactionOffset;
        return Mathf.Clamp(adjustedTarget, -yBound, yBound);
    }

    private float ReflectWithinBounds(float position, float limit)
    {
        float min = -limit;
        float max = limit;

        while (position > max || position < min)
        {
            if (position > max)
            {
                position = max - (position - max);
            }
            else if (position < min)
            {
                position = min + (min - position);
            }
        }

        return position;
    }
}
