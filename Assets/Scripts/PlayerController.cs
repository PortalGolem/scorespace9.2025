
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] float maxSpeed;
    [SerializeField] AnimationCurve acceleration;

    [SerializeField] float opposingActionModifier;
    [SerializeField] float decelerationModifier;
    //How fast the player should change direction.

    Vector2 input;
    Vector2 speed;
    Vector2 accelerationTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        accelerationTime.x += Time.deltaTime * input.x * (input.x > 0 && accelerationTime.x < 0 || input.x < 0 && accelerationTime.x > 0 ? opposingActionModifier : 1);
        if (input.x == 0)
        {
            if (Mathf.Abs(accelerationTime.x) < decelerationModifier * (Time.deltaTime+0.0001))
            {
                accelerationTime.x = 0;
            }
            else
            {
                accelerationTime.x -= decelerationModifier * Time.deltaTime * (accelerationTime.x > 0 ? 1 : -1);
            }
        }
        accelerationTime.y += Time.deltaTime * input.y * (input.y > 0 && accelerationTime.y < 0 || input.y < 0 && accelerationTime.y > 0 ? opposingActionModifier : 1);
        if (input.y == 0)
        {
            if (Mathf.Abs(accelerationTime.y) < decelerationModifier * (Time.deltaTime+0.0001f))
            {
                accelerationTime.y = 0;
            }
            else
            {
                accelerationTime.y -= decelerationModifier * Time.deltaTime * (accelerationTime.y > 0 ? 1 : -1);
            }
        }
        accelerationTime.x = Mathf.Clamp(accelerationTime.x, -acceleration.keys[acceleration.length - 1].time, acceleration.keys[acceleration.length - 1].time);
        accelerationTime.y = Mathf.Clamp(accelerationTime.y, -acceleration.keys[acceleration.length - 1].time, acceleration.keys[acceleration.length - 1].time);
        speed.x = acceleration.Evaluate(Mathf.Abs(accelerationTime.x));
        if (accelerationTime.x < 0)
        {
            speed.x *= -1;
        }
        speed.y = acceleration.Evaluate(Mathf.Abs(accelerationTime.y));
        speed = speed.normalized * maxSpeed;
        if (accelerationTime.y < 0)
        {
            speed.y *= -1;
        }
        rb.linearVelocity = speed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>().normalized;
    }
}
