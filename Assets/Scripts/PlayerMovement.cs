using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    //[SerializeField] private int speed;
    [SerializeField] private int turnSpeed;

    Rigidbody rb;
    Animator anim;
    AudioSource audioSteps;
    Vector3 movement;   // Stores the movement direction
    float horizontal,
            vertical;   


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        audioSteps = GetComponent<AudioSource>();
    }
    private void Update()
    {
        InputPlayer();
        Animating();
        AudioSteps();
    }
    void FixedUpdate()
    {       
        //Movement();
        Rotation();        
    }
    private void OnAnimatorMove()
    {
        // *animator.deltaPosition.magnitude --> How much the character has moved from frame to frame 
        // animator.deltaPosition.magnitude --> would be equivalent to(speed* Time.deltaTime) in case
        // of normal movement
        rb.MovePosition(transform.position + (movement * anim.deltaPosition.magnitude));
    }
    void InputPlayer()
    {
        //Store user input as a movement vector
        horizontal = Input.GetAxis("Horizontal");//AD
        vertical = Input.GetAxis("Vertical");//WS
        movement = new Vector3(horizontal,0,vertical).normalized;        
    }
    void Animating() 
    {
        anim.SetBool("IsMoving", (horizontal != 0 || vertical != 0));     
    }
    void Rotation()
    {
        Vector3 desiredForce = Vector3.RotateTowards(transform.forward,movement,turnSpeed*Time.fixedDeltaTime,0f);

        Quaternion rotation = Quaternion.LookRotation(desiredForce);

        rb.MoveRotation(rotation);
    }
    //void Movement()
    //{
    //    //Apply the movement vector to the current position, which is
    //    //multiplied by deltaTime and speed for a smooth MovePosition
    //    rb.MovePosition(transform.position + movement * speed * Time.fixedDeltaTime);
    //}
    void AudioSteps()
    {
        if(horizontal != 0 || vertical != 0)
        {
            if (!audioSteps.isPlaying)
                audioSteps.Play();
        }
        else
            audioSteps.Stop();
    }
}
