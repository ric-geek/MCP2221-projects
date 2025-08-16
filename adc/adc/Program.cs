using System;
using mcp2221_dll_m;

namespace adc
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
            uint[] adcDataArray = new uint[3];

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
            
            // Configure GP1
            pinFunctions[1] = MCP2221.M_MCP2221_GP_ADC; // Set as ADC1
            pinDirections[1] = MCP2221.M_MCP2221_GPDIR_INPUT; // Configured as input
            outputValues[1] = MCP2221.M_MCP2221_GPVAL_LOW; // GP0 set to low value
            
            // Set GPIO configuration
            result = MCP2221.M_Mcp2221_SetGpioSettings(handleChip, sram, pinFunctions, pinDirections, outputValues);

            if (result != MCP2221.M_E_NO_ERR)
            {
                Console.WriteLine("Error {0}", result);
            }
            
            // Set ADC voltage reference
            result = MCP2221.M_Mcp2221_SetAdcVref(handleChip, sram, MCP2221.M_VREF_2048V);
            
            if (result != MCP2221.M_E_NO_ERR)
            {
                Console.WriteLine("Error {0}", result);
            }
            
            // Get ADC data
            result = MCP2221.M_Mcp2221_GetAdcData(handleChip, adcDataArray);
            
            if (result != MCP2221.M_E_NO_ERR)
            {
                Console.WriteLine("Error {0}", result);
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