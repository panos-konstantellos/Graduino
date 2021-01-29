// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

#include <OneWire.h>
#include <LiquidCrystal.h>
#include <Wire.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_BMP085_U.h>

OneWire ds(8);  //Temperature chip on digital pin 8

Adafruit_BMP085_Unified bmp = Adafruit_BMP085_Unified(10180); //barometric pressure sensor

int humPin = 0;      //humidity analog pin
LiquidCrystal lcd(12, 11, 5, 4, 3, 2);


void setup(void) 
{
  Serial.begin(9600);
  lcd.begin(16, 2);
  lcd.print("TEMP/HUM/PRES");

  if (!bmp.begin())
  {
    Serial.print("Ooops, no BMP085 detected ... Check your wiring or I2C ADDR!\n"); /* There was a problem detecting the BMP085 ... check your connections */
    while (1);
  }
}

void loop(void) 
{
  float temperature = getTemp();
  double humidity = getHumidity(temperature);
  double pressure;
  sensors_event_t event;
  bmp.getEvent(&event);
  
  if (event.pressure)
  {
    pressure = event.pressure;
  }
  
  String message = "{ \"temperature\": \"" + (String)temperature + "\", \"humidity\": \"" + (String)humidity + "\", \"pressure\": \"" + (String)pressure + "\" }";

  Serial.println(message);

  lcd.setCursor(0, 1);
  lcd.print(temperature);
  lcd.print("/");
  lcd.print(humidity);
  lcd.print("/");
  lcd.print(pressure);
  
  delay(1000);
}

double getHumidity(float temp) 
{
  double outputVoltage = analogRead(humPin) / 1023.0 * 5.0; // convert it into voltage (Vcc = 5V)
  
  double relativeHumidity = 161.0 * outputVoltage / 5 - 25.8; // calculates the sensor relative humitidy
  
  return relativeHumidity / (1.0546 - 0.0026 * temp); // calculates temperature by adapting current temperature to relative humidity
}

float getTemp() 
{
  byte data[12];
  byte addr[8];

  if (!ds.search(addr)) 
  {
    ds.reset_search(); //no more sensors on chain, reset search

    return -1000;
  }

  if (OneWire::crc8( addr, 7) != addr[7])
  {
    return -1000;
  }

  if (addr[0] != 0x10 && addr[0] != 0x28)
  {
    return -1000;
  }

  ds.reset();
  ds.select(addr);
  ds.write(0x44, 1); // start conversion, with parasite power on at the end

  byte present = ds.reset();
  ds.select(addr);
  ds.write(0xBE); // Read Scratchpad

  for (int i = 0; i < 9; i++) // we need 9 bytes
  {
    data[i] = ds.read();
  }

  ds.reset_search();

  byte MSB = data[1];
  byte LSB = data[0];

  float tempRead = ((MSB << 8) | LSB); //using two's compliment

  return tempRead / 16;
}
