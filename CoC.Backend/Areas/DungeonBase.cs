using CoC.Backend.Engine;
using CoC.Backend.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CoC.Backend.Areas
{
	public enum Direction { NORTH, SOUTH, EAST, WEST }

	public static class DirectionStrings
	{
		public static string AsString(this Direction direction)
		{
			switch (direction)
			{
				case Direction.NORTH: return "North";
				case Direction.SOUTH: return "South";
				case Direction.EAST: return "East";
				case Direction.WEST: return "West";
				default: throw new ArgumentException("Direction was not recognized");
			}
		}
	}

	public abstract class DungeonBase : VisitableAreaBase
	{
		private DungeonRoom currentRoom;

		private readonly HashSet<DungeonRoom> visitedRooms = new HashSet<DungeonRoom>();

		protected DungeonBase(SimpleDescriptor dungeonName) : base(dungeonName)
		{
			currentRoom = null;
		}

		//the dungeon entrance.
		protected DungeonRoom entranceRoom { get; }

		//Links together a series of dungeon rooms. Note that dungeons can now have one-way doors (think TLoZ). DungeonRooms are optional, but recommended, as they make it
		//easier to debug or maintain, as all logic for a single room can be done in its own little instance.
		protected void LinkRooms(DungeonRoom sourceRoom, DungeonRoom destinationRoom, Direction direction, bool isOneWay = false)
		{
			if (sourceRoom is null) throw new ArgumentNullException(nameof(sourceRoom));
#if DEBUG
			if (destinationRoom is null)
			{
				Debug.WriteLine("Attaching null to the source room: '" + sourceRoom.name() + "' in the " + direction.AsString() + "direction. By default, this is already null;" +
					" if you meant to attach a non-null room, something broke. If you meant for this to be null, please ignore this message.");
			}
#endif
			switch (direction)
			{
				case Direction.EAST:
					sourceRoom.eastRoom = destinationRoom;
					if (!isOneWay) destinationRoom.westRoom = sourceRoom;
					break;
				case Direction.WEST:
					sourceRoom.westRoom = destinationRoom;
					if (!isOneWay) destinationRoom.eastRoom = sourceRoom;
					break;
				case Direction.SOUTH:
					sourceRoom.southRoom = destinationRoom;
					if (!isOneWay) destinationRoom.northRoom = sourceRoom;
					break;
				case Direction.NORTH:
					sourceRoom.northRoom = destinationRoom;
					if (!isOneWay) destinationRoom.southRoom = sourceRoom;
					break;
			}
		}


		//on its own, this only factors in to statistics and perhaps NPC who care (like Mareth, Jojo, etc), however, it gets additional effects based on disable flag.
		public abstract bool isCompleted { get; protected set; }

		//if true, causes this dungeon to be disabled when the isCompleted flag is set to true. the Player can no longer enter the dungeon once they leave.
		public abstract bool disableOnCompletion { get; }

		//does the player get to see the dungeon in the list of available dungeons, assuming they found it? If false, there needs to be some other way to enter the dungeon.
		public abstract bool isHidden { get; }

		internal override void RunArea()
		{
			if (currentRoom is null)
			{
				currentRoom = entranceRoom;
				OnDungeonEnter();
			}

			currentRoom.onEnter();
		}

		//a virtual function that handles any random data initialization, as needed. it should not print anything.
		protected virtual void OnDungeonEnter() { }

	}
}
