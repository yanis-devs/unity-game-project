using UnityEngine;

public class betterMovement : MonoBehaviour
{
    private Rigidbody2D rd;

    public float TitanaCooldown = 0f;
    public float TitanaDuration = 4f;
    private bool TitanaActive = false;

    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. Only count down cooldown when ability is NOT active
        if (!TitanaActive && TitanaCooldown > 0)
        {
            TitanaCooldown -= Time.deltaTime;
        }

        // 2. Count down duration while active
        if (TitanaActive)
        {
            TitanaDuration -= Time.deltaTime;

            if (TitanaDuration <= 0f)
            {
                // Return to normal size
                transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                TitanaDuration = 4f; // Reset duration timer
                TitanaActive = false;
                TitanaCooldown = 8f; // Start 8-second cooldown NOW
            }
        }

        // 3. Use GetKeyDown so it triggers ONCE per keypress
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (TitanaCooldown <= 0 && !TitanaActive)
            {
                // Grow big!
                transform.localScale = new Vector3(2f, 2f, 0.5f);
                TitanaActive = true;
            }
        }
    }

    void FixedUpdate()
    {
        float moveX = 0f;
        float moveY = 0f;
        float speed = 5f;

        if (Input.GetKey(KeyCode.W)) moveY = 1f;
        if (Input.GetKey(KeyCode.S)) moveY = -1f;
        if (Input.GetKey(KeyCode.D)) moveX = 1f;
        if (Input.GetKey(KeyCode.A)) moveX = -1f;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 10f;
        }

        rd.linearVelocity = new Vector2(moveX * speed, moveY * speed);
    }
}