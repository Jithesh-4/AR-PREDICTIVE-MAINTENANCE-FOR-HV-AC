using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class getData : MonoBehaviour
{
    public TextMeshProUGUI temperature;
    public TextMeshProUGUI current;
    public TextMeshProUGUI voltage;
    public TextMeshProUGUI airflow;
    public TextMeshProUGUI pressure;
    public TextMeshProUGUI vibration;
    public TextMeshProUGUI humidity;
    public TextMeshProUGUI airQuality;

    void Start()
    {
        // Assign TextMeshProUGUI components to variables
        temperature = GameObject.Find("TemperatureText").GetComponent<TextMeshProUGUI>();
        current = GameObject.Find("CurrentText").GetComponent<TextMeshProUGUI>();
        voltage = GameObject.Find("VoltageText").GetComponent<TextMeshProUGUI>();
        airflow = GameObject.Find("AirFlowText").GetComponent<TextMeshProUGUI>();
        pressure = GameObject.Find("PressureText").GetComponent<TextMeshProUGUI>();
        vibration = GameObject.Find("VibrationText").GetComponent<TextMeshProUGUI>();
        humidity = GameObject.Find("HumidityText").GetComponent<TextMeshProUGUI>();
        airQuality = GameObject.Find("AirQualityText").GetComponent<TextMeshProUGUI>();

        Debug.Log("Vuforia Started");
        InvokeRepeating("FetchData", 1,1f); // Fetch data every 3 seconds
    }

    void FetchData()
    {
        StartCoroutine(GetSensorValues());
    }

    IEnumerator GetSensorValues()
    {
        Debug.Log("Getting Data");

        string uri = "https://ar-predictive-maintenance-api.vercel.app/data";
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Network Error: " + request.error);
            }
            else
            {
                // Deserialize JSON response into Sens object
                Sens data = JsonUtility.FromJson<Sens>(request.downloadHandler.text);

                // Update UI with fetched data
                temperature.text = data.temperature.ToString() + "�C";
                current.text = data.current.ToString() + " A";
                voltage.text = data.voltage.ToString() + " V";
                airflow.text = data.airflow.ToString() + " m�/h";
                pressure.text = data.pressure.ToString() + " Pa";
                vibration.text = data.vibration.ToString() + " mm/s";
                humidity.text = data.humidity.ToString() + "%";
                airQuality.text = data.airQuality.ToString() + " AQI";
            }
        }
    }
}

[System.Serializable]
class Sens
{
    public int temperature;
    public int current;
    public int voltage;
    public int airflow;
    public int pressure;
    public int vibration;
    public int humidity;
    public int airQuality;
}
