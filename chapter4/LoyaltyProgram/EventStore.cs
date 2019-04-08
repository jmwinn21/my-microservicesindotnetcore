﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace LoyaltyProgram.EventFeed {
  public class LoyaltyProgram : IEventStore {
    private static long currentSequenceNumber = 0;
    private static readonly IList<Event> database = new List<Event> ();

    public IEnumerable<Event> GetEvents (
        long firstEventSequenceNumber,
        long lastEventSequenceNumber) => database
      .Where (e =>
        e.SequenceNumber >= firstEventSequenceNumber &&
        e.SequenceNumber <= lastEventSequenceNumber)
      .OrderBy (e => e.SequenceNumber);

    public void Raise (string eventName, object content) {
      var seqNumber = Interlocked.Increment (ref currentSequenceNumber);
      database.Add (
        new Event (
          seqNumber,
          DateTimeOffset.UtcNow,
          eventName,
          content));
    }
  }
}