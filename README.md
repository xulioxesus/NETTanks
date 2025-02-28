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