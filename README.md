# Pong Arcade — Proyecto Unity C#

## 1. Descripción del proyecto

**Pong Arcade** es una recreación del clásico Pong desarrollada en **C# con Unity**.  
El objetivo del juego es marcar **10 puntos** antes de que el rival lo haga.

El jugador controla una pala manualmente, mientras que la pala contraria está controlada por una lógica programada mediante scripts. No se trata de una IA real, sino de un comportamiento predefinido con distintos niveles de dificultad:

- Fácil
- Intermedio
- Difícil

El proyecto incluye:

- Menú principal
- Selección de dificultad
- Partida principal de Pong
- Pantalla de victoria y derrota
- Sistema de audio con música y efectos
- Navegación entre escenas mediante botones

La música y efectos fueron creados manualmente en **FL Studio** e integrados mediante scripts en Unity.

No se utilizan librerías externas para la lógica del juego. La implementación se basa en:

- Unity
- TextMesh Pro
- Componentes estándar del motor

---

## 2. Mecánicas del juego

### 2.1 Objetivo

El objetivo es llegar antes que la IA al número máximo de puntos configurado en el `GameManager`, que actualmente es **10**.

En resumen:

- El jugador gana al llegar a **10**
- La IA gana al llegar a **10**

### 2.2 Funcionamiento general

La pelota comienza en el centro del campo y sale disparada en una dirección aleatoria.  
Cada vez que entra en una portería:

- Se suma un punto al lado correspondiente
- Se reinician las posiciones de las palas y la pelota
- La pelota vuelve a lanzarse tras un pequeño retraso de **3 segundos**

### 2.3 Dificultad de la IA

Antes de entrar en la partida, el jugador puede elegir entre:

- Fácil
- Intermedio
- Difícil

La dificultad afecta a:

- La velocidad de la pala enemiga
- La precisión del seguimiento de la pelota
- El margen de error del rival

### 2.4 Condición de victoria

La partida termina cuando uno de los dos jugadores alcanza el puntaje objetivo:

- Si gana el jugador, se carga la escena `YouWIN`
- Si gana la IA, se carga la escena `Game Over`

---

## 3. Arquitectura OOP — Mapa de clases

```text
┌────────────────────────────────────────────────────────────┐
│                         Menu / menu2                       │
│                  Escenas de navegación inicial             │
└───────────────┬───────────────────────┬────────────────────┘
                │                       │
                ▼                       ▼
          Jugar.cs          JugarFacil / JugarIntermedio / JugarDificil
      (abre menu2)             (elige dificultad y carga Game)
                                        │
                                        ▼
┌────────────────────────────────────────────────────────────┐
│                    GameDifficultySettings                  │
│               Guarda la dificultad seleccionada            │
└──────────────────────────────┬─────────────────────────────┘
                               │
                               ▼
┌────────────────────────────────────────────────────────────┐
│                        Escena Game                         │
│                    Núcleo de la partida                    │
└───────────────┬────────────────────┬───────────────────────┘
                │                    │
                ▼                    ▼
        GameManager.cs            Ball.cs
    (marcador, reglas,      (movimiento, rebotes,
     victoria y escenas)      goles y reinicio)
                │                    │
                ▼                    ▼
          Paddle.cs              PaddleAI.cs
      (jugador manual)         (enemigo automático)
```

### Audio

```text
┌────────────────────────────────────────────────────────────┐
│                      AudioManager.cs                       │
│         Control central de música y efectos de sonido      │
└───────────────┬───────────────────────┬────────────────────┘
                │                       │
                ▼                       ▼
         MenuAudio.cs             GameplayAudio.cs
      (música de menú)          (música de partida)
```

### Escenas finales

```text
GameManager.cs
    ├─► YouWIN
    └─► Game Over
```

---

## 4. Capas del proyecto

Para entender mejor cómo está organizado el código, los scripts se pueden agrupar según su función dentro del juego.

### 4.1 Navegación

Incluye los scripts que se encargan de los botones y del cambio entre escenas.

**Clases:**

- `Jugar`
- `JugarFacil`
- `JugarIntermedio`
- `JugarDificil`
- `Tryagain`
- `SalirDelJuego`

**Responsabilidad:**

- Mover al jugador entre menús y partida
- Cargar escenas
- Cerrar el juego
- Reaccionar a botones de la interfaz

### 4.2 Estado del juego

Incluye los scripts que controlan las reglas generales de la partida.

**Clases:**

- `GameManager`
- `GameDifficulty`
- `GameDifficultySettings`

**Responsabilidad:**

- Llevar el marcador
- Decidir cuándo termina la partida
- Detectar victoria o derrota
- Guardar la dificultad seleccionada

### 4.3 Jugabilidad

Incluye los scripts que hacen que la partida funcione dentro del campo.

**Clases:**

- `Ball`
- `Paddle`
- `PaddleAI`

**Responsabilidad:**

- Mover la pelota
- Detectar rebotes y goles
- Controlar la pala del jugador
- Controlar la pala enemiga mediante lógica programada

### 4.4 Audio

Incluye los scripts relacionados con música y efectos de sonido.

**Clases:**

- `AudioManager`
- `MenuAudio`
- `GameplayAudio`

**Responsabilidad:**

- Reproducir música de menú
- Reproducir música de partida
- Reproducir efectos de botones, golpes, goles, victoria y derrota

---

## 5. Detalle de cada clase

### 5.1 `GameManager`
**Ruta:** `Assets/GameManager.cs`

Es el controlador principal de la partida.

**Responsabilidades:**

- Guardar el marcador del jugador y de la IA
- Actualizar el texto de puntos en pantalla
- Reiniciar posiciones tras cada gol
- Detectar victoria o derrota
- Cargar la escena final correspondiente

**Métodos principales:**

- `Start()`  
  Inicializa la partida
- `Paddle1Scored()`  
  Suma punto al jugador
- `Paddle2Scored()`  
  Suma punto a la IA
- `Restart()`  
  Recoloca la pelota y las palas
- `ResetMatch()`  
  Reinicia marcador y posiciones
- `CheckForWinner()`  
  Comprueba si alguien llegó al puntaje objetivo
- `LoadEndScene()`  
  Prepara el cambio a la escena final con retraso

---

### 5.2 `Ball`
**Ruta:** `Assets/ball.cs`

Representa la pelota y toda su lógica de movimiento.

**Responsabilidades:**

- Lanzar la pelota al empezar
- Moverla
- Acelerar ligeramente al golpear una pala
- Detectar entrada en portería
- Avisar al `GameManager` cuando se marca un gol
- Reiniciar el saque tras una espera

**Métodos principales:**

- `Start()`
- `Launch()`
- `OnCollisionEnter2D()`
- `OnTriggerEnter2D()`
- `LaunchAfterDelay()`

---

### 5.3 `Paddle`
**Ruta:** `Assets/Paddle.cs`

Control de una pala manejada por el jugador.

**Responsabilidades:**

- Leer input del teclado
- Mover la pala hacia arriba o abajo
- Limitar el movimiento para que no salga del campo

**Método principal:**

- `Update()`

---

### 5.4 `PaddleAI`
**Ruta:** `Assets/PaddleAI.cs`

Controla la pala enemiga.

**Responsabilidades:**

- Leer la dificultad seleccionada
- Seguir la trayectoria de la pelota
- Predecir a qué altura llegará la pelota
- Moverse hacia esa altura

**Métodos principales:**

- `Awake()`  
  Prepara la IA al empezar la escena
- `ApplySelectedDifficulty()`  
  Configura si la IA será fácil, media o difícil
- `Update()`  
  Mueve la pala en cada frame
- `GetTargetY()`  
  Calcula dónde debería colocarse la pala
- `ReflectWithinBounds()`  
  Corrige la predicción teniendo en cuenta los rebotes

---

### 5.5 `GameDifficulty`
**Ruta:** `Assets/GameDifficulty.cs`

Define las dificultades del juego.

**Incluye:**

- `GameDifficulty`
  - `Easy`
  - `Medium`
  - `Hard`

- `GameDifficultySettings`  
  Guarda la dificultad elegida entre escenas

---

### 5.6 `Jugar`
**Ruta:** `Assets/Jugar.cs`

Script del botón principal de jugar que carga el segundo menú de dificultades.

**Responsabilidad:**

- Reproducir sonido de botón
- Cargar la escena `menu2`

**Método:**

- `LoadScene()`

---

### 5.7 `JugarFacil`
**Ruta:** `Assets/JugarFacil.cs`

**Responsabilidad:**

- Seleccionar dificultad fácil
- Reproducir sonido de botón
- Cargar la escena `Game`

**Método:**

- `LoadScene()`

---

### 5.8 `JugarIntermedio`
**Ruta:** `Assets/JugarIntermedio.cs`

**Responsabilidad:**

- Seleccionar dificultad intermedia
- Reproducir sonido de botón
- Cargar la escena `Game`

**Método:**

- `LoadScene()`

---

### 5.9 `JugarDificil`
**Ruta:** `Assets/JugarDificil.cs`

**Responsabilidad:**

- Seleccionar dificultad difícil
- Reproducir sonido de botón
- Cargar la escena `Game`

**Método:**

- `LoadScene()`

---

### 5.10 `Tryagain`
**Ruta:** `Assets/Tryagain.cs`

**Responsabilidad:**

- Volver desde una escena final al menú principal

**Método:**

- `LoadScene()`

---

### 5.11 `SalirDelJuego`
**Ruta:** `Assets/SalirDelJuego.cs`

**Responsabilidad:**

- Cerrar el juego desde un botón

**Método:**

- `QuitGame()`

**Comportamiento:**

- En el editor, detiene el modo Play
- En el ejecutable, cierra la aplicación

---

### 5.12 `AudioManager`
**Ruta:** `Assets/AudioManager.cs`

Sistema central de sonido.

**Responsabilidades:**

- Mantener un único gestor de audio
- Reproducir música de menú
- Reproducir música de juego
- Reproducir efectos de:
  - botón
  - golpe de pala
  - gol
  - victoria
  - derrota
- Mantener el audio entre escenas

**Métodos principales:**

- `Awake()`
- `EnsureAudioSources()`
- `PlayButtonClick()`
- `PlayPaddleHit()`
- `PlayGoal()`
- `PlayWin()`
- `PlayLose()`
- `PlayMenuMusic()`
- `PlayGameplayMusic()`

---

### 5.13 `MenuAudio`
**Ruta:** `Assets/MenuAudio.cs`

**Responsabilidad:**

- Activar la música de menú al cargar una escena de menú

**Método:**

- `Start()`

---

### 5.14 `GameplayAudio`
**Ruta:** `Assets/GameplayAudio.cs`

**Responsabilidad:**

- Activar la música de juego al entrar en la escena `Game`

**Método:**

- `Start()`

---

## 6. Flujo del programa

```text
Menu
 │
 └─► Jugar.LoadScene()
        │
        └─► menu2
               │
               ├─► JugarFacil.LoadScene()
               ├─► JugarIntermedio.LoadScene()
               └─► JugarDificil.LoadScene()
                        │
                        ├─► guarda dificultad en GameDifficultySettings
                        └─► carga Game
                                 │
                                 ├─► GameManager.ResetMatch()
                                 ├─► Ball.Launch()
                                 ├─► Paddle controla al jugador
                                 ├─► PaddleAI controla a la IA
                                 └─► Ball detecta goles
                                          │
                                          ├─► GameManager.Paddle1Scored()
                                          ├─► GameManager.Paddle2Scored()
                                          └─► GameManager.CheckForWinner()
                                                    │
                                                    ├─► YouWIN
                                                    └─► Game Over
```

---

## 7. Estructura actual del proyecto

```text
pong/
│
├── Assets/
│   ├── AudioManager.cs
│   ├── ball.cs
│   ├── GameDifficulty.cs
│   ├── GameManager.cs
│   ├── GameplayAudio.cs
│   ├── Jugar.cs
│   ├── JugarDificil.cs
│   ├── JugarFacil.cs
│   ├── JugarIntermedio.cs
│   ├── MenuAudio.cs
│   ├── Paddle.cs
│   ├── PaddleAI.cs
│   ├── points.cs
│   ├── SalirDelJuego.cs
│   ├── Tryagain.cs
│   └── Scenes/
│       ├── Menu.unity
│       ├── menu2.unity
│       ├── Game.unity
│       ├── YouWIN.unity
│       └── Game Over.unity
│
├── Assembly-CSharp.csproj
├── pong.slnx
└── Packages / ProjectSettings / Library ...
```

---

## 8. Estado actual del proyecto

Actualmente el proyecto ya permite:

- Navegar por menús
- Elegir dificultad
- Jugar una partida completa
- Ganar o perder con cambio de escena
- Reproducir música y efectos
- Cerrar el juego
