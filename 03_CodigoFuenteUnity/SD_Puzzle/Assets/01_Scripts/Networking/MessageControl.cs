using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MessageControl : MonoBehaviour
{
    public readonly string[] servers = {"192.168.30.52:8081", "192.168.30.53:8081", "192.168.30.54:8081", "192.168.30.53:443", "localhost:8081"  ,"echo.websocket.org/"};

    public TMP_InputField UniqueIDInputField;
    public TextMeshProUGUI UniqueIDMessage;

    public TextMeshProUGUI ConnectionMessage;
    public TMP_Dropdown dropProtocol;
    public TMP_Dropdown dropServers;
    public Button botonConectar;

    public bool orderUpdateUI = false;
    public bool connected = false;
    public string message = "";

    //SIMPLE SINGLETON PATTERN---------------------
    private static MessageControl _instance;
    public static MessageControl Instance {
        get{
            return _instance;
        }
    }  
    private void Awake() {

        dropServers.AddOptions(new List<string>(servers));

        if(MessageControl._instance==null){
            _instance = this;
        }
    } 
    //--------------------------------------MESSAGE CONTROL
    char symbol = '_';

    public void SplitInfoReceived(string s){
        string[] dataParts = s.Split(symbol);
        //id, name, pos, rot, level, color
        GameManager.Instance.InfoJugadorActualizar(dataParts[0], dataParts[1], dataParts[2], dataParts[3], dataParts[4], dataParts[5]); 
    }

    public string ComposeInfoToSend(){
        string inf =   GameManager.Instance.jugadores[GameManager.Instance.mySlotjugadores].id
                +symbol+
                GameManager.Instance.jugadores[GameManager.Instance.mySlotjugadores].name
                +symbol+
                GameManager.Instance.jugadores[GameManager.Instance.mySlotjugadores].lastPos.ToString()
                +symbol+
                GameManager.Instance.jugadores[GameManager.Instance.mySlotjugadores].lastRot.ToString()
                +symbol+
                GameManager.Instance.jugadores[GameManager.Instance.mySlotjugadores].level.ToString()
                +symbol+
                GameManager.Instance.jugadores[GameManager.Instance.mySlotjugadores].color.ToString();
        return inf;
    }
    //-----------------------------------------UI CONTROL
    public void ResetValueInputFieldUniqueID(){
        UniqueIDInputField.text = GameManager.Instance.OriginalUniqueID;
    }
    //Muestra el ID utilizado por este cliente
    public void UpdateUniqueID(string s){
        GameManager.Instance.UniqueID = s;
        if(GameManager.Instance.OriginalUniqueID==GameManager.Instance.UniqueID){
            UniqueIDMessage.text = "UniqueID:"+GameManager.Instance.UniqueID;
            }
        else{
            UniqueIDMessage.text="UniqueID:"+GameManager.Instance.UniqueID+"\n This Unique ID is not the original given by Unity";
        }
        
    }
    //Activado por el boton de 'ENTRAR' en lobby
    public void ChangeToGameState(){
        GameManager.Instance.ChangeGameState(GameManager.GameState.Game_Start);
    }
    //Actualizacion desde el InputField (constante) en lobby
    public void UpdateName(string s){
        GameManager.Instance.myName = s;
    }
    //Actualizacion desde el dropdown color en lobby
    public void UpdateColor(int i){
        if (i==0){
            GameManager.Instance.myColor = Color.white;
        }else if (i==1){
            GameManager.Instance.myColor = Color.red;
        }
    }
    //Por boton 'conectar', requiere de la información del menu dropdown en lobby
    public void Conectar(){
        if(dropProtocol.captionText.text=="Websocket"){
            this.GetComponent<WsClient>().enabled=true;
            this.GetComponent<WsClient>().Connect(dropServers.captionText.text);
        }
        else if (dropProtocol.captionText.text=="Websocket Cifrado"){
            this.GetComponent<WsClientSecure>().enabled=true;
            this.GetComponent<WsClientSecure>().Connect(dropServers.captionText.text);
        }
    }
    //Actualizacion de UI
    public void UpdateConnectionUI(){
        if(!connected){
            ConnectionMessage.text = message;
            dropProtocol.interactable   = true;
            dropServers.interactable    = true;
            botonConectar.interactable  = true;
        }
        else{
            ConnectionMessage.text = message;
            dropProtocol.interactable   = false;
            dropServers.interactable    = false;
            botonConectar.interactable  = false;
        }
    }

    //-----------------------------------------------------------------------------------------------
    void Start(){
        StartCoroutine(UIUpdateCoroutine());
    }

    bool orderUpdate(){
        if(orderUpdateUI==true){
            orderUpdateUI = false;
            return true;
        }
        else{
            return false;
        }
    }

    IEnumerator UIUpdateCoroutine(){
        while(true){
            yield return new WaitUntil(orderUpdate);
            UpdateConnectionUI();
            print("update UI");
        }
    }
}
