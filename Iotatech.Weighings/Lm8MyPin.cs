using System.Net;
using FluentModbus;

namespace Iotatech.Weighings;

/// <summary>
///     LM8MyPin abstraction above the Modbus communication
/// </summary>
public class Lm8MyPin
{
    /// <summary>
    ///     Initialize an instance of LM8MyPin with Modbus over TCP adapter
    /// </summary>
    /// <param name="adapterIpAddress">IP address of the adapter</param>
    public Lm8MyPin(IPAddress adapterIpAddress)
    {
        AdapterIpAddress = adapterIpAddress;
        ModbusClient = new ModbusTcpClient();
    }

    /// <summary>
    ///     Initialize LM8MyPin with Modbus over RTU
    /// </summary>
    /// <param name="serialPortName">The serial port name</param>
    public Lm8MyPin(string serialPortName)
    {
        SerialPortName = serialPortName;
        ModbusClient = new ModbusRtuClient();
    }

    /// <summary>
    ///     initialize LM8MyPin from with an existing ModbusClient
    /// </summary>
    /// <param name="modbusClient">ModbusClient instance</param>
    public Lm8MyPin(ModbusClient modbusClient)
    {
        ModbusClient = modbusClient;
    }

    private IPAddress? AdapterIpAddress { get; }
    private ModbusClient ModbusClient { get; }
    private string? SerialPortName { get; }

    /// <summary>
    ///     Connect to the adapter or serial port for communication.
    /// </summary>
    public void Connect()
    {
        if (ModbusClient is ModbusTcpClient tcpClient)
            tcpClient.Connect(AdapterIpAddress, ModbusEndianness.BigEndian);
        else if (ModbusClient is ModbusRtuClient rtuClient)
            rtuClient.Connect(SerialPortName, ModbusEndianness.BigEndian);
    }

    private decimal RegisterToWeight(short[] weightRegisters)
    {
        var weight = (ushort) weightRegisters[0];
        var isNegative = Convert.ToBoolean(weight >> 15);
        if (isNegative) weight = (ushort) (weight & 0x7FFF);

        decimal pointValue = Convert.ToDecimal((ushort) weightRegisters[1]) / ushort.MaxValue;
        decimal result = Convert.ToDecimal(weight) + pointValue;
        return result;
    }

    /// <summary>
    ///     Get the current reading of LM8MyPin asynchronously
    /// </summary>
    /// <param name="unitIdentifier">The unit identifier or address of the LM8MyPin hardware</param>
    /// <returns>Current weight</returns>
    public async Task<decimal> GetWeightAsync(byte unitIdentifier)
    {
        var weightRegisters = await ModbusClient.ReadHoldingRegistersAsync<short>(unitIdentifier, 98, 2);
        return RegisterToWeight(weightRegisters.ToArray());
    }

    /// <summary>
    ///     Get the current reading of LM8MyPin synchronously. Consider using asynchronous version whenever possible.
    /// </summary>
    /// <param name="unitIdentifier">The unit identifier or address of the LM8MyPin hardware</param>
    /// <returns>Current weight</returns>
    public decimal GetWeight(byte unitIdentifier)
    {
        var weightRegisters = ModbusClient.ReadHoldingRegisters<short>(unitIdentifier, 98, 2);
        return RegisterToWeight(weightRegisters.ToArray());
    }
}