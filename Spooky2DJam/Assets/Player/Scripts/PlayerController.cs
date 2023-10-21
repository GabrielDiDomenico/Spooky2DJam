using UnityEngine;

// A simple 2D movement controller for a player in Unity
public class PlayerController : MonoBehaviour
{

    private float playerInput;
    private float jumpForce = 24f;
    private Rigidbody2D rb;

    [SerializeField] private float speed = 250f;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject flashlight;
    [SerializeField] private Sprite[] walkFrames;
    [SerializeField] private Sprite[] dropFirstFrame;
    [SerializeField] private Sprite[] dropFrames;
    [SerializeField] private Sprite[] idleFrames;

    private int currentFrameWalk;
    private int currentFrameDrop;
    private int currentFrameIdle;
    private float timer;
    private float framerate = .1f;
    private SpriteRenderer spriteRenderer;
    private bool isFalling=false;
    private float timerToIdle=0;


    private void Awake()
    {
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        // Get component references
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        // get A and D press to movement
        playerInput = Input.GetAxisRaw("Horizontal");
        

        JumpAction();

        CameraUpdatePosition();

        SpriteAnimation();


    }

    private void CameraUpdatePosition()
    {
        Vector3 cameraPos = playerCamera.transform.position;
        Vector3 playerPos = gameObject.transform.position;
        playerCamera.transform.position = new Vector3(playerPos.x, playerPos.y, cameraPos.z);
    }

    private void JumpAction()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(rb.velocity.y == 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0);
            }

        }
    }

    private void SpriteAnimation()
    {
        
        
        if (rb.velocity.y != 0)
        {
            timerToIdle = 0;
            isFalling = true;
            Drop(dropFirstFrame);
            if(isFalling && rb.velocity.y == 0)
            {
                Drop(dropFrames);
            }
        }
        else if(currentFrameDrop == 0)
        {
            // Right
            if (playerInput > 0)
            {
                timerToIdle = 0;
                gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                Walk(walkFrames);
            }
            // Left
            else if (playerInput < 0)
            {
                timerToIdle = 0;
                if (gameObject.transform.rotation.y == 0)
                    gameObject.transform.Rotate(new Vector3(0, -180, 0));
                Walk(walkFrames);
            }
            else
            {
                timerToIdle += Time.deltaTime;
                if(timerToIdle > 3)
                    Idle(idleFrames);
            }
        }
        else
        {
            timerToIdle = 0;
            Drop(dropFrames);
        }
        
    }

    private void Idle(Sprite[] frames)
    {
        float idleFramerate = .15f;
        timer += Time.deltaTime;

        if (timer >= idleFramerate)
        {
            timer -= idleFramerate;
            currentFrameIdle = (currentFrameIdle + 1) % frames.Length;
            spriteRenderer.sprite = frames[currentFrameIdle];
        }
    }

    private void Walk(Sprite[] frames)
    {
        timer += Time.deltaTime;

        if (timer >= framerate)
        {
            timer -= framerate;
            currentFrameWalk = (currentFrameWalk + 1) % frames.Length;
            spriteRenderer.sprite = frames[currentFrameWalk];
        }
    }

    private void Drop(Sprite[] frames)
    {
        timer += Time.deltaTime;

        if (timer >= framerate)
        {
            timer -= framerate;
            currentFrameDrop = (currentFrameDrop + 1) % frames.Length;
            spriteRenderer.sprite = frames[currentFrameDrop];
        }
    }

    private void FixedUpdate()
    {
        // Move the player horizontally
        rb.velocity = new Vector2(
            playerInput * speed * Time.fixedDeltaTime,
            rb.velocity.y
        );
    }


}