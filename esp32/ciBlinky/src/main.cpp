/*
 * Blink
 * Turns on an LED on for one second,
 * then off for one second, repeatedly.
 */

#include <Arduino.h>
#include <WiFi.h>
#include "jenkinsJob.h"

const char *ssid = "Ankur";
const char *password = "youCANhack1";

jenkinsJob jenkins("http://192.168.1.104:8080", "DotnetXUnit", "master");

const uint success_pin = 18;
const uint fail_pin = 19;
const uint building_pin = 5;

void setup()
{
  pinMode(success_pin, OUTPUT);
  pinMode(fail_pin, OUTPUT);
  pinMode(building_pin, OUTPUT);

  Serial.begin(115200);
  delay(10);

  Serial.println();
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(ssid);

  WiFi.begin(ssid, password);

  while (WiFi.status() != WL_CONNECTED)
  {
    delay(500);
    Serial.print(".");
  }

  Serial.println("");
  Serial.println("WiFi connected");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());
}

void loop()
{
  delay(1000);
  String result = jenkins.getResult();

  Serial.println(result);

  if (result.equals(jenkins.SUCCESS))
  {
    digitalWrite(success_pin, LOW);
    digitalWrite(fail_pin, HIGH);
    digitalWrite(building_pin, HIGH);
  }
  else if (result.equals(jenkins.FAILURE))
  {
    digitalWrite(success_pin, HIGH);
    digitalWrite(fail_pin, LOW);
    digitalWrite(building_pin, HIGH);
  }
  else if (result.equals(jenkins.BUILDING))
  {
    digitalWrite(success_pin, HIGH);
    digitalWrite(fail_pin, HIGH);
    digitalWrite(building_pin, LOW);
  }
  else
  {
    digitalWrite(success_pin, LOW);
    digitalWrite(fail_pin, LOW);
    digitalWrite(building_pin, LOW);
  }
}
