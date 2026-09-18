using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public ParticleSystem destroyedParticles;
    public int size = 3;  
    public GameManager gameManager;

    // Start is called before the first frame update
    private void Start()
    {
        //Scale based on size 
        transform.localScale = 0.5f*size*Vector3.one;

        //Add movement, bigger asteroids are slower.
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 direction = new Vector2(Random.value, Random.value).normalized;
        float spawnSpeed = Random.Range(4f - size, 5f - size);
        rb.AddForce(direction*spawnSpeed, ForceMode2D.Impulse);

        //Register creation
        gameManager.asteroidCount++;

        
    }
    //Collision with bullets
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //asteroids are only destroyed with bullets
        if (collision.CompareTag("Bullet"))
        {
            //Increase score
            if(size <= 1)
            {
                gameManager.score+=100;
            }
            else if(size <= 2)
            {
                gameManager.score+=50;
            }
            else
            {
                gameManager.score+=25;
            }
            Debug.Log("Score: "+ gameManager.score);
            //Register the destruction with the game manager.
            gameManager.asteroidCount--;

            //Destroy the bullet so it doesn't carry on and hit more things
            Destroy(collision.gameObject);

            //If size > 1 spawn 2 smaller asteroids of size = 1
            if(size > 1)
            {
                for(int i = 0; i < 2; i++)
                {
                    Asteroid newAsteroid = Instantiate(this, transform.position, Quaternion.identity);
                    newAsteroid.size = size - 1;
                    newAsteroid.gameManager = gameManager;
                }
            }
            //Spawn particles on destruction
            Instantiate(destroyedParticles, transform.position, Quaternion.identity);
            //Destroy this asteroid
            Destroy(gameObject);
            
        }
    }
}