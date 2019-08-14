using CoC.Backend.Strings;
using System;

namespace CoC.Backend.Areas
{
	public class DungeonRoom
	{
		public readonly SimpleDescriptor name;

		public readonly Action onEnter;
		public readonly Action onReEnter;

		public DungeonRoom(SimpleDescriptor roomName, Action onAllEnters)
		{
			name = roomName ?? throw new ArgumentNullException(nameof(roomName));

			onEnter = onAllEnters ?? throw new ArgumentNullException();
			onReEnter = onAllEnters;
		}

		public DungeonRoom(SimpleDescriptor roomName, Action onFirstEnter, Action onSubsequentEnters)
		{
			name = roomName ?? throw new ArgumentNullException(nameof(roomName));

			onEnter = onFirstEnter ?? throw new ArgumentNullException();
			onReEnter = onSubsequentEnters ?? throw new ArgumentNullException();
		}

		//by default, buttons are reserved for North, South, East, and West, and two additonal slots, to the left and right of North. 
		//however, by default, these extra slots are null, though 
		public DungeonRoom westRoom { get; internal set; } = null;
		public DungeonRoom eastRoom { get; internal set; } = null;

		public DungeonRoom northRoom { get; internal set; } = null;

		public DungeonRoom southRoom { get; internal set; } = null;

		public DungeonRoom firstExtraPassage { get; protected set; } = null;
		public SimpleDescriptor firstExtraPassageText { get; protected set; } = GlobalStrings.None;

		public DungeonRoom secondExtraPassage { get; protected set; } = null;
		public SimpleDescriptor secondExtraPassageText { get; protected set; } = GlobalStrings.None;

	}
}