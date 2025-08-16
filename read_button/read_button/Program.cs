using System;
using mcp2221_dll_m;

namespace read_button
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            
            uint device = 0;
            
            // Default values
            uint vid = 0x4D8;
            uint pid = 0xDD;
            
            int result;
            byte sram = MCP2221.M_RUNTIME_SETTINGS;
            byte[] pinFunctions = new byte[4];
            byte[] pinDirections = new byte[4];
            byte[] outputValues = new byte[4];
            byte[] gpioValues = {0, MCP2221.M_NO_CHANGE, MCP2221.M_NO_CHANGE, MCP2221.M_NO_CHANGE };
            
            result = MCP2221.M_Mcp2221_GetConnectedDevices(vid, pid, ref device);
            
            // Check if devices is connected
            if (result != MCP2221.M_E_NO_ERR)
            {
                
                Console.WriteLine("MCP2221 no connected!");

                return;

            }
            
            // Open the communication
            var handleChip = MCP2221.M_Mcp2221_OpenByIndex(vid, pid, device-1);
            
            // Read GPIO configuration
            result = MCP2221.M_Mcp2221_GetGpioSettings(handleChip, sram, pinFunctions, pinDirections, outputValues);
            
            if (result != MCP2221.M_E_NO_ERR)
            {
                Console.WriteLine("Error {0}", result);
            }

            // Configure GP0
            pinFunctions[0] = MCP2221.M_MCP2221_GPFUNC_IO; // Set as GPIO
            pinDirections[0] = MCP2221.M_MCP2221_GPDIR_OUTPUT; // Configured as output
            outputValues[0] = MCP2221.M_MCP2221_GPVAL_LOW; // Set to low value
                                                           
            // Configure GP1
            pinFunctions[1] = MCP2221.M_MCP2221_GPFUNC_IO; // Set as GPIO
            pinDirections[1] = MCP2221.M_MCP2221_GPDIR_INPUT; // Configured as input
            outputValues[1] = MCP2221.M_MCP2221_GPVAL_LOW; // Set to low value
            
            // Set GPIO configuration
            result = MCP2221.M_Mcp2221_SetGpioSettings(handleChip, sram, pinFunctions, pinDirections, outputValues);

            if (result != MCP2221.M_E_NO_ERR)
            {
                Console.WriteLine("Error {0}", result);
            }
            
            // Ask the user to press the button
            Console.WriteLine("Press the button to turn on LED");
            Console.ReadLine();
            
            // Read GPIO
            result = MCP2221.M_Mcp2221_GetGpioValues(handleChip, gpioValues);
            
            if (result != MCP2221.M_E_NO_ERR)
            {
                Console.WriteLine("Error {0}", result);
            }

            if (gpioValues[1] == 1)
            {
                
                // Set GP0 to high value (LED ON)
                gpioValues[0] = MCP2221.M_MCP2221_GPVAL_HIGH;
                gpioValues[1] = MCP2221.M_NO_CHANGE;
                gpioValues[2] = MCP2221.M_NO_CHANGE;
                gpioValues[3] = MCP2221.M_NO_CHANGE;
            
                result = MCP2221.M_Mcp2221_SetGpioValues(handleChip, gpioValues);
            
                if (result != MCP2221.M_E_NO_ERR)
                {
                    Console.WriteLine("Error {0}", result);
                }
                
            }
            else
            {
                // Set GP0 to low value (LED OFF)
                gpioValues[0] = MCP2221.M_MCP2221_GPVAL_LOW;
                gpioValues[1] = MCP2221.M_NO_CHANGE;
                gpioValues[2] = MCP2221.M_NO_CHANGE;
                gpioValues[3] = MCP2221.M_NO_CHANGE;
            
                result = MCP2221.M_Mcp2221_SetGpioValues(handleChip, gpioValues);
            
                if (result != MCP2221.M_E_NO_ERR)
                {
                    Console.WriteLine("Error {0}", result);
                }
                
            }
            
            // Close communication with the chip
            result = MCP2221.M_Mcp2221_Close(handleChip);
            
            if (result != MCP2221.M_E_NO_ERR)
            {
                Console.WriteLine("Error {0}", result);
            }
            
        }
    }
}