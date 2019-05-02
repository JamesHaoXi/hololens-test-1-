using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DebugClick : MonoBehaviour {
    public Text buttonText;
    public Text descriptionText;
    public Text testText;
    public Text testButtonText;
    public Renderer batteryRenderer;
    public Renderer lightRenderer;
    static private bool isPolling = false;
    static private bool isTesting = false;
    static private bool testLoading = false;

    public void TogglePoll()
    {
        if (isPolling)
        {
            isPolling = false;
            buttonText.text = "Get Data";
            CancelInvoke("Poll");
        }
        else
        {
            isPolling = true;
            buttonText.text = "Stop Data";
            InvokeRepeating("Poll", 0, 1);
        }
    }

    void Poll()
    {
        StartCoroutine(LoadDeviceDataAsync());
    }

    private string baseUrl = "https://james6342hololensfunctionapp1.azurewebsites.net/api/Function1";

    IEnumerator LoadDeviceDataAsync()
    {
        var req = UnityWebRequest.Get(baseUrl + "?name=TEST&mac=FF:FF:FF:FF:FF:FF");
        yield return req.SendWebRequest();
        if (req.isNetworkError || req.isHttpError)
        {
            Debug.Log("error!");
            descriptionText.text = req.error;
        }
        else
        {
            // Show results as text
            var data = JsonUtility.FromJson<DeviceData>(req.downloadHandler.text);
            var builder = new StringBuilder();

            var faultyBattery = data.vdd < 2000;
            var faultyLight = data.brightness < 185;
            var replaceLight = faultyLight && !faultyBattery;

            builder.Append("Device: ");
            builder.Append(data.deviceID);
            builder.Append("\n");

            builder.Append("Brightness: ");
            builder.Append(data.brightness);
            if (replaceLight) { builder.Append(" (faulty light)"); }
            builder.Append("\n");

            builder.Append("Voltage: ");
            builder.Append(data.vdd);
            if (faultyBattery) { builder.Append(" (faulty battery)"); }
            builder.Append("\n");

            builder.Append("Last Updated: ");
            builder.Append(data.timestamp);

            descriptionText.text = builder.ToString();

            testText.text = data.status ? "Testing: TRUE" : "Testing: FALSE";

            batteryRenderer.enabled = faultyBattery;
            lightRenderer.enabled = faultyLight;

            if (!testLoading)
            {
                isTesting = data.status;
                testButtonText.text = data.status ? "Stop Test" : "Start Test";
            }

            if (testLoading && data.status == isTesting)
            {
                testLoading = false;
            }
        }
    }

    public void ToggleTest()
    {
        StartCoroutine(ToggleTestAsync());
    }

    IEnumerator ToggleTestAsync()
    {
        if (testLoading)
        {
            yield break;
        }

        if (isTesting)
        {
            testButtonText.text = "Stopping Test...";
            isTesting = false;
            testLoading = true;
            var req = UnityWebRequest.Get(baseUrl + "?name=TEST%20OFF&mac=FF:FF:FF:FF:FF:FF");
            yield return req.SendWebRequest();
        }
        else
        {
            testButtonText.text = "Starting Test...";
            isTesting = true;
            testLoading = true;
            var req = UnityWebRequest.Get(baseUrl + "?name=TEST%20ON&mac=FF:FF:FF:FF:FF:FF");
            yield return req.SendWebRequest();
        }
    }

    public void JobDone()
    {
        StartCoroutine(JobDoneAsync());
    }

    IEnumerator JobDoneAsync()
    {
        var req = UnityWebRequest.Get(baseUrl + "?name=JOB%20DONE&mac=FF:FF:FF:FF:FF:FF");
        yield return req.SendWebRequest();
    }
}

class DeviceData
{
    public string deviceID;
    public string timestamp;
    public int brightness;
    public int vdd;
    public bool status;
}