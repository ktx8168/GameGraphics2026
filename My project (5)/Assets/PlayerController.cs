using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
 
public class PlayerController : MonoBehaviour
{

public string playerName = "Player";
public int hp = 100;
private const int StartingLives = 3;
public int lives = StartingLives;
public float moveSpeed = 5f;
public Transform visual;
public Vector2 airScale = new Vector2(0.8f, 1.2f);
private Vector2 moveInput;
public float jumpPower = 8f;
public float fallLimit = -10f;
public Vector3 startPosition;
public GameObject gameOverPanel;
public TMP_Text livesText;
public GameObject heart1;
public GameObject heart2;
public GameObject heart3;
public string nextSceneName = "Stage2";
private Rigidbody2D rb;
private bool isGrounded = false;
private bool isGameOver = false;
private int lastLifeLostFrame = -1;
private float facing = 1f;

    void Start()
  {
  
 
    rb = GetComponent<Rigidbody2D>();
        transform.position = startPosition;
        lives = StartingLives;
        isGameOver = false;
        gameOverPanel.SetActive(false);
        UpdateLivesText();

        Debug.Log(playerName + "시작.체력" + hp);
        Debug.Log("피격 후 체력" + (hp - 30));
        Debug.Log("달리기 속도" + moveSpeed *2);
        Debug.Log("10/4="+10f/4f);
  }


    void OnMove(InputValue value)
    {
        if (isGameOver)
        {
            return;
        }
        moveInput = value.Get<Vector2>();

        if (moveInput.x > 0)
        {
            facing = 1f;
        }
        else if (moveInput.x < 0)
        {
            facing = -1f;
        }
    }
    void OnJump(InputValue value)
  {
     if (value.isPressed && isGrounded&& !isGameOver)
   
 {
      Debug.Log("점프!");
rb.linearVelocity=new Vector2(rb.linearVelocity.x,jumpPower);
    }
  }


    void Update()
    {
        if (isGameOver)
        {
            return;
        }

        if (!isGameOver)
        {
            transform.Translate(
                Vector3.right * moveInput.x * moveSpeed * Time.deltaTime
            );
        }

        if (transform.position.y < fallLimit)
        {
            LoseLife();
        }
        if (isGrounded)
        {
            visual.localScale = new Vector3(facing, 1f, 1f);
        }
        else
        {
            visual.localScale = new Vector3(
                facing * airScale.x,
                airScale.y,
                1f
            );
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            //Debug.Log("착지");
        }
    }
    
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            //Debug.Log("공중");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    
    {
        if (isGameOver)
        {
            return;
        }

        if(other.CompareTag("Goal"))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        
        else if (other.CompareTag("Trap"))
        {
            LoseLife();
        }
    }    
     
     void LoseLife()
    {
        // A fall and overlapping traps in the same frame count as one death.
        if (isGameOver || lastLifeLostFrame == Time.frameCount)
        {
            return;
        }

        lastLifeLostFrame = Time.frameCount;
        lives = Mathf.Max(0, lives - 1);
        UpdateLivesText();

        transform.position = startPosition;
        rb.linearVelocity = Vector2.zero;
        isGrounded = false;

        Debug.Log("낙사. 남은 목숨 " + lives);

        if (lives <= 0)
        {
            isGameOver = true;
            moveInput = Vector2.zero;
            gameOverPanel.SetActive(true);
        }
    }

    void UpdateLivesText()
    {
        // LIFE 숫자 대신 하트 이미지로 목숨을 표시합니다.
        if (livesText != null)
        {
            livesText.gameObject.SetActive(false);
        }

        if (heart1 != null)
        {
            if (lives >= 1)
            {
                heart1.SetActive(true);
            }
            else
            {
                heart1.SetActive(false);
            }
        }

        if (heart2 != null)
        {
            if (lives >= 2)
            {
                heart2.SetActive(true);
            }
            else
            {
                heart2.SetActive(false);
            }
        }

        if (heart3 != null)
        {
            if (lives >= 3)
            {
                heart3.SetActive(true);
            }
            else
            {
                heart3.SetActive(false);
            }
        }
    }
    
}
