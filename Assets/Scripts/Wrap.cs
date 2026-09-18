using UnityEngine;

public class Wrap : MonoBehaviour
{


    //Wrap around if we've gone off the screen
    private void Update()
    {
        //need to get the position
        //Convert world point to Viewpoint so it's in 0->1 range
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        // If it's moved out of the viewport, warp to opposite side
        Vector3 moveAdjustment = Vector3.zero;
        if(viewportPosition.x < 0)//if moves out of left side move to right side
        {
            moveAdjustment.x += 1;
        }
        else if(viewportPosition.x > 1)//if moves out of right side move to right side
        {
            moveAdjustment.x -= 1;
        }
        else if(viewportPosition.y < 0)//if moves out of bottom move to top
        {
            moveAdjustment.y += 1;
        }
        else if(viewportPosition.y > 1)//if moves out of top move to bottom
        {
            moveAdjustment.y -= 1;
        }


        //Convert back into world coordinates before assigning.
        transform.position = Camera.main.ViewportToWorldPoint(viewportPosition + moveAdjustment);
    }
}
