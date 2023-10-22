using UnityEngine;

// A simple 2D movement controller for a player in Unity
public class PlayerController : MonoBehaviour
{

    private float playerInput;
    private float jumpForce = 24f;
    private Rigidbody2D rb;

    [SerializeField] private GameObject wallLight;
    [SerializeField] private float speed = 250f;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject flashlight;
    [SerializeField] private Sprite[] walkFrames;
    [SerializeField] private Sprite[] dropFirstFrame;
    [SerializeField] private Sprite[] dropFrames;
    [SerializeField] private Sprite[] idleFrames;
    [SerializeField] private Sprite[] martelandoFrames;
    [SerializeField] private Sprite[] martelandoLastFrame;
    [SerializeField] private Sprite[] dieByFallFrame;
    [SerializeField] private Sprite[] lastDeathByFallFrame;
    [SerializeField] private Sprite[] dieByEnemyFrame;
    [SerializeField] private Sprite[] lastDeathByEnemyFrame;

    private int currentFrameWalk;
    private int currentFrameDrop;
    private int currentFrameIdle;
    private int currentFrameDieFall;
    private int currentFrameDieEnemy;
    private int currentFrameMartelo;
    private float frameCountToStopMartelada;
    private float timer;
    private float framerate = .1f;
    private SpriteRenderer spriteRenderer;
    private bool isFalling=false;
    private float timerToIdle=0;
    private bool isMartelando = false;
    private bool playerIsDead = false;
    private bool isDoneDieing = false;
    private bool isDeadByFall = false;
    private int firstFrameDeath = 0;

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
        if (!playerIsDead)
        {
            // get A and D press to movement
            playerInput = Input.GetAxisRaw("Horizontal");


            JumpAction();

            CameraUpdatePosition();

            SpriteAnimation();

            PlaceLights();
        }else if (!isDoneDieing)
        {
            rb.velocity = new Vector2(0, 0);
            if (isDeadByFall)
            {
                DieFall(dieByFallFrame);
            }
            else
            {
                DieEnemy(dieByEnemyFrame);
            }
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
            if (isDeadByFall)
            {
                spriteRenderer.sprite = lastDeathByFallFrame[0];
            }
            else
            {
                spriteRenderer.sprite = lastDeathByEnemyFrame[0];
            }
        }
        
    }

    private void CameraUpdatePosition()
    {
        Vector3 cameraPos = playerCamera.transform.position;
        Vector3 playerPos = gameObject.transform.position;
        playerCamera.transform.position = new Vector3(playerPos.x, playerPos.y, cameraPos.z);
    }

    private void JumpAction()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isMartelando == false)
        {
            if(rb.velocity.y == 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0);
            }

        }
    }

    private void PlaceLights()
    {
        
        if (Input.GetKeyDown(KeyCode.E) && rb.IsTouchingLayers(LayerMask.GetMask("Walls")))
        {
            isMartelando = true;
            RaycastHit2D[] resultsRightCast = new RaycastHit2D[5];
            RaycastHit2D[] resultsLeftCast = new RaycastHit2D[5];
            int a = rb.Cast(new Vector2(1, 0), resultsRightCast);
            int b = rb.Cast(new Vector2(-1, 0), resultsLeftCast);

            GameObject go = Instantiate(wallLight);
            if (resultsRightCast[0].transform.gameObject.name == "RightWall" &&
               resultsLeftCast[0].transform.gameObject.name == "LeftWall")
            {
                if (resultsRightCast[0].distance < resultsLeftCast[0].distance)
                {
                    go.transform.position = new Vector3(resultsRightCast[0].transform.position.x+.23f,
                                                        resultsRightCast[0].transform.position.y,
                                                        resultsRightCast[0].transform.position.z);
                }
                else
                {
                    go.transform.position = new Vector3(resultsLeftCast[0].transform.position.x - .23f,
                                                        resultsLeftCast[0].transform.position.y,
                                                        resultsLeftCast[0].transform.position.z);
                    go.transform.Rotate(0, 180, 0);
                }
            }
            

        }
        
    }

    private void SpriteAnimation()
    {
        if(isMartelando == false)
        {
            if (rb.velocity.y > 2 || rb.velocity.y < 0)
            {
                timerToIdle = 0;
                isFalling = true;
                Drop(dropFirstFrame);
                if (isFalling && rb.velocity.y == 0)
                {
                    Drop(dropFrames);
                }
            }
            else if (currentFrameDrop == 0)
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
                    if (timerToIdle > 3)
                        Idle(idleFrames);
                }
            }
            else
            {
                timerToIdle = 0;
                Drop(dropFrames);
            }
        }
        else
        {
            Martela(martelandoFrames);
            if (frameCountToStopMartelada > 6f)
            {
                frameCountToStopMartelada = 0;
                spriteRenderer.sprite = martelandoLastFrame[0];
                isMartelando = false;
            }
        }
    }


    private void DieFall(Sprite[] frames)
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.Sleep();
        timer += Time.deltaTime;

        if (timer >= framerate)
        {
            timer -= framerate;
            currentFrameDieFall = (currentFrameDieFall + 1) % frames.Length;
            spriteRenderer.sprite = frames[currentFrameDieFall];
            firstFrameDeath++;
        }
        if (firstFrameDeath > 0 && currentFrameDieFall == 0)
        {
            isDoneDieing = true;
        }
    }

    private void DieEnemy(Sprite[] frames)
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.Sleep();
        timer += Time.deltaTime;
        
        if (timer >= framerate && !isDoneDieing)
        {
            timer -= framerate;
            currentFrameDieEnemy = (currentFrameDieEnemy + 1) % frames.Length;
            spriteRenderer.sprite = frames[currentFrameDieEnemy];
            firstFrameDeath++;
        }
        if (firstFrameDeath > 0 && currentFrameDieEnemy == 0)
        {
            isDoneDieing = true;
        }
    }

    private void Martela(Sprite[] frames)
    {
        timer += Time.deltaTime;

        if (timer >= framerate)
        {
            timer -= framerate;
            currentFrameMartelo = (currentFrameMartelo + 1) % frames.Length;
            frameCountToStopMartelada += ((currentFrameMartelo + 1) % frames.Length) / 4;
            spriteRenderer.sprite = frames[currentFrameMartelo];
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

    public void DeathByEnemy()
    {
        playerIsDead = true;
    }

    private void FixedUpdate()
    {
        if (isMartelando == false)
        {
            // Move the player horizontally
            rb.velocity = new Vector2(
                playerInput * speed * Time.fixedDeltaTime,
                rb.velocity.y
            );
        }
          
    }


}