using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Globalization;
using System.Linq;

public class GameManager : MonoBehaviour
{
    //SIMPLE SINGLETON PATTERN---------------------
    private static GameManager _instance;
    public static GameManager Instance {
        get{
            return _instance;
        }
    }  
    private void Awake() {
        //frames per second control
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 45;

        if(GameManager._instance==null){
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else{
            Destroy(this.gameObject);
        }
    } 
    //----------------------------------------------
    public bool testing = false;
    public GameState currentGameState = GameState.Game_Start;
    public string OriginalUniqueID;
    public string UniqueID;
    public int mySlotjugadores;
    public string myName = "Name";
    public Color myColor = Color.white;
    Transform[] spawnPoint;
    GameObject waitMsg;

    [System.Serializable]
    public struct Jugador{
        public Transform t_character;
        public Renderer renderer;
        public TextMeshPro nameText;
        public string id;
        public string name;
        public Vector3 lastPos;
        public Vector3 lastRot;
        public int level;
        public Color color;
    }
    public Jugador[] jugadores;   

    ///-----------------------------------------------------------------------ESTADOS
    public enum GameState{
        Lobby,
        Game_Start,
        Game_Play,
        Game_Win,
        Game_End
    }  

    public void Start(){
        waitMsg = GameObject.Find("EsperarMsg");
        if(SceneManager.GetActiveScene().buildIndex==0){
            currentGameState=GameState.Lobby;
        }
        else{
            currentGameState=GameState.Game_Start;
        }
        ChangeGameState(currentGameState);
    }

    public void ChangeGameState(GameState gs){
        currentGameState = gs;
        StartCoroutine(gs.ToString()+"_state");
    }
    
    IEnumerator Lobby_state(){
        waitMsg.SetActive(false);

        OriginalUniqueID = SystemInfo.deviceUniqueIdentifier;
        UniqueID = OriginalUniqueID;
        MessageControl.Instance.ResetValueInputFieldUniqueID();

        while(currentGameState==GameState.Lobby){
            yield return null;
        } 
        //A que nivel se cargará
        int lvl = returnCorrectLevel();
        if (lvl==0){
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
        else{
            SceneManager.LoadScene(lvl, LoadSceneMode.Single);
        }
        yield return null;
    }

    IEnumerator Game_Start_state(){
        //Mientras sea el lobby esperar
        while(SceneManager.GetActiveScene().buildIndex==0){ 
            yield return null;  
        }
        waitMsg.SetActive(true);
        //actualizacion de pos al spawn point (si ya esta unido el usuario al iniciar el nivel)
        spawnPoint = GameObject.Find("SpawningPoint").transform.GetComponentsInChildren<Transform>();
        for (int i=0; i<jugadores.Length;i++){
            if(jugadores[i].id!= null & jugadores[i].id!="" & jugadores[i].id!="0"){
                jugadores[i].t_character.position = spawnPoint[i].position;
            }
        }  
        //Generarme a mi mismo como jugador (al entrar a escena actualizar nivel)
        InfoJugadorActualizar(UniqueID,myName,"","",SceneManager.GetActiveScene().buildIndex.ToString(), myColor.ToString());
        for (int i=0; i<jugadores.Length;i++){
            if(jugadores[i].id == UniqueID){
                mySlotjugadores = i;
            }
        }         
        while(currentGameState==GameState.Game_Start){
            //simular entrada de Jugadores
            if (Input.GetKeyDown(KeyCode.Space) & testing){
                InfoJugadorActualizar(Random.Range(1,100).ToString(),"otro","","", jugadores[mySlotjugadores].level.ToString(), Color.green.ToString());
            }  
            //inicia el juego cuando 3 jugadores esten connectados
            int connectedPlayers = 0;
            foreach (Jugador j in jugadores){
                if (j.id != null & j.id != "" & j.id != "0")
                    connectedPlayers+=1;
            }
            if(connectedPlayers==3)
                ChangeGameState(GameState.Game_Play);
            yield return null;
        }
        waitMsg.SetActive(false);
        yield return null;
    }

    IEnumerator Game_Play_state(){
        jugadores[mySlotjugadores].t_character.GetComponent<OrtographicCharacterController>().enabled = true;
        jugadores[mySlotjugadores].t_character.GetComponent<Rigidbody>().isKinematic = false;
        while(currentGameState==GameState.Game_Play){
            //guarda mi informacion
            jugadores[mySlotjugadores].lastPos = jugadores[mySlotjugadores].t_character.transform.position;
            jugadores[mySlotjugadores].lastRot = jugadores[mySlotjugadores].t_character.transform.rotation.eulerAngles; 
            //update position and rotation of teammates
            for  (int i=0; i<jugadores.Length;i++){
                if(i!=mySlotjugadores){
                    if(testing){
                        jugadores[i].t_character.GetComponent<OrtographicCharacterController>().enabled = true;
                        jugadores[i].t_character.GetComponent<Rigidbody>().isKinematic = false;
                    }
                    if(!testing){
                       UpdateVisuals(i);
                    }
                }
            }
            //checa si alguien mas pasó a otro nivel
            if(returnCorrectLevel()>SceneManager.GetActiveScene().buildIndex){
                ChangeGameState(GameState.Game_Win);
            }
            yield return null;
        }
        yield return null;
    }

    public int returnCorrectLevel(){
        int totalScenes     = SceneManager.sceneCountInBuildSettings;
        int current     = SceneManager.GetActiveScene().buildIndex; 
        int maxlevel    = 0;
        foreach (Jugador j in jugadores){
            if (j.level>maxlevel){
                maxlevel = j.level;
            }
        }
        return maxlevel;
    }

    IEnumerator Game_Win_state(){
        print("WIN");
        int totalScenes     = SceneManager.sceneCountInBuildSettings;
        int currentScene    = SceneManager.GetActiveScene().buildIndex;
        int nextScene       = (currentScene+1)%totalScenes;
        jugadores[mySlotjugadores].level  = nextScene;
        
        //CargarEscena
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single); 
        while(currentGameState==GameState.Game_Win){
            //Cuando termina de cargar la escena
            if(SceneManager.GetActiveScene().buildIndex==nextScene){
                if(nextScene==(totalScenes-1)){
                    ChangeGameState(GameState.Game_End);
                }
                else{
                    ChangeGameState(GameState.Game_Start);
                }
            }
            yield return null;
        }
        yield return null;
    }

    IEnumerator Game_End_state(){
        while(currentGameState==GameState.Game_End){
            yield return null;
        }
        yield return null;
    }

    //-------------------------------------------------------------------------------------
    void UpdateVisuals(int i){
        //actualiza posicion y rotacion
        jugadores[i].t_character.position = jugadores[i].lastPos;
        jugadores[i].t_character.eulerAngles = jugadores[i].lastRot;
        //Pone nombre de jugador
        jugadores[i].nameText.text = jugadores[i].name;
        //Color a jugador
        jugadores[i].renderer.material.SetColor("_Color", jugadores[i].color);
    }

    public void InfoJugadorActualizar(string currentId, string currentName, string pos, string rot, string nivel, string color){
        //pos y rot contienen informacion (Si existe, pero no somos nosotros)
        if(currentId!=UniqueID){
            for  (int i=0; i<jugadores.Length;i++){
                if(jugadores[i].id == currentId){
                    jugadores[i].name = currentName;
                    jugadores[i].level = int.Parse(nivel, CultureInfo.InvariantCulture);
                    //int.TryParse(nivel, out jugadores[i].level);
                    jugadores[i].color = StringToColor(color);
                    jugadores[i].lastPos = StringToVector3(pos);
                    jugadores[i].lastRot = StringToVector3(rot);  
                    break;
                }
            }  
        }
        //si hay un slot vacio, y es primera vez que recibimos info de este jugador 
        //(cualquiera, incluso nosotros)
        for  (int i=0; i<jugadores.Length;i++){
            if(jugadores[i].id == null | jugadores[i].id == "" | jugadores[i].id == "0"){
                bool existsId = jugadores.Any(Jugador=> Jugador.id == currentId);
                if(!existsId){
                    jugadores[i].id=currentId;
                    jugadores[i].name=currentName;
                    jugadores[i].level = int.Parse(nivel, CultureInfo.InvariantCulture);
                    //int.TryParse(nivel, out jugadores[i].level);
                    jugadores[i].color = StringToColor(color);
                    jugadores[i].lastPos = spawnPoint[i].position;
                    jugadores[i].lastRot = Vector3.zero;
                    //Hace update de visuales al crear al jugador
                    UpdateVisuals(i);
                }
                break;
            }
        }
    }

    Color StringToColor(string s){
        s                = s.Trim(new char[] {'R','G','B','A', '(' , ')' });
        string[] parts   = s.Split(',');
        Color nColor = new Color(float.Parse(parts[0], CultureInfo.InvariantCulture),
                                        float.Parse(parts[1], CultureInfo.InvariantCulture),
                                        float.Parse(parts[2], CultureInfo.InvariantCulture),
                                        float.Parse(parts[3], CultureInfo.InvariantCulture));
        
        return nColor;
    }

    Vector3 StringToVector3(string s){
        s                = s.Trim(new char[] { '(' , ')' });
        string[] parts   = s.Split(',');
        Vector3 nVector3 = new Vector3(float.Parse(parts[0], CultureInfo.InvariantCulture),
                                        float.Parse(parts[1], CultureInfo.InvariantCulture),
                                        float.Parse(parts[2], CultureInfo.InvariantCulture));
        return nVector3;
    }

}
