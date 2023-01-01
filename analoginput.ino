
int sensorPin1 = A0;   // select the input pin for the potentiometer
int sensorPin2 = A1; 
// int ledPin = 13;      // select the pin for the LED
double sensorValue1 = 0;  // variable to store the value coming from the sensor
double sensorValue2 = 0;
void setup() {
  // declare the ledPin as an OUTPUT:
    Serial.begin(9600); 
}

int LeftArrow = 37;
int UpArrow = 38;
int RightArrow = 39;
int DownArrow = 40;

void loop() {
  // read the value from the sensor:
  Serial.println(LeftArrow);  
  delay(100);
  Serial.flush();
  Serial.println(LeftArrow); 
  delay(100);
  Serial.flush();
  Serial.println(LeftArrow);  
  delay(100);
  Serial.flush();
  Serial.println(LeftArrow);  
  delay(100);
  Serial.flush();
//  sensorValue1 = analogRead(sensorPin1);
//  Serial.println(sensorValue1); 
//  Serial.print(","); 
//  sensorValue2 = analogRead(sensorPin2);
//  Serial.println(sensorValue2);
}
