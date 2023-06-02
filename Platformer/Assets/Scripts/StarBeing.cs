using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBeing : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject Laser;
    private Logic logic;
    public Animator Animator;
    public GameObject Bullet;

    public float maxSpeed = 10;
    public float acceleration = 5;
    public float deceleration = 5;
    public float stopThreshold = 1;
    public float jumpStrength = 10;
    public int maxNumberOfJumps = 3;
    public int MaxNumberOfDashes = 3;
    public float dashSpeed = 10;
    public float laserKick = 10;
    private int numberOfJumps = 3;
    private int numberOfDashes = 3;
    private float laserOffset = 25;
    public float bulletOffset = 25;
    private bool canJump = true;
    private bool canDash = true;
    private bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.FindWithTag("Logic").GetComponent<Logic>();
    }

    // Update is called once per frame
    void Update()
    {
        GetHorizontalMovement();
        GetJump();
        GetDash();
        GetAttack();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 3)
        {
            numberOfDashes = MaxNumberOfDashes;
            numberOfJumps = maxNumberOfJumps;
            canJump = true;
            canDash = true;
        }
        if(collision.gameObject.layer == 7)
        {
            dead = true;
            logic.GameOver();
        }
    }

    void GetHorizontalMovement()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1 && Mathf.Abs(rb.velocity.x) < maxSpeed && !dead)
        {
            rb.AddForce(new Vector2(Input.GetAxis("Horizontal"), 0) * acceleration);
            Animator.SetBool("IsRunning", true);
        }
        else
        {
            if (rb.velocity.x > 0)
            {
                rb.AddForce(new Vector2(-1, 0) * deceleration);
            }
            if (rb.velocity.x < 0)
            {
                rb.AddForce(new Vector2(1, 0) * deceleration);
            }
            if(Mathf.Abs(rb.velocity.x) < stopThreshold)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                Animator.SetBool("IsRunning", false);
            }
        }
    }

    void GetJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump && !dead)
        {
            Debug.Log("jump input recieved");
            rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
            numberOfJumps--;
        }
        if (numberOfJumps <= 0)
        {
            canJump = false;
        }
    }

    void GetDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !dead)
        {
            Debug.Log("Dash input recieved");
            if (Input.GetAxis("Horizontal") < -0.1)
            {
                rb.velocity = new Vector2(-1, 0) * dashSpeed;
            }
            else
            {
                rb.velocity = new Vector2(1, 0) * dashSpeed;
            }
            numberOfDashes--;
        }
        if(numberOfDashes <= 0)
        {
            canDash = false;
        }
    }

    void GetAttack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && !dead)
        {
            if (Input.GetAxis("Horizontal") < -0.1)
            {
                Instantiate(Laser, new Vector3(transform.position.x - laserOffset, transform.position.y, transform.position.z), Quaternion.Euler(new Vector3(0, 180, 0)));
                rb.AddForce(new Vector2(1, 0) * laserKick);
            }
            else
            {
                Instantiate(Laser, new Vector3(transform.position.x + laserOffset, transform.position.y, transform.position.z), Quaternion.identity);
                rb.AddForce(new Vector2(-1, 0) * laserKick);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && !dead)
        {
            if (Input.GetAxis("Horizontal") < -0.1)
            {
                Instantiate(Bullet, new Vector3(transform.position.x - bulletOffset, transform.position.y, transform.position.z), Quaternion.identity);
            }
            else
            {
                Instantiate(Bullet, new Vector3(transform.position.x + bulletOffset, transform.position.y, transform.position.z), Quaternion.identity);
            }
        }
    }
}
