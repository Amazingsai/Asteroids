using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    // Private variables 
    //ship parameters
    private Rigidbody2D ShipRigidbody; // Reference to the Rigidbody2D component attached to the player
    private float acceleration = 10f;
    private float velocity = 10f;
    private float rotationSpeed = 200f;
    private bool isAlive = true;
    private bool isAccelerating = false;
    private float bulletSpeed = 8f;

    //object reference
    public Transform bulletSpawn; // sets position of bullets when created
    public Rigidbody2D bulletPrefab;
    public ParticleSystem destroyedParticles;


    void Start()
    {
        // Initialize the Rigidbody2D component
        ShipRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isAlive)
        {
            //handles ship acceleration
            isAccelerating = Input.GetKey(KeyCode.UpArrow);//Increase velocity up to a maximum
            //handles ship rotation
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(rotationSpeed * Time.deltaTime * transform.forward);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(-rotationSpeed * Time.deltaTime * transform.forward);
            }
            //handles shooting
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //creates bullet prefab with position and rotation to a rigidbody bullet variable
                Rigidbody2D bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity); 

                //Inherit velocity only in the forward direction of the ship
                Vector2 shipVelocity = ShipRigidbody.velocity;
                Vector2 shipDirection = transform.up;
                float shipForwardSpeed = Vector2.Dot(shipVelocity, shipDirection); //use dot product to calculate speed and direction player is facing

                //Don't want to inherit in the opposite direction, else we'll get stationary bullets.
                if(shipForwardSpeed < 0)
                {
                    shipForwardSpeed = 0;
                }
            bullet.velocity = shipDirection * shipForwardSpeed;
                
            //Add force to propel bullet in direction the player is facing
            bullet.AddForce(bulletSpeed * transform.up, ForceMode2D.Impulse);
            }
        }
    }

    void FixedUpdate()
    {
        if(isAlive && isAccelerating)
        {
            // Are we accelerating?
            ShipRigidbody.AddForce(acceleration*transform.up); //adds a forward force
            ShipRigidbody.velocity = Vector2.ClampMagnitude(ShipRigidbody.velocity, velocity); //limits velocity to not accelerate forever
        }
    }
    //call gamemanager script to end the game when player gets hit by an Asteroid
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Asteroid"))
        {
            isAlive = false;

            //Get a reference to GameManager
            GameManager gameManager = FindAnyObjectByType<GameManager>();

            //Restart game after delay
            gameManager.GameOver();

            //Show the destroyed effect
            Instantiate(destroyedParticles, transform.position, Quaternion.identity);

            //Destroy the player
            Destroy(gameObject);
        }
    }
}
