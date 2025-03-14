## 2. Core Gameplay

### 2.4 Network Authority

Se añade botón de conexión como Host

Se cambia la autoridad sobre los NetworkTransform del Prefab Player para que sea del Owner.

### 2.5 Player Movement

Script para mover la base del jugador.

RigidBody2D al prefab del player. Interpolar para que no hay saltos en el movimiento.

Se crea el fondo.

### 2.6 Player Aiming

Modificar InputReader para añadir la acción de seguir el cursor del ratón.

Crear `PlayerAiming.cs` para seguir el cursor del ratón con la torreta.

### 2.7 Networked Projectiles

Crear el Prefab ProjectileBase con dos variantes:

- ProjectileClient (Con un sprite 2D para la bala)

- ProjectileServer

En el ProjectileBase hay dos scripts `LifeTime.cs` y `DestroySelfOnContact`

### 2.8 Firing Projectiles

Nueva `Layer` `Projectile`

Revisar matriz de colisiones para Física y para Física2D.

Se crea un script `ProjectileLauncher` para disparar: crea balas que solo ven los clientes a nivel local e invocan la creación de las balas en el servidor.

Este script se asocia al prefab Player.

### 2.9 Firing Improvements

Se modifica el prefab Player:

- añadir un componente con un sprite para mostrar la explosión en el cañón (muzzleFlash).

Modificar el código de ProjectileLauncher para:

- Añadir retardo entre disparos

- Evitar que la bala colisione con el propio tanque.

- Proporcionar la velocidad de la bala y que se mueva utilizando el Rigidbody2D de la misma.

### 2.10 Health Component

- Se crea un script Health.cs para gestionar la salud de cualquier componente de nuestro juego que la requiera.

- Se modifica el prefab Player para añadir el componente Health

- Usa una variable de red para guardar el valor

- Se crea un evento OnDie para anunciar que el gameobject tiene su vida a cero.

### 2.11 Health Display

- Se crea un script HealthDisplay.cs para gestionar la barra de salud.

Este script se suscribe al evento HandleHealthChanged para modificar de forma visual la salud del jugador.

- Se crea un pequeño Canvas en el Player para mostrar su salud.

### 2.12 Dealing with Damages

- Modificado ProjectileLauncher para que cuando se dispara la bala en el servidor se le asigne el dueño de la bala adecuado.

- Se crea DealDamageOnContact.cs para aplicar daño a un jugador enemigo cuando la bala colisiona con él.

### 2.13 Coins

- Se crea un Prefab CoinBase que sirve como modelo para las distintas monedas

- Se crea una variante del Prefab anterior llamada RespawningCoin

- Se crea un script Coin.cs (abstracto) para la funcionalidad general de una moneda:

    - Método Collect para coleccionar la moneda
    - Método SetValue para asignar valor a la moneda
    - Método Show para mostrar la moneda

- Se crea un script RespawningCoin que hereda de Coin e implementa el método Collect para este tipo de monedas.

### 2.14 Coin Wallet

- Modificado Coin.cs para asignar un valor a las monedas.
- Creado un monedero para que cada jugador lleve cuenta de las monedas recogidas CoinWallet.cs
- Modificado el Prefab Player para añadir el monedero.

### 2.15 Coin Spawner

- Se crea una Layer nueva 'Pickup' y se asigna esa capa al Prefab CoinBase
- Se añade un GameObject nuevo CoinSpawner con un nuevo componente CoinSpawner.cs y NetworkObject
- Se modifica la matriz de Física 2D para detectar colisiones entre Pickup y Default
- Se crea un script CoinSpawner.cs para generar monedas de forma aleatoria y evitando que colisionen con objetos ya situados en el mapa
- Hay que observar como el CoinSpawner se suscribe al evento OnCollected de cada moneda

### 2.16 Map Design

- Se crea el mapa de juego

###     

- Nuevo Prefab DustCloud para mostrar explosión de la bala al chocar
- Nuevo script SpawnOnDestroy para hacer aparecer un objeto cuando otro es destruído
- Modificado ProjectileLauncher:
    - Se cambia la forma de medir el tiempo entre disparos.
    - Se añade un coste para disparar, es necesario tener monedas recolectadas para hacerlo.

## 3. Connecting Online

### 3.2 Main Menu

- Se crea una escena nueva para el Menú principal
- Se renombra la escena SampleScene a Game

### 3.3 Application Controller

- Estructura básica de la aplicación de Networking
- Ver imagen

### 3.4 Authentication

- Nueva escena `NetBootstrap` que muestra el mensaje `Loading ...`
    - Gameobject NetworkManager
    - Gameobject ApplicationController con el script ApplicationController asociado.
- Prefab `ClientManager`
- Prefab `HostManager`
- Se modifica el script `AuthenticationController` para usar la autenticación anónima de los servicios de Unity. Si la autenticación es correcta como cliente se abre la escena `Menu`.
- Se crea `AuthenticationWrapper.cs` para gestionar la autenticación de los servicios de Unity. Se define una clase estática y un tipo enumerado.
- Se actualiza `ClientGameManager.cs` para utilizar `AuthenticationWrapper` y cargar la escena si consigue autenticarse.

### 3.5 Auth Improvements

-  Se modifica AuthenticationWrapper para controlar algunos errores en la autenticación.

### 3.6 Relay Service Setup

- Proyecto conectado con Unity Cloud y con Relay configurado
- NetworkManager se cambia para utilizar Unity Relay como transporte en vez de Unity Transport

### 3.7 Allocating a Relay

- En la escena Menu, en el HostButton se asocia el método StartHost() del script `MainMenu` para conectar de forma asíncrona al Relay del Cloud de Unity.
- En ApplicationController.cs se cambia el orden de creación de los singletons en LaunchInMode para crear primero el del host y después el del cliente.
- HostGameManager.cs:
    - Nuevo método `StartHostAsync`:
        - Crea un nuevo Allocation en el Relay de Unity Cloud
        - Obtiene el `joinCode` de esa nueva localización
        - Modifica el protocolo de comunicación de red para utilizar `udp`
        - Inicia un nuevo host
        - Carga la escena de juego

### 3.8 Joining a Relay

- En la escena Menu, en el ClientButton se asocia el método StartClient() del script `MainMenu` para conectar de forma asíncrona al Relay del Cloud de Unity. Es necesario escribir el `Join Code` para conectar a un host previamente lanzado.
- MainMenu.cs:
    - Se añade una referencia al campo de texto del `joinCode`.
    - Se añade un método `StartClient` que utilizando el singleton de cliente y el código se une a la partida existente.
- HostGameManager.cs
    -Se cambia el protocolo `udp` por `dtls` que es mucho más seguro.
- ClientGameManager.cs
    - Nuevo método `StartClientAsync` para intentar conectarse a una partida existente.
    - Inicia el cliente.

### 3.9 Lobbies UI

- LobbiesBackground Prefab para mostrar una ventana con la lista de lobbies a los que poder unirse en una partida.
- LobbyItem Prefab para mostrar cada uno de los elementos de la lista anterior.
- Se añade un gameobject de LobbiesBackground en el canvas de la escena `Menu`

### 3.10 Creating Lobbies

- Se modifica `HostGameManager` para crear un `Lobby` público justo antes de crear el Host.
- También se crea una corutina para mantener vivo el Lobby mandando una señal cada pocos milisegundos.

### 3.11 Joining Lobbies

- Nuevo LobbiesList.cs
    - Para mostrar la lista de lobbies disponibles para conectarse con el método `RefreshList`.
        - Lobbies con espacios disponibles y no bloqueados (filtro).
        - Crea elementos del prefab `LobbyItem` para rellenar la UI.
    - Método para conectarse a un Lobby `JoinAsync`
- Nuevo LobbyItem.cs
    - Inicializa un prefab `LobbyItem` con los textos adecuados, método `Initialise`.
    - Método `Join` para conectarse a un Lobby que utiliza el método `JoinAsync` del script anterior.

### 3.12 Player Name Selection

- Nueva escena `Bootstrap`
    - Sirve para introducir el nombre del juegador y acceder a la escena `NetBootstrap`.
    - Se edita la lista de escenas para que sea la primera.
    - Campo de texto para introducir el nombre, guarda el nombre del jugador cada vez que se modifica algo en él.
    - Botón para conectar.
    - Gameobject `NameSelector` con un script asociado `NameSelector.cs`.
- Nuevo script `UI/NameSelector.cs`:
    - Método `Start`
        - Si es el servidor carga la siguiente escena directamente.
        - Si no es el servidor obtiene el nombre del jugador de `PlayerPrefs`
    - Método `HandleNameChanged` para gestionar el cambio de nombre
    - Método `Connect` que guarda el nombre del jugador en `PlayerPrefs` y carga la siguiente escena.

### 3.13 Connection Approval

- En la escena `NetBootsrap` en el NetworkManager se marca la opción de que ser requiere `Connection Approval` para los clientes.
- Nueva clase UserData.cs para enviar información del usuario a través de la red.
- Nueva clase `NetworkServer.cs`
    - Con un constructor para acceder al networkManager del juego y registrar una función que será llamada cuando se requiera la aprobación de la conexión de un usuario `ApprovalCheck`;
    - Una función `ApprovalCheck`:
        - Recibe una solicitud con los datos de usuario y los decodifica.
        - Por ahora aprueba siempre al usuario recibido e indica que el prefab player debe ser instanciado para este cliente.
- Modificado `ClientGameManager` para enviar la información del usuario mediante `UserData` antes de hacer `StartClient`.
- Modificado `HostGameManager`:
    - para enviar la información del usuario mediante `UserData` antes de hacer `StartHost`.
    - para utilizar el nombre del usuario cuando se crea un `Host`.

### 3.14 Handling Connections

- Modificada la clase `UserData` para guardar un nuevo atributo `userAuthId` que almacena la identificación de red de cada cliente conectado.
- Modificado `HostGameManager` para enviar el `userAuthId` antes de crear un `Host`.
- Nueva clase `NetworkClient`:
    - Hace uso del NetworkManager al cual accede mediante su constructor.
    - Se suscribe al evento que sucede cuando un cliente se desconecta y lo gestiona cargando la escena de menú nuevamente y limpiando el networkmanager correspondiente.
- Mddificado `ClientGameManager`:
    - Utiliza un `NetworClient` descrito anteriormente
    - para enviar el `userAuthId` antes de crear un `Cliente
- Modificado `NetworkManager`
    - Creado un diccionario para asociar el identificador en red de un cliente con el identificador del sistema de autenticación de Unity Cloud.
    - Creado un segundo diccionario para asociar el identificador del sistema de autenticación de Unity Cloud con los datos de usuario en la forma `UserData`.
    - Cuando se aprueba un cliente se añade a los diccionarios.
    - Cuando se desconecta un cliente se borra de los diccionarios.

### 3.16 Shutting Down Cleanly

- Se crea la carpeta `Editor` con un script `StartupSceneLoader` para cargar siempre la escena inicial cuando le damos a `Play` desde el editor.
- Se implementa la interfaz `IDisposable` con el método `Dispose`para hacer limpieza cuando salimos del juego en las siguientes clases:
    - ClientGameManager
    - NetworkClient
    - HostGameManager
    - NetworkServer
- Se implementa el método OnDestroy para hacer limpieza en las siguientes clases:
    - ClientSingleton
    - HostSingleton

### 3.17 WebGL Setup

- Se cambia el protocolo de comunicación a wss para WebGL
- Se habilitan los WebSockets en Unity Transport

### 4.2 Camera Player

- Instalar el paquete Cinemachine
- Modificado prefab player con gameobject `FollowCam`
    - Es un componente Cinemachine Camera
    - Se habilita la prioridad a 10
    - En el Tracking Target se pone el Player
    - En Ortographic Size se ponde 12
- Modificado prefab player con componente `TankPlayer.cs` para dar mayor prioridad a la propia camara porque habrá tantas cámaras como jugadores conectados.
- Nuevo script para priorizar la propia cámara `TankPlayer.cs`

### 4.3 Overhead Names

- Prefab Player
    - Se añade un campo de texto para mostrar el nombre del jugador
    - se añade una variable de red `PlayerName` en `TankPlayer`a la que se le asigna valor en el servidor mediante el identificador de red (`GetUserDataByClientId`).
- Nuevo script (`PlayerNameDisplay`) asociado al canvas de Player suscrito a la variable de red `PlayerName`.
- Modificado `NetworkServer.cs` para crear el método `GetUserDataByClientId`

### 4.4 Spawn Points

- Escena `Game`
    - Nuevo GameObject `SpawnPoints` cuyos hijos son los puntos del mapa donde aparecen los jugadores.
- Nuevo `SpawnPoint.cs`
    - Cuando se habilita se añade a una lista de spawnpoints (atributo estátioo)
    - Método estático `GetRandomSpawnPos` para obtener una posición aleatoria entre los posibles spawnpoints de la escena.
- Modificado `NetworkServer.cs` para que cuando se valida la conexión de un cliente se le asigna una posición aleatoria entre los spawnpoints posibles.

### 4.5 Respawn

- Modificado `TankPlayer.cs`:
    - Nuevo campo `health` para referirse a la salud del jugador.
    - Se definen dos eventos nuevos para anunciar al sistema cuando un jugador es instanciado en la red y cuando es desinstanciado.
        - `OnPlayerSpawned`
        - `OnPlayerDespawned`
    - Estos eventos son invocados solo por el servidor

- Nuevo `RespawnHandler.cs` asociado a un GameObject `RespawnHandler` en la escena `Game`:
    - Cuando un jugador aparece en la escena se suscribe a su evento `OnDie`.
    - Cuando el jugador anuncia que muere destruye el gameobject y lo vuelve a crear mediante `RespawnPlayer` con el mismo identificador de red.