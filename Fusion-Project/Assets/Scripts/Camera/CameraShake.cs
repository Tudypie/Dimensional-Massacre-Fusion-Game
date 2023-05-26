using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeMagnitude = 0.1f;

    [SerializeField] private float shakeTimer;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            ShakeCamera();
            shakeTimer -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = initialPosition;
        }
    }

    public void Shake(float shakeDuration=0.5f)
    {
        shakeTimer = shakeDuration;
    }

    private void ShakeCamera()
    {
        float offsetX = Mathf.PerlinNoise(Time.time * 50f, 0f) * 2f - 1f;
        float offsetY = Mathf.PerlinNoise(0f, Time.time * 50f) * 2f - 1f;

        Vector3 shakeOffset = new Vector3(offsetX, offsetY, 0f) * shakeMagnitude;
        transform.localPosition = initialPosition + shakeOffset;
    }
}
