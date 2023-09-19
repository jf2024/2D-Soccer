using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftKick : MonoBehaviour
{
    public bool isKicking = false;
    float timer = 0.0f;
    private Quaternion startingRot;

    public Rigidbody2D ballRigidbody; 

    //play around with these 
    public float kickForce = 18.0f;
    public float kickDistance = 1.3f;
    public float kickDuration = 0.10f;

    private Transform ballTransform;
    public AudioSource audioSource;


    void Start()
    {
        Time.timeScale = 1;

        startingRot = this.transform.rotation;
        ballTransform = GameObject.FindWithTag("Ball").transform;
        ballRigidbody = ballTransform.GetComponent<Rigidbody2D>();

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Kick2") && !isKicking)
        {
  
                //Debug.Log("Start Kick");
                isKicking = true;
                timer = 0.0f;
                audioSource.PlayOneShot(audioSource.clip, 1f);

            if (Vector2.Distance(transform.position, ballTransform.position) < kickDistance)
            {
                Vector2 kickForceStrength = (ballTransform.position - transform.position).normalized * kickForce;
                ballRigidbody.AddForce(kickForceStrength, ForceMode2D.Impulse);
                //Debug.Log("kicking hard");
            }

        }

        if (isKicking)
        {

            this.transform.eulerAngles += new Vector3(0f, 0f, Time.deltaTime * 750); //might * by a constant
            timer += Time.deltaTime;

            if (timer >= kickDuration) //play around with .3
            {
                isKicking = false;
                this.transform.rotation = startingRot;
            }
        }
    }
}
