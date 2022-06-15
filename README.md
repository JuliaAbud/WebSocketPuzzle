# Asymmetric Puzzle 
WebSocket Multiplayer Puzzle
  
## Objetivos principales
|![Implementar Websocket](images/00_implementarwebsocket.PNG)|![](images/01_utilizarcifradoSSL.PNG)|![](images/02_nodejsWebservice.PNG)|![](images/03_InterconexionServidores.PNG)|![](images/04_ClienteSuplantador.PNG)|
| ------------- | ------------- | ------------- | ------------- | ------------- |
| Implementar Websocket | Utilizar Cifrado SSL | NodeJS como webservice | Interconexión de servidores | Cliente suplantador |
| Para la comunicación de clientes y servidores | Para comunicación segura | Se encarga del stream de datos al servidor | Conexión en malla para 3 servidores con redundancia | Para simular suplantación de identidad |

## Conexiones
![](images/06_Conexiones.PNG)

## Elementos de juego
Los tres jugadores deben trabajar en equipo para completar cada uno de los tres niveles. Cada nivel se gana llegando a la moneda
Multijugador: para exactamente 3 jugadores
Tipo de juego: puzzle
Vista: isométrica
![](images/07_GameElements.png)

## Diseño de niveles
| ![](images/nivel1.PNG)| ![](images/nivel2.PNG)| ![](images/nivel3.PNG)| 
| ------------- | ------------- | ------------- | 
| Nivel 1 | Nivel 2 | Nivel3 | 
| 1. Requiere que los 3 jugadores empujen la caja hacia el botón naranja | 1. Requiere que un jugador se ponga sobre el botón azul para abrir compuerta azul | 1. Los 3 jugadores deben empujar una de las cajas hacía el botón azul para abrir compuerta azul |
| 2. Se abre compuerta naranja | 2. Y que los otros 2 jugadores empujen la caja hacia el botón naranja | 2. Requiere que dos jugadores se ubiquen en los botones naranjas para abrir compuerta naranja |
| 3. Tomar moneda | 3. Se abre compuerta naranja |  3. Y que el jugador restante empuje la caja hacia el botón morado |
|                 | 4. Tomar moneda | 4. Se abre compuerta morada |
| | | 5. Tomar moneda | 


 





## Sobre el equipo
Proyecto realizado para la materia de Ingeniera de Software para la maestría de Ciencias de la Computación en CINVESTAV Guadalajara. Entrega Diciembre 2020.
* Profesor: 
  * Dr. Félix Francisco Ramos Corchado
* Equipo de desarrollo:
  * Lic. Julia Alejandra Rodríguez Abud
  * Mtro. Carlos Cárdenas Ruiz
  * Ing. Gustavo Orozco
