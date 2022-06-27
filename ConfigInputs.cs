namespace Iot.Raspi.Mqtt
{
    public class ConfigInputs
    {
        /// <summary>Array of GPIO pin numbers to which the buttons are connected. Any number of buttons are supported.</summary>
        public int[] ButtonPins { get; set; } = new int[] {};
    }
}