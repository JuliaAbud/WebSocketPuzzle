using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int fuerzaRequerida  =    3;
    public int fuerzaAplicada = 0;
    Rigidbody rb;
    RigidbodyConstraints originalConstraints;

    void Start(){
        rb = this.GetComponent<Rigidbody>();
        originalConstraints = rb.constraints;
    }

     void OnTriggerEnter(Collider collider){
        if(collider.transform.GetComponent<Propiedades>()!=null){
            if(collider.transform.GetComponent<Propiedades>().fuerza>0){
                fuerzaAplicada+=collider.transform.GetComponent<Propiedades>().fuerza;
                UpdateActivationOfMovement();
            }
        }
    }
    void OnTriggerExit(Collider collider){
        if(collider.transform.GetComponent<Propiedades>()!=null){
            if(collider.transform.GetComponent<Propiedades>().fuerza>0){
                fuerzaAplicada-=collider.transform.GetComponent<Propiedades>().fuerza;
                UpdateActivationOfMovement();
            }
        }
    }
/*
    void OnCollisionEnter(Collision collision){
        if(collision.transform.GetComponent<Propiedades>()!=null){
            if(collision.transform.GetComponent<Propiedades>().fuerza>0){
                fuerzaAplicada+=collision.transform.GetComponent<Propiedades>().fuerza;
                UpdateActivationOfMovement();
            }
        }
    }
    void OnCollisionExit(Collision collision){
        if(collision.transform.GetComponent<Propiedades>()!=null){
            if(collision.transform.GetComponent<Propiedades>().fuerza>0){
                fuerzaAplicada-=collision.transform.GetComponent<Propiedades>().fuerza;
                UpdateActivationOfMovement();
            }
        }
    }
*/
    void UpdateActivationOfMovement(){
        if(fuerzaAplicada >= fuerzaRequerida){
            rb.constraints =    originalConstraints;
        }
        else{
            rb.constraints =    RigidbodyConstraints.FreezeAll;
        }
    }
}
