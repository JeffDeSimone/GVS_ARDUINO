/*
 * CONNECT TO OUTGOING PORT HC-05 'DEV-B' TO VIEW DATA ON SERIAL MONITOR
 * USE THIS SKETCH ONLY FOR VIEWING SENSOR DATA ON SERIAL MONITOR.....NOT FOR FILE WRITING
 */
#include <math.h>

long tm,t,d; //variables to record time in seconds

 
//joystick setup

 int ledPin = 13;
 int joyPin1 = 1;                 // slider variable connecetd to analog pin 0
 int joyPin2 = 0;                 // slider variable connecetd to analog pin 1
 int value1 = 0;                  // variable to read the value from the analog pin 0
 int value2 = 512;  
 int repCheck = 0;
 
void setup()
{
  Serial.begin(9600);
  pinMode(0,INPUT);
  analogReference(DEFAULT);
}

 int treatValue(int data) {
  return (data * 9 / 1024) + 48;
 }

void loop()
{
  value1 = analogRead(joyPin1);  
  // this small pause is needed between reading
  // analog pins, otherwise we get the same value twice
              
  // reads the value of the variable resistor
  //value2 = analogRead(joyPin2);  

  digitalWrite(ledPin, HIGH);          
  //delay(value1);
  digitalWrite(ledPin, LOW);
  //delay(value2);

  if(value1 != repCheck){
    float rad = atan2 (value1-512, value2);
    //Serial.print("TIME: "+ String(t)+"    ");
    Serial.println(rad);
    
 
    //required for converting time to seconds
    tm = millis();
    t = tm/1000;

    Serial.flush();
    repCheck = value1;
  }
  

  delay(50);//delay of 2 seconds
}
