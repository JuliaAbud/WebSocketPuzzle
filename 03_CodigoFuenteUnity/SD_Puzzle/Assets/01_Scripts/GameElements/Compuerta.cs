using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compuerta : MonoBehaviour
{
    public BotonPeso[] botones;
    public Color colorAsignado;
    public Renderer renderer;
    Animator animator;
    
    public void Start(){
        renderer.material.SetColor("_BaseColor",colorAsignado);
        animator = this.GetComponent<Animator>();
        foreach (BotonPeso b in botones){
            b.AgregarCompuerta(this);
        }
    }

    public void checarCompuerta(){
        int i = 0;
        foreach (BotonPeso b in botones){
            if (b.activado)
                i+=1;
        }
        //revisa que todos los botones esten activos
        if(i == botones.Length){
            //print("bajar puerta");
            animator.SetBool("Activada",false);
        }
        else{
            //print("subir puerta");
            animator.SetBool("Activada",true);
        }
    }
}
