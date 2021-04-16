using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonPeso : MonoBehaviour
{
    public Renderer botonColor;
    public int pesoRequerido    =   1;
    public int pesoAplicado     =   0;

    public bool activado        =   false;
    [HideInInspector]
    public List<Compuerta> compuertasAsignadas;
    
    public void AgregarCompuerta(Compuerta c){
        compuertasAsignadas.Add(c);
        botonColor.material = c.renderer.material;
    }

    void OnTriggerEnter(Collider collider){
        if(collider.transform.GetComponent<Propiedades>()!=null){
            if(collider.transform.GetComponent<Propiedades>().peso>0){
                pesoAplicado+=collider.transform.GetComponent<Propiedades>().peso;
                UpdateActivation();
            }
        }
    }
    void OnTriggerExit(Collider collider){
        if(collider.transform.GetComponent<Propiedades>()!=null){
            if(collider.transform.GetComponent<Propiedades>().peso>0){
                pesoAplicado-=collider.transform.GetComponent<Propiedades>().peso;
                UpdateActivation();
            }
        }
    }
    void UpdateActivation(){
        if(pesoAplicado >= pesoRequerido){
            activado = true;
        }
        else{
            activado = false;
        }
        foreach(Compuerta c in compuertasAsignadas){
                c.checarCompuerta();
        }
    }
}
