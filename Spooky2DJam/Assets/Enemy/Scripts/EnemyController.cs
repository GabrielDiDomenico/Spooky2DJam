using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float enemySpeed;
    public SpriteRenderer enemySprite;
    public BoxCollider2D enemyMouth;
    public GameObject playerReference;
    private bool playerIsNotDead = true;

    [SerializeField] private Sprite[] killFrame;
    [SerializeField] private Sprite[] walkFrame;

    private int currentFrameKill;
    private int currentFrameWalk;
    private float timer;
    private float framerate = .15f;
    public SpriteRenderer spriteRenderer;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsNotDead)
        {
            PursuitPlayer();

            KillPlayer();
        }
        else
        {
            AnimationKill();
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.Sleep();
        }
        
    }
    private void AnimationKill()
    {
        timer += Time.deltaTime;

        if (timer >= framerate)
        {
            timer -= framerate;
            currentFrameKill = (currentFrameKill + 1) % killFrame.Length;
            spriteRenderer.sprite = killFrame[currentFrameKill];
           
        }
    }
    private void KillPlayer()
    {
        float enemyPosX = rb.gameObject.transform.position.x;
        float playerPosX = playerReference.transform.position.x;
        if (enemyPosX < 0)
        {
            enemyPosX *= -1;
        }
        if (playerPosX < 0)
        {
            playerPosX *= -1;
        }
        float distanceEnemyPlayer = playerPosX - enemyPosX;
        if (distanceEnemyPlayer < 0)
        {
            distanceEnemyPlayer *= -1;
        }
        if (distanceEnemyPlayer < 1)
        {
            ContactPoint2D[] contacts = new ContactPoint2D[5];
            enemyMouth.GetContacts(contacts);
            foreach (ContactPoint2D c in contacts)
            {
                if(c.collider.Distance(enemyMouth).distance < 1)
                {
                    playerReference.GetComponent<PlayerController>().DeathByEnemy();
                    playerIsNotDead = false;
                    
                }
            }
        }
            
    }

    private void PursuitPlayer()
    {
        float enemyPosY = rb.gameObject.transform.position.y;
        float playerPosY = playerReference.transform.position.y;
        if (enemyPosY < 0)
        {
            enemyPosY *= -1;
        }
        if (playerPosY < 0)
        {
            playerPosY *= -1;
        }
        float distanceEnemyPlayer = playerPosY - enemyPosY;
        if (distanceEnemyPlayer < 0)
        {
            distanceEnemyPlayer *= -1;
        }

        if(distanceEnemyPlayer < 1)
        {
            timer += Time.deltaTime;

            if (timer >= framerate)
            {
                timer -= framerate;
                currentFrameWalk = (currentFrameWalk + 1) % walkFrame.Length;
                spriteRenderer.sprite = walkFrame[currentFrameWalk];

            }

            RaycastHit2D[] resultsRightCast = new RaycastHit2D[5];
            RaycastHit2D[] resultsLeftCast = new RaycastHit2D[5];
            int a = rb.Cast(new Vector2(1, 0), resultsRightCast);
            int b = rb.Cast(new Vector2(-1, 0), resultsLeftCast);

            if (resultsRightCast[0].transform.gameObject.name == "Player")
            {
                rb.velocity = new Vector2(
                        1 * enemySpeed * Time.fixedDeltaTime,
                        rb.velocity.y
                    );
                enemySprite.flipX = false;
            }
            else if (resultsLeftCast[0].transform.gameObject.name == "Player")
            {
                rb.velocity = new Vector2(
                        -1 * enemySpeed * Time.fixedDeltaTime,
                        rb.velocity.y
                    );
                enemySprite.flipX = true;
            }
            else
            {
                rb.velocity = new Vector2(0, 0);
            }
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
    }
            
}
