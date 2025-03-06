using System.Collections;
using UnityEngine;

namespace Project.Templates.Scripts.Camera.Effect
{
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
        /// ������
        /// </summary>
        /// <param name="duration">������������ ������</param>
        /// <param name="magnitude">���������� �� ��������� �����</param>
        /// <param name="noise">���� ������</param>
        public void Shake(float duration, float magnitude, float noise, bool _shake3D) => _ = _shake3D ? StartCoroutine(IShake3D(duration, magnitude, noise)) : StartCoroutine(IShake2D(duration, magnitude, noise));

        /// <summary>
        /// ������ �� �����
        /// </summary>
        /// <param name="duration">������������ ������</param>
        /// <param name="magnitude">���������� �� ��������� �����</param>
        /// <param name="noise">���� ������</param>
        /// <param name="point">�������� ������</param>
        /// <param name="maxForce">����������� ��������� ������</param>
        public void Shake(float duration, float magnitude, float noise, Vector2 point, bool shake3D, float maxForce = 1)
        {
            float distance = Vector2.Distance(transform.position, point);
            float force = Mathf.Min(100 / distance, maxForce);
            _ = _shake3D
                ? StartCoroutine(IShake3D(duration, force * magnitude, force * noise))
                : StartCoroutine(IShake2D(duration, force * magnitude, force * noise));
        }
        /// <summary>
        /// ������� ������
        /// </summary>
        /// <param name="duration">������������ ������</param>
        /// <param name="magnitude">���������� �� ��������� �����</param>
        /// <param name="noise">���� ������</param>
        private IEnumerator IShake2D(float duration, float magnitude, float noise)
        {
            //�������������� ��������� ���������� �������
            float elapsed = 0f;
            //���������� ��� ����� �� "��������" ���� �������
            Vector2 noiseStartPointX = Random.insideUnitCircle * noise;
            Vector2 noiseStartPointY = Random.insideUnitCircle * noise;
            Vector3 oldDelta = Vector3.zero;
            //��������� ��� �� ��� ��� ���� �� �������� �����
            while (elapsed < duration)
            {
                //���������� ��� ��������� ���������� �� �������� ������� � ����������� �� ���������� �������
                Vector2 currentNoisePointX = Vector2.Lerp(noiseStartPointX, Vector2.zero, elapsed / duration);
                Vector2 currentNoisePointY = Vector2.Lerp(noiseStartPointY, Vector2.zero, elapsed / duration);
                //������ ����� ������ � �������� � �� ����� ���� ������ �������� �������
                Vector2 positionDelta = new(
                    RandomExtensions.Positive() * Mathf.PerlinNoise(currentNoisePointX.x, currentNoisePointX.y),
                    RandomExtensions.Positive() * Mathf.PerlinNoise(currentNoisePointY.x, currentNoisePointY.y));
                positionDelta *= magnitude * (duration - elapsed) / duration;

                //���������� ������ � ����� ����������
                transform.localPosition += (Vector3)positionDelta - oldDelta;
                oldDelta = positionDelta;

                //����������� ������� ���������� �������
                elapsed += Time.deltaTime;
                //���������������� ���������� ��������, � ��������� ����� ��� ��������� ���������� � ������ �����
                yield return null;
            }
        }
        /// <summary>
        /// ������� ������
        /// </summary>
        /// <param name="duration">������������ ������</param>
        /// <param name="magnitude">���������� �� ��������� �����</param>
        /// <param name="noise">���� ������</param>
        private IEnumerator IShake3D(float duration, float magnitude, float noise)
        {
            //�������������� ��������� ���������� �������
            float elapsed = 0f;
            //���������� ��� ����� �� "��������" ���� �������
            Vector2 noiseStartPointX = Random.insideUnitCircle * noise;
            Vector2 noiseStartPointY = Random.insideUnitCircle * noise;
            Vector2 noiseStartPointZ = Random.insideUnitCircle * noise;
            Vector3 oldDelta = Vector3.zero;
            //��������� ��� �� ��� ��� ���� �� �������� �����
            while (elapsed < duration)
            {
                //���������� ��� ��������� ���������� �� �������� ������� � ����������� �� ���������� �������
                Vector2 currentNoisePointX = Vector2.Lerp(noiseStartPointX, Vector2.zero, elapsed / duration);
                Vector2 currentNoisePointY = Vector2.Lerp(noiseStartPointY, Vector2.zero, elapsed / duration);
                Vector2 currentNoisePointZ = Vector2.Lerp(noiseStartPointZ, Vector2.zero, elapsed / duration);
                //������ ����� ������ � �������� � �� ����� ���� ������ �������� �������
                Vector3 positionDelta = new(
                    RandomExtensions.Positive() * Mathf.PerlinNoise(currentNoisePointX.x, currentNoisePointX.y),
                    RandomExtensions.Positive() * Mathf.PerlinNoise(currentNoisePointY.x, currentNoisePointY.y),
                    RandomExtensions.Positive() * Mathf.PerlinNoise(currentNoisePointZ.x, currentNoisePointZ.y));
                positionDelta *= magnitude * (duration - elapsed) / duration;

                //���������� ������ � ����� ����������
                transform.localPosition += positionDelta - oldDelta;
                oldDelta = positionDelta;

                //����������� ������� ���������� �������
                elapsed += Time.deltaTime;
                //���������������� ���������� ��������, � ��������� ����� ��� ��������� ���������� � ������ �����
                yield return null;
            }
        }
    }
}
