using System;
using System.Net;
using Iotatech.Weighings;
using NUnit.Framework;

namespace Tests;

public class LM8MyPinTests
{
    private Lm8MyPin _myPin;

    [SetUp]
    public void Setup()
    {
        _myPin = new Lm8MyPin(IPAddress.Parse("127.0.0.1"));
        _myPin.Connect();
    }

    [Test]
    public void GetWeight()
    {
        Console.WriteLine(_myPin.GetWeight(1)); // assuming the simulated slave is on localhost with unit id 1.
    }
}