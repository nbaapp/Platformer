using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StarBeing : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject Laser;
    public GameObject Burst;
    private Logic logic;
    public Animator Animator;
    public GameObject Bullet;
    private PlayerInputActions playerInputActions; //hi

    public float maxSpeed = 10;
    public float acceleration = 5;
    public float deceleration = 5;
    public float stopThreshold = 1;
    public float jumpStrength = 10;
    public int maxNumberOfJumps = 3;
    public int MaxNumberOfDashes = 3;
    public float dashSpeed = 10;
    public float laserKick = 10;
    public float blastKick = 5;
    private int numberOfJumps = 3;
    private int numberOfDashes = 3;
    private float laserOffset = 25;
    public float bulletOffset = 25;
    private bool canJump = true;
    private bool canDash = true;
    private bool dead = false;
    private bool facingRight = true;
    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.FindWithTag("Logic").GetComponent<Logic>();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.performed += Jump;
        playerInputActions.Player.Dash.performed += Dash;
        playerInputActions.Player.ShootBullet.performed += ShootBullet;
        playerInputActions.Player.ShootLaser.performed += ShootLaser;
        playerInputActions.Player.Bang.performed += Bang;

    }


    private void FixedUpdate()
    {
        Move();
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

    private void Move()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        if (inputVector.x > 0 && !facingRight)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
            facingRight = true;
        }
        else if (inputVector.x < 0 && facingRight)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
            facingRight = false;
        }

        if (Mathf.Abs(rb.velocity.x) < maxSpeed && !dead)
        {
            rb.AddForce(inputVector * acceleration);
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
            if (Mathf.Abs(rb.velocity.x) < stopThreshold)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                Animator.SetBool("IsRunning", false);
            }
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump" + context);
        if (context.performed)
        {
            if (canJump && !dead)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
                numberOfJumps--;
            }
            if (numberOfJumps <= 0)
            {
                canJump = false;
            }
        }
    }

    private void Dash(InputAction.CallbackContext context)
    {
        if (canDash && !dead)
        {
            Debug.Log("Dash");
            if (!facingRight)
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

    private void ShootBullet(InputAction.CallbackContext context)
    {
        Debug.Log("Bullet" + context);
        if (!dead)
        {
            if (!facingRight)
            {
                Instantiate(Bullet, new Vector3(transform.position.x - bulletOffset, transform.position.y, transform.position.z), Quaternion.identity);
            }
            else
            {
                Instantiate(Bullet, new Vector3(transform.position.x + bulletOffset, transform.position.y, transform.position.z), Quaternion.identity);
            }
        }
    }

    private void ShootLaser(InputAction.CallbackContext context)
    {
        Debug.Log("Laser" + context);
        if (!dead)
        {
            if (!facingRight)
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
    }

    private void Bang(InputAction.CallbackContext context)
    {
        Debug.Log("Bang" + context);
        if (!dead)
        {
            if (!facingRight)
            {
                Instantiate(Burst, new Vector3(transform.position.x, transform.position.y + 12.5f, transform.position.z), Quaternion.Euler(new Vector3(0, 180, 0)));
                rb.AddForce(new Vector2(1, 0) * blastKick);
            }
            else
            {
                Instantiate(Burst, new Vector3(transform.position.x, transform.position.y + 12.5f, transform.position.z), Quaternion.identity);
                rb.AddForce(new Vector2(-1, 0) * blastKick);
            }
        }
    }
}
