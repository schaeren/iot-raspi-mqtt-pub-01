# iot-raspi-mqtt-pub-01

This MQTT client application publishes messages to an MQTT broker (Mosquitto). The connection to the broker uses TLS 1.3 and client certificates. This example runs directly on the Raspberry Pi (Standard or Zero 2 W) with .NET and C#, but can also be used on any other system that supports .NET Core. 

For the corresponding subcriber application can be found at ://github.com/schaeren/iot-raspi-mqtt-sub-01.

Target system: Raspberry Pi 4 Model B or Raspberry Pi Zero 2 W, Pi OS 32-bit or 64-bit. Mosquitto MQTT broker.

Development environment: Visual Studio Code with .NET SDK 6, Visual Studio Remote Debugger.

Documentation:
- MQTT publisher and subscriber application: https://www.schaerens.ch/raspi-using-mqtt-on-raspberry-pi-with-net-and-c/
- Setting up Mosquitto MQTT broker on Raspberry Pi / Docker: https://www.schaerens.ch/raspi-setting-up-mosquitto-mqtt-broker-on-raspberry-pi-docker/
- Development environment: https://www.schaerens.ch/raspi-software-development-with-visual-studio-code-csharp-and-net-6-debugging/
