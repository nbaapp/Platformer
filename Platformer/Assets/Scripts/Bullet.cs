using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject player;
    public float speed = 10;
    private bool positiveDirection;
    public float timer = 5;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Star Being");
        if (transform.position.x >= player.transform.position.x)
        {
            positiveDirection = true;
        }
        else
        {
            positiveDirection = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (positiveDirection)
        {
            transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;
        }
        else
        {
            transform.position -= new Vector3(speed, 0, 0) * Time.deltaTime;

        }

        Destroy(gameObject, timer);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
