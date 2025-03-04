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