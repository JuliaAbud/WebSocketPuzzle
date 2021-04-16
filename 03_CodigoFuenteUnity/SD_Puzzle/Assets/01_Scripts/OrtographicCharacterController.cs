using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrtographicCharacterController : MonoBehaviour
{
    float speed =   3f;

    Vector3 forward,right;
    Rigidbody rb;
    
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        forward = new Vector3(-1,0,1);
        right = new Vector3(1,0,1);
    }

    void Update()
    {
        move();
    }

    void move()
    {
        //Vector3 direction = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
        Vector3 rightMovement = right * Input.GetAxis("Horizontal");//right * speed * Time.deltaTime * Input.GetAxis("Horizontal");
        Vector3 forwardMovement = forward * Input.GetAxis("Vertical");//forward * speed * Time.deltaTime * Input.GetAxis("Vertical");
        Vector3 heading = Vector3.Normalize(rightMovement + forwardMovement);
        rb.velocity = heading*speed;
        //this.transform.position+=rightMovement;
        //this.transform.position+=forwardMovement;
        if(heading!=Vector3.zero){
            this.transform.forward = heading;
        }  
    }
}
