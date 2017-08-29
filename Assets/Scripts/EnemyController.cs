using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	/*
      Mechanics of the enemy 
      
      De que el enemigo avanzace hacia la izquierda con cierta velocidad y 
      detecta cuando tocamos la barrera invisible que es la que producira,
      la destruccion del GameObject.
    */

	// Normal
	private float speed = 2f;

    // UI
    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = Vector2.left * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Destroyer")
        {
            Destroy(gameObject);
		}
    }

}
