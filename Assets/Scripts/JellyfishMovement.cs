using UnityEngine;
using System.Collections;

public class JellyfishMovement : MonoBehaviour {

    //For bobbing
    public float maxDisplacement;
    public float bobSpeed;
    Vector3 basePosition; //position it should return to
    bool goingUp;

    float currTime;
    float startDelay;

    //For moving to finger
    public Vector3 destination;
    float depth = 2f;
    public float moveSpeed;
    float distanceThreshold = 1f;

    void Start () {
        basePosition = transform.position;
        goingUp = true;

        //add a random delay so that they don't all start at the same time
        currTime = 0;
        startDelay = Random.Range(0, 2f);
	}
	
	void Update () {
        Bob();
        MoveToFinger();
    }

    void Bob()
    {
        if (currTime < startDelay)
        {
            currTime += Time.deltaTime;
            return;
        }

        //update the position
        if (goingUp) transform.Translate(transform.up * bobSpeed * Time.deltaTime);
        else transform.Translate(transform.up * bobSpeed * Time.deltaTime * -1);

        //check if should switch direction
        float displacement = Vector3.Distance(transform.position, basePosition);
        if (displacement >= maxDisplacement) goingUp = !goingUp;
    }

    void MoveToFinger()
    {
        //check if user touched screen
        if (Input.touchCount > 0)
        {
            destination = Input.GetTouch(0).position;
            destination.z = depth;
        }

        if (destination != null && Vector3.Distance(destination, basePosition) > distanceThreshold)
        {
            Vector3 movementDirection = (destination - basePosition).normalized;
            print(movementDirection);
            transform.Translate(movementDirection * moveSpeed * Time.deltaTime);
            basePosition += movementDirection * moveSpeed * Time.deltaTime;
        }
    }
}