using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Ghost : MonoBehaviour
{
    public GameManager gameManager; // InterCommunication scripts

    public Transform[] positions;   // Pos. array for the enemy's patrol
    public float speed,
                turnSpeed;

    Vector3 posToGo;                // This var. stores the target position
    int i;                          // Index to control in which array's pos I am

    bool upwardArray;

    bool canRotate;
    Vector3 rotateDirection;        // Stores the rotation direction

    Ray ray;
    RaycastHit hit;
    public float rayLength;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Configure globally the invariant culture
        System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
    }

    void Start()
    {
        i = 0;
        posToGo = positions[i].position;
        canRotate = false;

        upwardArray = true;  // Set upward array sense
    }


    void Update()
    {        
        Move();
        //Rotate();
        CheckEnableRotation();
    }
    private void FixedUpdate()
    {
        // Set 1m offset respect from the y-axis
        ray.origin = new Vector3(transform.position.x,
                                transform.position.y+1,
                                transform.position.z);
        ray.direction = transform.forward;

        if (Physics.Raycast(ray, out hit, rayLength))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Quieto ahi!");
                gameManager.isPlayerCaught = true;
            }                            
        }

        Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red);

        UpdatePosition();
        Rotation();
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, posToGo, speed * Time.deltaTime);
    }
    void UpdatePosition()
    {        
        if (Vector3.Distance(transform.position, posToGo) <= Mathf.Epsilon)
        {
            // Updates the posToGo Index            
            if (i >= (positions.Length - 1))
            {
                upwardArray = false;                // Inverse movement sense
                i += upwardArray ? 1 : -1;
            }                
            else if (i <= 0)
            {
                upwardArray = true;                // Inverse movement sense
                i += upwardArray ? 1 : -1;
            }
            else
                i += upwardArray ? 1 : -1;
            
            // Updates the posToGo
            posToGo = positions[i].position;
            // Updates the rotation direction
            //rotateDirection = -transform.forward;
            rotateDirection = (posToGo-transform.position).normalized;
            // Enables the rotation flag
            canRotate = true;            
        }        
    }
    void Rotate()
    {
        transform.LookAt(posToGo);
    }
    void Rotation()
    {
        if (canRotate)
        {
            Vector3 desiredForce = Vector3.RotateTowards(transform.forward, rotateDirection, turnSpeed * Time.fixedDeltaTime, 0f);            

            // Calculates the rotation
            Quaternion rotation = Quaternion.LookRotation(desiredForce);
            rb.MoveRotation(rotation);
        }
    }
    void CheckEnableRotation()
    {
        float dot = Vector3.Dot(transform.forward.normalized, rotateDirection.normalized);

        if (dot > 0.999f) 
            canRotate = false;      // Reset the Rotation flag    
    }
}