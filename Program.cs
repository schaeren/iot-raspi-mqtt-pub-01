using System;
using System.Device.Gpio;

using Iot.Raspi.Logger;

namespace Iot.Raspi.Mqtt
{
    /// <summary>
    /// PUBLISH TO MQTT BROKER:
    /// Demonstrates how to use MQTTnet on Raspberry Pi.
    /// The program polls the buttons (GPIO ports have to be configured in appsettings.json) and 
    /// publishes changes (button press/release) to MQTT topics inputs/button{buttonIndex}/isPressed and 
    /// inputs/button{buttonIndex}/lastChangedAt.
    /// </summary>
    class Program
    {
        // Set logger context
        private static Serilog.ILogger Log => Serilog.Log.ForContext<Program>();

        private static GpioController? gpio;
        private static List<PinValue> previousButtonStates = new List<PinValue>();
        private static MqttClient? mqttClient;

        // Entry point of program
        static async Task Main(string[] args)
        {
            try 
            {
                // Initialize logger (Serilog), see appsettings.json for configuration.
                // Here() is an extension method to Serilog.ILogger, it automatically extends the logger 
                // context with information about class name, method name, source file path and 
                // line number. This information may be written to log outputs (see configuration).
                IotLogger.Init("appsettings.json");
                Log.Here().Information("Application starting ...");

                // Initialize list with previous button states equal to 'high' (button not pressed).
                previousButtonStates = Enumerable.Repeat(PinValue.High, Config.Instance.Inputs.ButtonPins.Length).ToList();

                // Configure GPIO digital inputs
                gpio = new GpioController();
                foreach (var buttonPin in Config.Instance.Inputs.ButtonPins)
                {
                    gpio.OpenPin(buttonPin, PinMode.InputPullUp);    
                }

                // Configure MQTT client
                mqttClient = new MqttClient();
                await mqttClient.StartAsync();
                
                // Read digital inputs and publish changes
                while (true)
                {
                    for (var i=0; i<Config.Instance.Inputs.ButtonPins.Length; i++)
                    {
                        await SendMessagesIfButtonHasChanged(i);
                    }

                    // Delay should be longer than max debouncing time of button
                    Thread.Sleep(20);
                }
            }
            catch (Exception ex)
            {
                Log.Here().Fatal("Failed: {exception}", ex.Message );
            }
            finally
            {
                Serilog.Log.CloseAndFlush();
            }
        }

        /// <summary>
        /// Check if button state (PinValue.High, PinValue.Low) has changed. If so, publish two MQTT
        /// messages inputs/button{buttonIndex}/isPressed and inputs/button{buttonIndex}/lastChanged.
        /// The button inputs have PinValue.High state when released and PinValue.Low wenn pressed.
        /// </summary>
        /// <param name="buttonIndex">The buttons configured in appsettings.json are referenced here 
        /// with a zero based index.</param>
        /// <returns></returns>
        private static async Task SendMessagesIfButtonHasChanged(int buttonIndex)
        {
            if (gpio == null) { throw new Exception("Failed to initialize GPIO."); }
            if (mqttClient == null) { throw new Exception("Failed to initalize MQTT client."); }

            var buttonState = gpio.Read(Config.Instance.Inputs.ButtonPins[buttonIndex]);
            if (buttonState != previousButtonStates[buttonIndex])
            {
                previousButtonStates[buttonIndex] = buttonState;
                await mqttClient.PublishMessageAsync(
                    $"inputs/button{buttonIndex}/isPressed", 
                    (buttonState == PinValue.Low).ToString());
                await mqttClient.PublishMessageAsync(
                    $"inputs/button{buttonIndex}/lastChangedAt", 
                    DateTime.Now.ToString("o"));
            }
        }

    }
}