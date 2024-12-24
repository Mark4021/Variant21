using UnityEngine;

public class AcceleratingBouncingObject : MonoBehaviour
{
    public float initialSpeed = 5f;             // Начальная скорость
    public float accelerationFactor = 1.2f;    // Ускорение при столкновении
    public float minimumSpeed = 2f;            // Минимальная скорость, чтобы избежать "прилипания"

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Случайное начальное направление только в плоскости XZ
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        rb.linearVelocity = randomDirection * initialSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Увеличиваем текущую скорость
        float currentSpeed = rb.linearVelocity.magnitude;
        float newSpeed = Mathf.Max(currentSpeed * accelerationFactor, minimumSpeed); // Гарантия минимальной скорости

        // Направление отражения
        Vector3 reflectedVelocity = Vector3.Reflect(rb.linearVelocity.normalized, collision.contacts[0].normal);

        // Убираем вертикальную составляющую (движение в плоскости XZ)
        reflectedVelocity.y = 0;
        reflectedVelocity = reflectedVelocity.normalized; // Нормализуем направление

        // Применяем новую скорость
        rb.linearVelocity = reflectedVelocity * newSpeed;
    }

    void FixedUpdate()
    {
        // Проверяем и восстанавливаем минимальную скорость, если объект сильно замедлился
        if (rb.linearVelocity.magnitude < minimumSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * minimumSpeed;
        }
    }
}
