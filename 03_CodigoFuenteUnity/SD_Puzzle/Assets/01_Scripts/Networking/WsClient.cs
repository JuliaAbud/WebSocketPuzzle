using UnityEngine;
using WebSocketSharp;
public class WsClient : MonoBehaviour
{
    WebSocket ws;
    string lastServerTried;
    bool tryingReconnection = false;
    int serverWhenTryingreconnection;

    private void Update()
    {
        if(ws == null){
            return;
        }
        if(GameManager.Instance.currentGameState!=GameManager.GameState.Lobby){
            ws.Send(MessageControl.Instance.ComposeInfoToSend());
        }     
    } 
    private void OnApplicationQuit()
    {
        if(ws != null){
            Disconnect();
        }
    }

    //-------------------------------------------------------------------------------

    public void Connect(string server){
        print(server);
        server = "ws://"+ server;
        lastServerTried = server;
        ws = new WebSocket(server);

        ws.OnOpen +=  ( sender,  e )  =>  { 
            Debug.Log  ("Websocket Open "+server); 
            UIData(true,"Websocket Open "+server);
            tryingReconnection = false;
        };
        ws.OnError +=  ( sender ,  e )  =>  { 
            Debug.Log  ( "WebSocket Error Message:"  +  e.Message ); 
        };
        ws.OnClose +=  ( sender ,  e )  =>  { 
            Debug.Log  ( "WebSocket Close " + e.Reason + " --- " + e.Code );  
            UIData(true,"WebSocket Close: " + e.Reason + " --- " + e.Code);    
            Disconnect();   
            ReconnectToAnother();   
        };
        ws.OnMessage += (sender, e) => 
        {
            Debug.Log("Message Received from "+((WebSocket)sender).Url+", Data : "+e.Data);
            MessageControl.Instance.SplitInfoReceived(e.Data);
        };
        ws.Connect();
    }

    void Disconnect()  {   
        print("Close WebSocket");    
        ws = null ; 
        ws.Close();       
    }

    void ReconnectToAnother(){
        int serverInList =  System.Array.IndexOf(MessageControl.Instance.servers,lastServerTried);
        if(tryingReconnection == false){ 
            tryingReconnection = true;
            serverWhenTryingreconnection = serverInList;
        }
        serverInList += 1;
        if(serverInList!=serverWhenTryingreconnection){
            Connect(MessageControl.Instance.servers[serverInList%MessageControl.Instance.servers.Length]);
        }
        else{
            UIData(false,"WebSocket Close: Tried to connect to all the servers without success"); 
        }
    }

    void UIData(bool connected, string message){
        MessageControl.Instance.orderUpdateUI = true;
        MessageControl.Instance.connected = connected;
        MessageControl.Instance.message = message;
    }
}