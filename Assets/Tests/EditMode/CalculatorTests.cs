using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculatorTests : MonoBehaviour
{
    [Test]
    public void Add()
    {
        var result = Calculator.Add(1, 5);

        Assert.AreEqual(6, result);
    }

}
