# 🦆 Duck Hunt - Unity 6

> **Proyecto Académico - Materia: Motores Avanzados** > *Tecnicatura en Desarrollo de Videojuegos (UNLaM)*

## Descripción General

Este proyecto consiste en un clon del clásico *Duck Hunt*, desarrollado desde cero para profundizar en el uso y las herramientas del motor **Unity 6**. 

El objetivo principal no es lograr una réplica 1:1, sino utilizar la base mecánica del juego para iterar e implementar un diseño propio. Esto incluye la creación de personajes originales, escenarios personalizados y modificaciones en el Game Design que le aporten una identidad única al proyecto.

## Arquitectura y Características Técnicas

El código fue estructurado priorizando las buenas prácticas, la modularidad y la escalabilidad, aplicando principios de **Clean Code** y **SOLID**. 

**Sistemas implementados hasta la fecha:**

* **Programación Orientada a Objetos (Herencia):** Creación de un `Duck_Base` como clase padre, permitiendo derivar variantes (hijos) con sus propias estadísticas (velocidad, puntaje).
* **Máquina de Estados Finitos (FSM):** Lógica del comportamiento de los patos segmentada en estados (`Flying`, `Shocked`, `Falling`) mediante `enums`, garantizando transiciones limpias y control sobre las físicas y animaciones en cada fase.
* **Patrón Observer:** Desacoplamiento de sistemas mediante `System.Action`. Los patos emiten señales al chocar contra el suelo, permitiendo que otros sistemas (UI, Animación del Perro) reaccionen sin generar dependencias.
* **Mecánica de Disparo por Raycasting (2D):** Implementación de la detección de impactos utilizando `Physics2D.Raycast` y conversión de coordenadas.


## Próximos Pasos (Roadmap)

- [ ] Implementación del controlador y animaciones del Perro.
- [ ] Desarrollo del HUD y UI (Contador de balas, puntaje, rondas).
- [ ] Integración de assets artísticos propios y diseño de niveles.
- [ ] Efectos de partículas y pulido de Audio.

## Tecnologías Utilizadas
* **Motor:** Unity 6
* **Lenguaje:** C#
