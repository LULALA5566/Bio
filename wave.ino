int sensorPin1 = A2;   // select the input pin for the potentiometer
int sensorPin2 = A3; 
int sensorPin3 = A4; 
int sensorPin4 = A5; 
int openPin = A4; 

int prev = 0; 
int buttonPin = 2; 
int buttonState = 0; 

// variable to store the value coming from the sensor
double muscleRead[4] = {0, 0, 0, 0} ;
double l_bicep = 0;  
double r_bicep = 0;
double l_tricep = 0;
double r_tricep = 0;

double norm_l_bicep = 0;  
double norm_r_bicep = 0;
double norm_l_tricep = 0;
double norm_r_tricep = 0;

double muscleAverage[4] = {0, 0, 0, 0} ;
double l_bicep_average = 0;  
double r_bicep_average = 0;
double l_tricep_average = 0;
double r_tricep_average = 0;

double muscleMax[4] = {0, 0, 0, 0} ;
double l_bicep_max = 0;  
double r_bicep_max = 0;
double l_tricep_max = 0;
double r_tricep_max = 0;


double rate = 0.67; 
double trirate = 0.75; 
double max_threshold = 0.90; 

char dataStr[100] = ""; 
int choice = 0; 

bool message = false; 
int start_cal = 0; 
bool begin_calibration = false; 
bool calibrated = false; 
double cal_result[4] = {0, 0, 0, 0};
int calibration_iteration = 0;

char re = '7';
bool transmit = true;

void setup() {
  // declare the ledPin as an OUTPUT:
    Serial.begin(9600); 
    pinMode(sensorPin1, INPUT);
    pinMode(sensorPin2, INPUT);
    pinMode(sensorPin3, INPUT);
    pinMode(sensorPin4, INPUT); 
}

void loop() {
if(Serial.available()){
  re = Serial.read();
}
  
//  Serial.print(buttonState);
//  
  if (begin_calibration == false ) {
//    if (buttonState == 0 && message == false) {
////    Serial.println("press button to begin calibration");
//    message = true;
//    }
    if (re == '1') {
          Serial.println(5);

      begin_calibration = true;   
//      Serial.println("BEGIN CALIBRATION");
    }
//    if (start_cal != 49 && message == false) {
//    Serial.println("press 1 to begin calibration");
//    message = true;
//    }
//    
//    if (Serial.available()){
//      start_cal = Serial.read();
//    }
//    if (start_cal == 49) {
//      begin_calibration = true;   
//      Serial.println("BEGIN!!!");
//    }
  }
  

  if (calibrated == false && begin_calibration == true) {

  // DO CALIBRATION
  
    calibration_iteration ++; 
    delay(10);

   
    l_bicep = analogRead(sensorPin1); 
//    Serial.println(l_bicep);
    l_bicep_average = (l_bicep_average * calibration_iteration + l_bicep)/ (calibration_iteration + 1); 
    l_bicep_max = (l_bicep<l_bicep_max)?l_bicep_max:l_bicep;
    
    r_bicep = analogRead(sensorPin2);
    r_bicep_average = (r_bicep_average * calibration_iteration + r_bicep)/ (calibration_iteration + 1); 
    r_bicep_max = (r_bicep<r_bicep_max)?r_bicep_max:r_bicep;
    
    l_tricep = analogRead(sensorPin3); 
    l_tricep_average = (l_tricep_average * calibration_iteration + l_tricep)/ (calibration_iteration + 1); 
    l_tricep_max = (l_tricep<l_tricep_max)?l_tricep_max:l_tricep;
    
    r_tricep = analogRead(sensorPin4);
    r_tricep_average = (r_tricep_average * calibration_iteration + r_tricep)/ (calibration_iteration + 1); 
    r_tricep_max = (r_tricep<r_tricep_max)?r_tricep_max:r_tricep;
//    delay(10);
    
    if (calibration_iteration == 1000) {
//      Serial.println("----- calibration ended -----");
      calibrated = true; 
//      Serial.print(" -> left bicep"); 
//      Serial.print(l_bicep_average); 
//      Serial.print("  ");
//      Serial.println(l_bicep_max);
      cal_result[0] = l_bicep_max * rate;
//      
//      Serial.print(" -> right bicep");
//      Serial.print(r_bicep_average); 
//      Serial.print("  ");
//      Serial.println(r_bicep_max); 
      cal_result[1] = r_bicep_max * rate;
//      
//      Serial.print(" -> left tricep");
//      Serial.print(l_tricep_average);  
//      Serial.print("  ");
//      Serial.println(l_tricep_max); 
      cal_result[2] = l_tricep_max * trirate;
//      
//      Serial.print(" -> right tricep"); 
//      Serial.print(r_tricep_average);  
//      Serial.print("  ");
//      Serial.println(r_tricep_max); 
      cal_result[3] = r_tricep_max * trirate;
        Serial.println(0);
    }
    
  
  } else {

    
    
    l_bicep = analogRead(sensorPin1); 
    norm_l_bicep = l_bicep/l_bicep_average;
//    Serial.print(l_bicep);
//    Serial.print(",");
    r_bicep = analogRead(sensorPin2);
    norm_r_bicep = r_bicep/r_bicep_average;
//    Serial.print(r_bicep);
//    Serial.print(",");
    l_tricep = analogRead(sensorPin3); 
    norm_l_tricep = l_tricep/l_tricep_average;
//    Serial.print(l_tricep);
//    Serial.print(",");
    r_tricep = analogRead(sensorPin4);
    norm_r_tricep = r_tricep/r_tricep_average;
//    Serial.println(r_tricep);

    // l_bicep_max = (l_bicep<l_bicep_max)?l_bicep_max:l_bicep;
    int max_bi = (norm_l_bicep < norm_r_bicep)?2:1;
    int max_bi_ = (norm_l_bicep < norm_r_bicep)?norm_r_bicep:norm_l_bicep;
    int max_tri = (norm_l_tricep < norm_r_tricep)?2:1;
    int max_tri_ = (norm_l_tricep < norm_r_tricep)?norm_r_tricep:norm_l_tricep;
    int max_ = (max_bi_ < max_tri_)?max_tri:max_bi; 
//    Serial.print(max_); 
//    Serial.println(); 
    
    if (l_bicep > cal_result[0] && max_ == 1 ) {
     Serial.println(6); 
     //Serial.println(); 
//     Serial.print(l_bicep);
//     rate = max_threshold; 
     prev = 1; 
     delay(2000); 
    } else if (r_bicep > cal_result[1] && max_ == 2) {
     Serial.println(8); 
     //Serial.println(); 
     prev = 2; 
//     Serial.print(r_bicep); 
//      rate = max_threshold; 
     delay(2000); 
    } 
    else if (l_tricep > cal_result[2]) {
     Serial.println(7); 
     Serial.println(); 
//     Serial.print(l_tricep);
//     trirate = max_threshold; 
     delay(2000); 
    } else if (r_tricep > cal_result[3]) {
     Serial.println(9); 
     Serial.println(); 
//     Serial.println(r_tricep);
//     trirate = max_threshold; 
     delay(2000); 
    } else {
//      rate -= 0.002;
//      trirate -= 0.002; 
    }
    delay(100);    
    }
  
}
