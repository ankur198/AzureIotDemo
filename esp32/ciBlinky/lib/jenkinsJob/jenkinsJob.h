#ifndef _JENKINS_JOB_
#define _JENKINS_JOB_

#include <ArduinoJson.h>
#include <HTTPClient.h>

class jenkinsJob
{
private:
    const size_t capacity = 3 * JSON_ARRAY_SIZE(0) + 5 * JSON_ARRAY_SIZE(1) + JSON_ARRAY_SIZE(19) + 13 * JSON_OBJECT_SIZE(0) + 5 * JSON_OBJECT_SIZE(1) + 8 * JSON_OBJECT_SIZE(2) + JSON_OBJECT_SIZE(4) + 2 * JSON_OBJECT_SIZE(5) + JSON_OBJECT_SIZE(21) + 1500;
    String url;

public:
    const char *SUCCESS = "SUCCESS";
    const char *FAILURE = "FAILURE";
    const char *BUILDING = "BUILDING";
    const char *UNKNOWN = "UNKNOWN";

    jenkinsJob(String host, String pipelineName, String branch);

    String getResult();
};

jenkinsJob::jenkinsJob(String host, String pipelineName, String branch)
{
    url = host + "/job/" + pipelineName + "/job/" + branch + "/lastBuild/api/json";
}

String jenkinsJob::getResult()
{
    String response = "null";

    HTTPClient http;
    http.begin(url);

    int httpCode = http.GET();

    if (httpCode > 0)
    {
        DynamicJsonDocument doc(capacity * 2);
        String payload = http.getString();
        DeserializationError err = deserializeJson(doc, payload);
        if (err)
        {
            response = "deserializeJson() failed with code " + String(err.c_str());
        }
        else
        {
            JsonObject obj = doc.as<JsonObject>();

            String result = obj["result"];
            bool building = obj["building"];

            if (result.equals("SUCCESS"))
            {
                response = SUCCESS;
            }
            else if (result.equals("FAILURE"))
            {
                response = FAILURE;
            }
            else if (building)
            {
                response = BUILDING;
            }
            else
            {
                response = UNKNOWN;
            }
        }
    }
    else
    {
        response = "Error on HTTP request";
    }

    http.end();

    return response;
}

#endif
