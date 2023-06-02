using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Logic logic;
    private GameObject player;
    public float moveSpeed = 10;
    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.FindWithTag("Logic").GetComponent<Logic>();
        player = GameObject.Find("Star Being");
    }

    // Update is called once per frame
    void Update()
    {
        MoveToPlayer();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
            logic.NewTarget();
            logic.AddScore();
        }
    }

    void MoveToPlayer()
    {
        transform.position = Vector3.MoveTowards(gameObject.transform.position, player.transform.position, moveSpeed);
    }
}
