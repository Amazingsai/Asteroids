using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int asteroidCount = 0;
    public Asteroid asteroidPrefab;
    private int level = 0;
    public int score;

    // Update is called once per frame
    private void Update()
    {
        //If there are no more asteroids, spawn more
        if(asteroidCount == 0)
        {
            //Increase level
            level+=1;
            Debug.Log("Level " + level);
        

            //spawn the correct number for this level
            // 1=>4, 2=>6, 3=>8, 4=>10 ...
            int numAsteroids = 2 + (2*level);
            for(int i = 0; i < numAsteroids; i++)
            {
                //spawn asteroid
                //How far along the edge
                float offset = Random.Range(0f, 1f);
                Vector2 viewportSpawnPosition = Vector2.zero;

                //which edge on the screen
                int edge = Random.Range(0, 4);
                //bottom edge
                if(edge == 0)
                {
                    viewportSpawnPosition = new Vector2(offset, 0);
                }
                //top edge
                else if(edge == 1)
                {
                    viewportSpawnPosition = new Vector2(offset, 1);
                }
                //left edge
                else if(edge == 2)
                {
                    viewportSpawnPosition = new Vector2(0, offset);
                } 
                //right edge
                else if(edge == 3)
                {
                    viewportSpawnPosition = new Vector2(0, offset);
                }
                //Create the asteroid
                Vector2 worldSpawnPosition = Camera.main.ViewportToWorldPoint(viewportSpawnPosition);
                Asteroid asteroid = Instantiate(asteroidPrefab, worldSpawnPosition, Quaternion.identity);
                asteroid.gameManager = this;
            }
        }
    }
    //end the game when player gets hit
    public void GameOver()
    {
        StartCoroutine(Restart());   
    }

    private IEnumerator Restart()
    {
        Debug.Log("Game Over :(" + " Total Score: " + score);

        //Wait a bit before 
        yield return new WaitForSeconds(2f);

        //Restart scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        yield return null;
    }
}
