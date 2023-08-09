using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rb2d;
    SpriteRenderer sR;
    Animator ani;

    public float speed = 5f;
    public float jumpForce = 7f;
    private float speedrun = 50f;
    public float inicioX = 0f;
    public float inicioY = 0f;
    private bool appearingActive = true;
    private bool isFacingLeft = false;
    public AudioSource AudioPush; 


    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sR = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = new Quaternion(0,0,0,0);
        if (appearingActive)
        {
            Appering();
        }
        if (DamagePlayer.damageSignal == false && pauseManager.pause == false)
        {
            HorizontalController();
            JumpController();
        } 
        else if (DamagePlayer.damageSignal == true)
        {
            Appering();
        }
    }

    private void HorizontalController()
    {
        ani.SetFloat("MoveHorizontal", Input.GetAxis("Horizontal"));
        ani.SetFloat("MoveVertical", Input.GetAxis("Vertical"));
        rb2d.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb2d.velocity.y);
        
        if (Input.GetAxis("Horizontal") < 0)
        {
            if (!isFacingLeft)
            {
                sR.flipX = true;
                isFacingLeft = true;
            }
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            if (isFacingLeft)
            {
                sR.flipX = false;
                isFacingLeft = false;
            }            
        }

        //Corrrer
        ani.SetBool("Run", false);
        if (Input.GetButtonDown("Fire1"))
        {
            ani.SetBool("Run", true);
            rb2d.velocity = new Vector2(Input.GetAxis("Horizontal") * speedrun, rb2d.velocity.y);
        }
    }

    private void JumpController()
    {
        if (DetectGround.checkGround == true)
        {
            ani.SetBool("MoveJump", false);
            //ani.SetBool("DJump", false);
            //doubleJump = true;
            if (Input.GetButtonDown("Jump"))
            {
                gameObject.GetComponent<AudioSource>().Play();
                ani.SetBool("MoveJump", true);
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            }
        }
        else if (DetectGround.checkGround == false)
        {
            ani.SetBool("MoveJump", false);
        }
        
    }
   
    private void Appering()
    {
        sR.enabled = false;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        gameObject.transform.position = new Vector3(inicioX, inicioY, 0);
        DamagePlayer.damageTime += Time.deltaTime;
        if (DamagePlayer.damageTime > 0.5f)
        {
            sR.enabled = true;
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            appearingActive = false;
            DamagePlayer.damageSignal = false;
            DamagePlayer.Lesslife();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MovingPlatform")
        {
            transform.parent = collision.transform;
        }
        
        if (collision.gameObject.tag == "Box" && DetectGround.checkGround == true)
        {
            ani.SetBool("Pushing", true);
            AudioPush.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MovingPlatform")
        {
            transform.parent = null;
        }

        if (collision.gameObject.tag == "Box")
        {
            ani.SetBool("Pushing", false);
        }
    }
}