using System.Collections;

using UnityEngine;

public class Shaker : MonoBehaviour
{
    [SerializeField]
    private bool _shake3D = false;
    [SerializeField]
    private Transform _point;
    [SerializeField]
    [Min(0)]
    private float _duration = 1, _magnitude = 1, _noise = 1, _maxForce = 1;
    #region Unity Methods
    private void OnEnable()
    {
        if (_point)
        {
            Shake(_duration, _magnitude, _noise, _point.position, _shake3D, _maxForce);
        }
        else
        {
            Shake(_duration, _magnitude, _noise, _shake3D);
        }
    }
    #endregion Unity Methods
    /// <summary>
    /// Тряска
    /// </summary>
    /// <param name="duration">длительность тряски</param>
    /// <param name="magnitude">расстояние от состояния покоя</param>
    /// <param name="noise">сила тряски</param>
    public void Shake(float duration, float magnitude, float noise, bool _shake3D) => _ = _shake3D ? StartCoroutine(IShake3D(duration, magnitude, noise)) : StartCoroutine(IShake2D(duration, magnitude, noise));

    /// <summary>
    /// Тряска от точки
    /// </summary>
    /// <param name="duration">длительность тряски</param>
    /// <param name="magnitude">расстояние от состояния покоя</param>
    /// <param name="noise">сила тряски</param>
    /// <param name="point">источник тряски</param>
    /// <param name="maxForce">максимально возможная тряска</param>
    public void Shake(float duration, float magnitude, float noise, Vector2 point, bool shake3D, float maxForce = 1)
    {
        float distance = Vector2.Distance(transform.position, point);
        float force = Mathf.Min(100 / distance, maxForce);
        _ = _shake3D
            ? StartCoroutine(IShake3D(duration, force * magnitude, force * noise))
            : StartCoroutine(IShake2D(duration, force * magnitude, force * noise));
    }
    /// <summary>
    /// Каротин тряски
    /// </summary>
    /// <param name="duration">длительность тряски</param>
    /// <param name="magnitude">расстояние от состояния покоя</param>
    /// <param name="noise">сила тряски</param>
    private IEnumerator IShake2D(float duration, float magnitude, float noise)
    {
        //Инициализируем счётчиков прошедшего времени
        float elapsed = 0f;
        //Генерируем две точки на "текстуре" шума Перлина
        Vector2 noiseStartPointX = Random.insideUnitCircle * noise;
        Vector2 noiseStartPointY = Random.insideUnitCircle * noise;
        Vector3 oldDelta = Vector3.zero;
        //Выполняем код до тех пор пока не иссякнет время
        while (elapsed < duration)
        {
            //Генерируем две очередные координаты на текстуре Перлина в зависимости от прошедшего времени
            Vector2 currentNoisePointX = Vector2.Lerp(noiseStartPointX, Vector2.zero, elapsed / duration);
            Vector2 currentNoisePointY = Vector2.Lerp(noiseStartPointY, Vector2.zero, elapsed / duration);
            //Создаём новую дельту и умножаем её на длину дабы учесть желаемый разброс
            Vector2 positionDelta = new(
                RandomExtensions.Positive() * Mathf.PerlinNoise(currentNoisePointX.x, currentNoisePointX.y),
                RandomExtensions.Positive() * Mathf.PerlinNoise(currentNoisePointY.x, currentNoisePointY.y));
            positionDelta *= magnitude * (duration - elapsed) / duration;

            //Перемещаем камеру в новую координату
            transform.localPosition += (Vector3)positionDelta - oldDelta;
            oldDelta = positionDelta;

            //Увеличиваем счётчик прошедшего времени
            elapsed += Time.deltaTime;
            //Приостанавливаем выполнение каротины, в следующем кадре она продолжит выполнение с данной точки
            yield return null;
        }
    }
    /// <summary>
    /// Каротин тряски
    /// </summary>
    /// <param name="duration">длительность тряски</param>
    /// <param name="magnitude">расстояние от состояния покоя</param>
    /// <param name="noise">сила тряски</param>
    private IEnumerator IShake3D(float duration, float magnitude, float noise)
    {
        //Инициализируем счётчиков прошедшего времени
        float elapsed = 0f;
        //Генерируем две точки на "текстуре" шума Перлина
        Vector2 noiseStartPointX = Random.insideUnitCircle * noise;
        Vector2 noiseStartPointY = Random.insideUnitCircle * noise;
        Vector2 noiseStartPointZ = Random.insideUnitCircle * noise;
        Vector3 oldDelta = Vector3.zero;
        //Выполняем код до тех пор пока не иссякнет время
        while (elapsed < duration)
        {
            //Генерируем две очередные координаты на текстуре Перлина в зависимости от прошедшего времени
            Vector2 currentNoisePointX = Vector2.Lerp(noiseStartPointX, Vector2.zero, elapsed / duration);
            Vector2 currentNoisePointY = Vector2.Lerp(noiseStartPointY, Vector2.zero, elapsed / duration);
            Vector2 currentNoisePointZ = Vector2.Lerp(noiseStartPointZ, Vector2.zero, elapsed / duration);
            //Создаём новую дельту и умножаем её на длину дабы учесть желаемый разброс
            Vector3 positionDelta = new(
                RandomExtensions.Positive() * Mathf.PerlinNoise(currentNoisePointX.x, currentNoisePointX.y),
                RandomExtensions.Positive() * Mathf.PerlinNoise(currentNoisePointY.x, currentNoisePointY.y),
                RandomExtensions.Positive() * Mathf.PerlinNoise(currentNoisePointZ.x, currentNoisePointZ.y));
            positionDelta *= magnitude * (duration - elapsed) / duration;

            //Перемещаем камеру в новую координату
            transform.localPosition += positionDelta - oldDelta;
            oldDelta = positionDelta;

            //Увеличиваем счётчик прошедшего времени
            elapsed += Time.deltaTime;
            //Приостанавливаем выполнение каротины, в следующем кадре она продолжит выполнение с данной точки
            yield return null;
        }
    }
}
