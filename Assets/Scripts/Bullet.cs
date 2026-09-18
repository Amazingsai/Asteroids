using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletLifetime = 1f;

    //When created, destroy after a set period of time.
    private void Awake()//when created
    {
        Destroy(gameObject, bulletLifetime);//destroyed after 1 second
    }

}
