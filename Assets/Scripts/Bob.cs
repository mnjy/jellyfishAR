using UnityEngine;
using System.Collections;

public class Bob : MonoBehaviour {

    public float maxDisplacement;
    public float speed;
    Vector3 basePosition; //position it should return to
    bool goingUp;

    float currTime;
    float startDelay;

    void Start () {
        basePosition = transform.position;
        goingUp = true;

        //add a random delay so that they don't all start at the same time
        currTime = 0;
        startDelay = Random.Range(0, 2f);
	}
	
	void Update () {
        if (currTime < startDelay)
        {
            currTime += Time.deltaTime;
            return;
        }

        //update the position
        if (goingUp) transform.Translate(transform.up * speed);
        else transform.Translate(transform.up * speed * -1);

        //check if should switch direction
        float displacement = Vector3.Distance(transform.position, basePosition);
        if (displacement >= maxDisplacement) goingUp = !goingUp;
    }
}
