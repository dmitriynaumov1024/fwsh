namespace Fwsh.Tests;

using System;
using NUnit.Framework;
using Fwsh.Common;

[TestFixture]
public class EventsTest
{
    [Test]
    public void TestEventCreation()
    {
        var _event = new PaycheckReceiveEvent(){
            PaycheckId = 13,
            Balance = -6000
        };

        Console.Write (
            "PaycheckReceiveEvent #{3}\n  when: {0:D04}-{1:D02}-{2:D02} \n  balance: {4}\n", 
            _event.EventYear, _event.EventMonth, _event.EventDay, _event.PaycheckId, _event.Balance
        );

        Assert.AreEqual (
            $"{DateTime.UtcNow:yyyy-MM-dd}", 
            $"{_event.EventYear:D04}-{_event.EventMonth:D02}-{_event.EventDay:D02}"
        );
    }
}
