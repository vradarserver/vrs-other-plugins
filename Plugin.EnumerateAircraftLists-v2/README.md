# Plugin.EnumerateAircraftLists

This plugin is intended as an example to show how a plugin can periodically
fetch all of the aircraft lists.

The `Plugin` class is the entry point. In its `Startup` function it creates
an instance of an `AircraftListEnumerator` class and then starts a timer
that ticks once a second.

The event handler for the timer uses the aircraft list enumerator class to
take snapshots of every aircraft list and then updates the plugin's status
description to show some statistics calculated from the snapshots, just for
something to do. You can see the status description in Virtual Radar Server
by going to **Tools | Plugins**.

The aircraft list enumerator instantiates the singleton `IFeedManager`. This
object has a property called `Feeds` which returns an array of all feeds that
exist at the time the property getter is called.

One of the properties on a feed is the `IAircraftList` that tracks the aircraft
on the feed. The aircraft list has a function called `TakeSnaphot` that creates
a copy of its aircraft list and populates it with clones of every aircraft that
it is tracking.

The aircraft list enumerator simply calls `TakeSnapshot` for every feed and
returns the results.

## Relevant Interfaces

* [IAircraft (v2)](https://github.com/vradarserver/vrs/blob/v2-master/VirtualRadar.Interface/IAircraft.cs) - the aircraft returned in the snapshots.
* [IFeed (v2)](https://github.com/vradarserver/vrs/blob/v2-master/VirtualRadar.Interface/Listener/IFeed.cs) - the object that describes a feed that VRS is listening to.
* [IFeedManager (v2)](https://github.com/vradarserver/vrs/blob/v2-master/VirtualRadar.Interface/Listener/IFeedManager.cs) - the singleton that keeps track of all the feeds that VRS is listening to.
* [IAircraftList (v2)](https://github.com/vradarserver/vrs/blob/v2-master/VirtualRadar.Interface/IAircraftList.cs) - the object that uses aircraft messages to build state for all aircraft on a feed. Every IFeed has one of these.
