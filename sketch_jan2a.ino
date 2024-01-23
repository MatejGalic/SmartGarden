#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>
#include <ArduinoJson.h>
#include <ArduinoMqttClient.h>
#include <DHT11.h>

//Pin definitions for sennsors and relays
const int SoilSensorA0Pin = 12;
const int SoilSensorD0Pin = 7;
const int TemperatureAndHumidityD0Pin = 2;
const int PumpD0Pin = 3;
const int WindowD0Pin = 4;

//Wifi credentials
const char* ssid = "zfguilg6786";
const char* password = "leonsopar123";
const String userName = "testUser";

//Relay manual control flags
bool manualPumpControl = false;
bool manualPumpTurnOn = false;
bool manualWindowControl = false;

//Relay and moisture flags
bool openPumpFlag = false;
bool pumpCooldownFlag = true;
bool lowSoilMoistureFlag = false;
bool openWindowFlag = false;

//Reading thresholds
float humidityThreshold = 50;
float soilMoistureThreshold = 50;
float temperatureThreshold = 15;

//Time intervals
const int pumpOpenDuration = 10000;
const int pumpCooldownDuration = 20000;
const int windowDuration = 10000;
const int checkMoisture = 20000;
const int sendReadingsInterval = 20000;
const int readSensorsInterval = 20000;

//Time variables
unsigned long currentMilis = 0;
unsigned long previousPumpOpen = 0;
unsigned long previousWindowOpen = 0;
unsigned long previousCheckMoisture = 0;
unsigned long previousReadingsSent = 0;
unsigned long previousSensorsRead = 0;

//Sensor readings
int temperature = 0;
int humidity = 0;
int moisture = 0;

//Wifi client needed for MQTT
WiFiClient wifiClient;

//Mqtt client
MqttClient mqttClient(wifiClient);

//Mqtt variables
const char* broker = "192.168.137.1";
const int port = 1883;
const char* publishTopic = "reading";
const char* subscribeTopic = "server";

//Temperature and humidity sensor 
DHT11 dht11(TemperatureAndHumidityD0Pin);

void setup() {
  Serial.begin(115200);
  delay(1000);

  WiFi.begin(ssid, password);

  Serial.print("Connecting...");

  //Connect to Wifi
  while(WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }

  Serial.println();
  Serial.print("Connected! IP-Address: ");
  Serial.println(WiFi.localIP());


  Serial.print("Connecting to MQTT broker...");
  Serial.println(broker);

  //Connect to MQTT broker
  if(!mqttClient.connect(broker, port)) {
    Serial.print("MQTT connection failed! Error code = ");
    Serial.println(mqttClient.connectError());
  }

  //Message received callback function
  mqttClient.onMessage(onMqttMessage);

  //Subscribe to topic
  mqttClient.subscribe(subscribeTopic);

  pinMode(PumpD0Pin, OUTPUT);
  pinMode(WindowD0Pin, OUTPUT);

}

void loop() {
  
  currentMilis = millis();

  mqttClient.poll();
  readTemperatureAndHumiditySensor();
  readSoilMoistureSensor();
  pumpControl();
  sendReadingsMqtt();

}

//Read temperature and humidity
void readTemperatureAndHumiditySensor() {
  if(currentMilis - previousSensorsRead >= readSensorsInterval) {
    //temperature = dht11.readTemperature();
    //humidity = dht11.readHumidity();
    temperature = random(100);
    humidity = random(100);

    if (temperature != DHT11::ERROR_CHECKSUM && temperature != DHT11::ERROR_TIMEOUT &&
          humidity != DHT11::ERROR_CHECKSUM && humidity != DHT11::ERROR_TIMEOUT)
      {
          Serial.print("Temperature: ");
          Serial.print(temperature);
          Serial.println(" °C");

          Serial.print("Humidity: ");
          Serial.print(humidity);
          Serial.println(" %");
      }
      else
      {
          if (temperature == DHT11::ERROR_TIMEOUT || temperature == DHT11::ERROR_CHECKSUM)
          {
              Serial.print("Temperature Reading Error: ");
              Serial.println(DHT11::getErrorString(temperature));
          }
          if (humidity == DHT11::ERROR_TIMEOUT || humidity == DHT11::ERROR_CHECKSUM)
          {
              Serial.print("Humidity Reading Error: ");
              Serial.println(DHT11::getErrorString(humidity));
          }
      }
      previousSensorsRead += readSensorsInterval;
  }
}

//Read soil moisture
void readSoilMoistureSensor() {
  if(currentMilis - previousCheckMoisture >= checkMoisture) {
    if(!openPumpFlag){
      moisture = random(75) + 25;

      Serial.print("Moisture: ");
      Serial.print(moisture);
      Serial.println(" %");

      soilMoistureThresholdCheck();
    }

    previousCheckMoisture += checkMoisture;
  }
}

//Publish readings to mqtt broker
void sendReadingsMqtt() {
  if(currentMilis - previousReadingsSent >= sendReadingsInterval){
    
    JsonDocument doc;
    doc["humidity"] = humidity;
    doc["temperature"] = temperature;
    doc["moisture"] = moisture;
    doc["pump"] = openPumpFlag ? "open" : "closed";
    doc["window"] = openWindowFlag ? "open" : "closed";
    doc["user"] = userName;
    String json;
    serializeJson(doc, json);

    mqttClient.beginMessage(publishTopic);
    mqttClient.print(json);
    mqttClient.endMessage();

    previousReadingsSent += sendReadingsInterval;
  }
}

//Open pump and set appropriate flag and timing
void openPump() {
  previousPumpOpen = currentMilis;
  digitalWrite(PumpD0Pin, HIGH);
  openPumpFlag = true;
  Serial.println("Opening pump...");
}

//Close pump and set appropriate flags
void closePump() {
  digitalWrite(PumpD0Pin, LOW);
  openPumpFlag = false;
  pumpCooldownFlag = true;
  if(manualPumpControl) manualPumpTurnOn = false;
  Serial.println("Closing pump...");
}

void openWindow() {
  digitalWrite(WindowD0Pin, HIGH);
}

void closeWindow() {
  digitalWrite(WindowD0Pin, LOW);
}

//Check if soil moisture is under threshold and set appropraite flag
void soilMoistureThresholdCheck() {
  if(moisture < soilMoistureThreshold) lowSoilMoistureFlag = true;
  else lowSoilMoistureFlag = false;
}

void turnOnPump() {
  if(!openPumpFlag) openPump();
  if(((currentMilis - previousPumpOpen) >= pumpOpenDuration) && openPumpFlag) {
    Serial.print("Pump has been open for ");
    Serial.print(pumpOpenDuration / 1000.0);
    Serial.println(" seconds.");
    closePump();
    previousPumpOpen += pumpOpenDuration;
  }
}

//Pump control
void pumpControl() {

  //If pump is on cooldown
  if(pumpCooldownFlag) {
    if(currentMilis - previousPumpOpen >= pumpCooldownDuration) {
      Serial.print("Pump cooldown of ");
      Serial.print(pumpCooldownDuration / 1000.0);
      Serial.println(" seconds over.");
      previousPumpOpen += pumpCooldownDuration;
      pumpCooldownFlag = false;
    }
  //If pump isn't on cooldown, manual mode is off and soil moisture is under threshold -> turn on pump
  //Or if pump control is manual and pump should turn on
  } else if((lowSoilMoistureFlag && !manualPumpControl && !pumpCooldownFlag) || (manualPumpControl && manualPumpTurnOn)) {
    turnOnPump();
  }
}

//MQTT on message function
void onMqttMessage(int messageSize) {

  //Read message
  char message[messageSize + 1];
  int count = 0;
  while (mqttClient.available()) {
    message[count++] = (char)mqttClient.read();
  }
  message[count] = '\0';

  Serial.println(message);

  //Deserialize message into JSON
  JsonDocument doc;
  DeserializationError error = deserializeJson(doc, message);

  if (error) {
    Serial.print("deserializeJson() returned ");
    Serial.println(error.c_str());
  } else {
    JsonObject root = doc.as<JsonObject>();

    String targetUser = root["user"];
    if(targetUser && targetUser == userName) {
      if(root.containsKey("turnOnManual")) {
        manualPumpControl = root["turnOnManual"];
        manualPumpControl ? Serial.println("Pump now in manual mode!") : Serial.println("Pump now in automatic mode!");
      }
      if(root.containsKey("turnOnPump")) {
        manualPumpTurnOn = root["turnOnPump"];
      }
    }
  }
}
