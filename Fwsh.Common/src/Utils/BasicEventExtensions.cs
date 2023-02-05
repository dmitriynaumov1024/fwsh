namespace Fwsh.Common;

using System;

public static class BasicEventExtensions
{
    public static TEvent Now<TEvent>(this TEvent @event) where TEvent : BasicEvent
    {
        var now = DateTime.UtcNow;
        @event.EventYear = now.Year;
        @event.EventMonth = now.Month;
        @event.EventDay = now.Day;

        return @event;
    }
}
