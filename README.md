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
