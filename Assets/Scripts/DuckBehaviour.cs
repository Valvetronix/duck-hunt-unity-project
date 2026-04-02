using UnityEngine;
using UnityEngine.U2D;

public class DuckBehaviour : MonoBehaviour
{
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

    private Vector2 direction;


    [Header("DUCK STATS")]  // Forma copada y prolija para visualizar los parámetros modificables en el inspector
    [SerializeField] private float speed;
    [SerializeField] private int scoreValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Inicializo los componentes
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        // Seteo el estado inicial (volar)
        ChangeState(DuckState.Flying);

        // Seteo una dirección mediante una función personalizada
        direction = GetRandomDirection();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (currentState)
        {
            case DuckState.Flying:
                rb.linearVelocity = direction * speed;
                break;
            case DuckState.Shocked:
                break;
            case DuckState.Falling:
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
    public void ChangeState(DuckState newState)
    {
        currentState = newState;
        print("Cambio de estado a: " + newState);

        // Acciones específicas que ocurren al ENTRAR a un nuevo estado
        switch (currentState)
        {
            case DuckState.Flying:
                // Setup de vuelo (ya está manejado en Start por ahora)
                break;
            case DuckState.Shocked:
                // Frenamos el pato
                rb.linearVelocity = Vector2.zero;
                // aca se activa el triggerr anim.SetTrigger("shot");
                break;
            case DuckState.Falling:
                // le damos gravedad para que caiga
                rb.gravityScale = 1f;
                break;
        }
    }

    // Función que devuelve un Vector2 (una dirección)
    Vector2 GetRandomDirection()
    {
        // Creo de manera random un ángulo entre 30° y 150°
        float randomAngleDegrees = Random.Range(MIN_ANGLE, MAX_ANGLE);

        // Seteo la animación correspondiente al angulo obtenido
        SetCurrentAnimation(randomAngleDegrees);

        // Convierto ese valor de grados a radianes
        float randomAngleRadians = randomAngleDegrees * Mathf.Deg2Rad;

        // Obtengo el valor X e Y del angulo (estos metodos trabajan con radianes, por eso la conversión)
        float x = Mathf.Cos(randomAngleRadians);
        float y = Mathf.Sin(randomAngleRadians);

        
        // Finalizo la función retornando la dirección randomm
        return new Vector2(x, y);
    }

    // Seteo la animación de acuerdo al ángulo/dirección
    void SetCurrentAnimation(float angle)
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
}