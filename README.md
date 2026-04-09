# 🦆 Duck Hunt - Unity 6

> **Proyecto Académico - Materia: Motores Avanzados** > *Tecnicatura en Desarrollo de Videojuegos (UNLaM)*

## Descripción General

Estoy armando un clon del clásico *Duck Hunt* desde cero en **Unity 6**. La idea no es hacer un calco, sino usar las mecánicas base del juego clásico para meter mano e implementar un diseño propio. Más adelante, la meta es meterle mi propio arte, escenarios personalizados y ajustes de Game Design para darle identidad.

Es un proyecto para la facultad para iniciarse en Unity, pero aproveché la oportunidad para implementar patrones de diseño y buenas prácticas que fui aprendiendo por mi cuenta.

## Arquitectura y Sistemas

Trate de darle mucho cariño a la arquitectura aplicando principios **SOLID**. Acá dejo algunas de las cosas que implementé:

* **Object Pooling (Fábrica de Patos):** Aunque quizá para el tipo de juego es algo exagerado, armé un `DuckPool` dinámico usando diccionarios y queues. Los patos se reciclan y vuelven al pool cuando mueren.
* **Patrón Observer (Eventos):** Todo está desacoplado. Cuando un pato muere o toca el piso, emite una señal. El `GameManager` y el `DuckPool` lo escuchan y hacen su trabajo.
* **Máquina de Estados (FSM):** El comportamiento de los patos está segmentado en estados (`Flying`, `Shocked`, `Falling`). Cada estado maneja su propia física y animación.
* **Game Manager & Game Loop:** Maneja las rondas, la cantidad de patos por nivel y elige *spawn points* aleatorios.
* **Gestor de Audio Dedicado (`DuckAudioHandler`):** Separé el sonido de la lógica de movimiento. Los patos tienen canales de audio independientes (para el aleteo y los "cuacks").
* **Data-Driven Rounds:** Utilice `ScriptableObjects` para definir cada ronda. Esto permite cambiar la secuencia de patos o la dificultad sin tocar el código.

## Próximos Pasos (Roadmap)

- [x] Spawneo dinámico de patos, Object Pooling.
- [x] Sistema de Rondas (con ScriptableObjects)
- [x] Implementar mecánica de "Escape".
- [x] Gestión de munición.
- [ ] Controlador y animaciones del Perro cazador.
- [ ] HUD general (Contador de balas y UI principal).
- [ ] Integración de assets propios y pulido general.

## Tecnologías
* **Motor:** Unity 6
* **Lenguaje:** C#

**Desarrollado por:** Juan Ignacio Battelli