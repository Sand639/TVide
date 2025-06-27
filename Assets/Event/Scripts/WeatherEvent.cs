using UnityEngine;

public class WeatherEvent : MonoBehaviour
{
    public enum WEATHER_TYPE { Wind, Rain, Thunder }
    public WEATHER_TYPE weatherType = WEATHER_TYPE.Wind;

    public float windDuration = 3f;
    public float windSpeed = 8.0f;

    public ParticleSystem windEffectPrefab;

    public static event System.Action<float> OnWindEventStart; 

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            PlayerManager player = collision.gameObject.GetComponent<PlayerManager>();

            if (player != null)
            {
                switch (weatherType)
                {
                    case WEATHER_TYPE.Wind:
                        player.ApplyWindBoost(windDuration, windSpeed);

                        OnWindEventStart?.Invoke(windDuration); ////Eventが起こったことを通知//カメラの振動を有効にすｒ
                        if (windEffectPrefab != null)
                        {
                            Debug.Log("強風イベントの実行をする");
                            Instantiate(windEffectPrefab, player.transform.position, Quaternion.Euler(0, 90, 0));
                        }
                        break;

                    case WEATHER_TYPE.Rain:
                        Debug.Log("水中イベントの実行をする");
                        player.ApplyRainFloat(4.0f);
                        break;
                    default:
                        break;
                }
            }

            Destroy(gameObject);
        }
    }
}
