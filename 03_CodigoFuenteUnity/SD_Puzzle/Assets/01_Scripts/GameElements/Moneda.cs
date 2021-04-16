using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moneda : MonoBehaviour
{
     void OnTriggerEnter(Collider collider){
        if(collider.transform.GetComponent<Propiedades>()!=null){
            if(collider.transform.GetComponent<Propiedades>().character){
                GameManager.Instance.ChangeGameState(GameManager.GameState.Game_Win);
            }
        }
    }
}
