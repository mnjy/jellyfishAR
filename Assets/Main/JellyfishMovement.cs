using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JellyfishMovement : MonoBehaviour {

    //For bobbing
    public float maxDisplacement;
    public float bobSpeed;
    Vector3 basePosition; //position it should return to
    bool goingUp;

    float currTime;
    float startDelay;

    //For moving to/from finger
    Vector3 startingPosition;
    public Vector3 destination = Vector3.zero;
    float depth = 2f;
    public float moveSpeed;
    float distanceThreshold = 2f;
    public float maxTimeToSpendAtFingerPos = 2f;
    float timeSpentAtFingerPos = 0;
    
    public Text text;

    void Start () {
        basePosition = transform.position;
        goingUp = true;
        startingPosition = transform.position;
        text.text = startingPosition.ToString();

        //add a random delay so that they don't all start at the same time
        currTime = 0;
        startDelay = Random.Range(0, 2f);
	}
	
	void Update () {
        Bob();
        MoveToOrFromFingerPos();
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
    
    void MoveToOrFromFingerPos()
    {
        //check if user touched screen
        if (Input.touchCount > 0)
        {
            Vector3 touchInput = Input.GetTouch(0).position;
            touchInput.y = Screen.height - touchInput.y;
            touchInput.z = basePosition.z;
            destination = Camera.main.ScreenToWorldPoint(touchInput);
            destination.z = basePosition.z;
            timeSpentAtFingerPos = 0;
            text.text = startingPosition + "; " + touchInput + "; " + destination + ";";
        }

        if (destination == Vector3.zero)
        {
            return; //user has not touched screen yet, no action
        } else if (Vector3.Distance(destination, basePosition) > distanceThreshold)
        {
            Vector3 movementDirection = (destination - basePosition).normalized;
            transform.Translate(movementDirection * moveSpeed * Time.deltaTime, Space.World);
            basePosition += movementDirection * moveSpeed * Time.deltaTime;
        } else
        {
            timeSpentAtFingerPos += Time.deltaTime;
            if (timeSpentAtFingerPos > maxTimeToSpendAtFingerPos)
            {
                //go back to starting position
                destination = startingPosition;
                //reset timer
                timeSpentAtFingerPos = 0;
            }
        }
    }
}