using UnityEngine;
using System;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(DuckAudioHandler))]
public class DuckBehaviour : MonoBehaviour
{
    [HideInInspector] public DuckBehaviour originalPrefab;
    public enum DuckState { Flying, Shocked, Falling }  // Creo un enum para los tres estados que va a tener el Patito
    public enum DirectionType { Horizontal, Diagonal, Vertical }    // Otro enum para las animaciones

    private const float MIN_ANGLE = 30f;
    private const float MAX_ANGLE = 150f;

    private const float MIN_VERTICAL_ANGLE = 80f;
    private const float MAX_VERTICAL_ANGLE = 100f;

    private const float MIN_DIAGONAL_ANGLE = 50f;
    private const float MAX_DIAGONAL_ANGLE = 130f;

    private DuckState currentState;
    private DirectionType currentAnimDirection;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private DuckAudioHandler duckAudioHandler;

    private Vector2 direction;
    private float nextDirectionChangeTime;

    // EVENTOS / SEÑALES
    public static event Action<int, Vector2> OnDuckKilled; // Pasa el valor del scoreValue y su posición.
    public static event Action<DuckBehaviour> OnDuckFinished; // Se dispara cuando el patito cae al piso. Se pasa a si mismo como referencia.

    [Header("Duck Stats")]  // Forma copada y prolija para visualizar los parámetros modificables en el inspector y mantenerlos privados
    [SerializeField] private float speed;
    [SerializeField] private int scoreValue;
    [SerializeField] private float directionChangeInterval = 2f;

    [Header("Boundaries")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    // Awake se llama al instanciar el objeto.
    void Awake()
    {
        // Awake se llama al instanciar el objeto. 
        // Ideal para cachear componentes de forma segura.
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        duckAudioHandler = GetComponent<DuckAudioHandler>();
    }

    // 
    public void Spawn(Vector2 startPosition)
    {
        // Reseteo sus valores
        rb.position = startPosition;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;

        // Inicio con el estado de volar
        ChangeState(DuckState.Flying);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (currentState)
        {
            case DuckState.Flying:
                rb.linearVelocity = direction * speed;
                CheckBoundaries();
                HandleRandomDirectionChange();
                //rb.AddForce(direction * speed); No utilizo AddForce ya que en el juego clásico el pato tiene una velocidad constante.
                break;
            case DuckState.Shocked:
                break;
            case DuckState.Falling:
                CheckGround();
                break;
        }
    }


    // Por alguna razón no hacía el flip seteandolo desde Start o FixedUpdate()
    // Creo que es porque el animator sobreescribe el flipX del sprite component
    // LateUpdate() me lo solucionó porque se ejecuta después del Animator.
    void LateUpdate()
    {
        if (currentState == DuckState.Flying)
        {
            sprite.flipX = (direction.x < 0);
        }
    }

    // Controlador de Estados
    // Acá realizo los cambios de estado y las acciones que solo necesitan ejecutarse una única vez y no una vez por frame (como en FixedUpdate())
    private void ChangeState(DuckState newState)
    {
        currentState = newState;
        print("Cambio de estado a: " + newState);

        // Acciones específicas que ocurren al ENTRAR a un nuevo estado
        switch (currentState)
        {
            case DuckState.Flying:
                // Setup de vuelo (ya está manejado en Start por ahora)
                EnterFlyingState();
                break;
            case DuckState.Shocked:
                // Frenamos el pato
                EnterShockedState();

                break;
            case DuckState.Falling:
                // le damos gravedad para que caiga
                EnterFallingState();
                break;
        }
    }

    private void EnterFallingState()
    {
        // Le doy gravedad para que caiga.
        rb.gravityScale = 1f;

        // Le doy play al audio de caída
        duckAudioHandler.StartFallingAudio();

    }

    private void EnterFlyingState()
    {
        // Seteo una dirección mediante una función personalizada
        SetNewRandomDirection(MIN_ANGLE, MAX_ANGLE);

        // Seteo el timer para el proximo cambio de dirección
        SetNextDirectionChangeTime();

        // Coloco el sonido de flapping
        duckAudioHandler.StartFlyingAudio();
    }

    private void EnterShockedState()
    {
        // Freno al pato
        rb.linearVelocity = Vector2.zero;
        // Triggereo el shot del animator
        anim.SetTrigger("shot");
        // Invoco la funcion que cambia al estado Falling en 0.5 segundos
        Invoke(nameof(ShockedAnimationEnded), 0.5f);

        // Freno el audio
        duckAudioHandler.StopAudio();
    }
    public void TakeHit()
    {
        if (currentState == DuckState.Flying)
        {
            ChangeState(DuckState.Shocked);

            // (el '?' verifica si hay alguien escuchando)
            OnDuckKilled?.Invoke(scoreValue, rb.position);
        }
    }

    public void CheckGround()
    {
        if (rb.position.y <= minX)
        {
            HitTheGround();
        }
    }
    public void HitTheGround()
    {
        // En lugar de destruir al pato, implemente un Pool de Patos
        // Le aviso al pool que se termino de caer
        OnDuckFinished?.Invoke(this);
    }

    public void ShockedAnimationEnded()
    {
        if (currentState == DuckState.Shocked)
        {
            ChangeState(DuckState.Falling);
        }
    }

    // Función que setea una nueva dirección aleatoria
    private void SetNewRandomDirection(float minDegree, float maxDegree)
    {
        // Creo de manera random un ángulo entre los angulos que pasemos como parametro
        float randomAngleDegrees = Random.Range(minDegree, maxDegree);

        // Seteo la animación correspondiente al angulo obtenido
        SetCurrentAnimation(randomAngleDegrees);

        // Convierto ese valor de grados a radianes
        float randomAngleRadians = randomAngleDegrees * Mathf.Deg2Rad;

        // Obtengo el valor X e Y del angulo (estos metodos trabajan con radianes, por eso la conversión)
        float x = Mathf.Cos(randomAngleRadians);
        float y = Mathf.Sin(randomAngleRadians);

        // Asigno la dirección calculada
        direction = new Vector2(x, y);
    }

    // Seteo la animación de acuerdo al ángulo o dirección
    private void SetCurrentAnimation(float angle)
    {
        if (angle > MIN_VERTICAL_ANGLE && angle < MAX_VERTICAL_ANGLE)
        {
            currentAnimDirection = DirectionType.Vertical;
        }
        else if ((angle >= MIN_DIAGONAL_ANGLE && angle <= MIN_VERTICAL_ANGLE) ||
            (angle >= MAX_VERTICAL_ANGLE && angle <= MAX_DIAGONAL_ANGLE))
        {
            currentAnimDirection = DirectionType.Diagonal;
        }
        else
        {
            currentAnimDirection = DirectionType.Horizontal;
        }

        anim.SetFloat("directionType", (float)currentAnimDirection);
    }


    // Chequea que el pato no se vaya de la pantalla e invierte su dirección
    private void CheckBoundaries()
    {
        bool didBounce = false;

        // Chequeo Horizontal
        if (rb.position.x >= maxX && direction.x > 0|| rb.position.x <= minX && direction.x < 0)
        {
            direction.x *= -1;

            // Fuerzo / clampeo la posición en X por si debido a la velocidad del pato se pasa del limite.
            float clampedX = Mathf.Clamp(rb.position.x, minX, maxX);
            rb.position = new Vector2(clampedX, rb.position.y);

            didBounce = true;
        }

        // Chequeo Vertical
        if (rb.position.y >= maxY && direction.y > 0 || rb.position.y <= minY && direction.y < 0)
        {
            direction.y *= -1;

            // Clampeo en Y
            float clampedY = Mathf.Clamp(rb.position.y, minY, maxY);
            rb.position = new Vector2(rb.position.x, clampedY);

            didBounce = true;
        }

        // Si hubo rebote cambio la animación
        if (didBounce)
        {
            // Atan2 convierte un vector en un ángulo en radianes.
            // Lo multiplico por Rad2Deg para pasarlo a grados.
            float currentAngleDegrees = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Como Atan2 devuelve valores de -180 a 180. Lo paso a 0-360° sumandole 360°
            if (currentAngleDegrees < 0)
            {
                currentAngleDegrees += 360f;
            }

            SetCurrentAnimation(currentAngleDegrees);
        }
    }

    private void HandleRandomDirectionChange()
    {
        if (Time.time >= nextDirectionChangeTime)
        {
            SetNewRandomDirection(0, 360);
            SetNextDirectionChangeTime();
        }
    }

    private void SetNextDirectionChangeTime()
    {
        nextDirectionChangeTime = Time.time + directionChangeInterval;
    }
}