/*
 * CONNECT TO OUTGOING PORT HC-05 'DEV-B' TO VIEW DATA ON SERIAL MONITOR
 * USE THIS SKETCH ONLY FOR VIEWING SENSOR DATA ON SERIAL MONITOR.....NOT FOR FILE WRITING
 */
int temp; //variable to hold temperature sensor value
long tm,t,d; //variables to record time in seconds


//joystick setup

 int ledPin = 13;
 int joyPin1 = 0;                 // slider variable connecetd to analog pin 0
 int joyPin2 = 1;                 // slider variable connecetd to analog pin 1
 int value1 = 0;                  // variable to read the value from the analog pin 0
 int value2 = 0;  
void setup()
{
  Serial.begin(9600);
  pinMode(0,INPUT);//temperature sensor connected to analog 0
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
  delay(100);            
  // reads the value of the variable resistor
  value2 = analogRead(joyPin2);  

  digitalWrite(ledPin, HIGH);          
  delay(value1);
  digitalWrite(ledPin, LOW);
  delay(value2);
  Serial.print('J');
  Serial.print(treatValue(value1));
  Serial.println(treatValue(value2));
  //required for converting time to seconds
  tm = millis();
  t = tm/1000;
  d = tm%1000;

  Serial.flush();

  //printing time in seconds


  //printing temperature sensor values

  
  delay(2000);//delay of 2 seconds
}
