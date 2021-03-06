﻿//Butt.cs
//Description:
//Author: JustSomeGuy
//12/26/2018, 11:10 PM
using CoC.Backend.BodyParts.EventHelpers;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WeakEvent;

namespace CoC.Backend.BodyParts
{
	//was originally just a wrapper for int.
	//but now it has validation! and a descriptor!
	//and tattoos (what a joy)

	public sealed partial class ButtTattooLocation : TattooLocation
	{

		private static readonly List<ButtTattooLocation> _allLocations = new List<ButtTattooLocation>();

		public static readonly ReadOnlyCollection<ButtTattooLocation> allLocations;

		private readonly byte index;

		static ButtTattooLocation()
		{
			allLocations = new ReadOnlyCollection<ButtTattooLocation>(_allLocations);
		}

		private ButtTattooLocation(byte index, TattooSizeLimit limitSize, SimpleDescriptor btnText, SimpleDescriptor locationDesc) : base(limitSize, btnText, locationDesc)
		{
			this.index = index;
		}

		public static ButtTattooLocation LEFT_CHEEK = new ButtTattooLocation(0, MediumTattoosOrSmaller, LeftCheekButton, LeftCheekLocation);
		public static ButtTattooLocation RIGHT_CHEEK = new ButtTattooLocation(1, MediumTattoosOrSmaller, RightCheekButton, RightCheekLocation);

		public static bool LocationsCompatible(ButtTattooLocation first, ButtTattooLocation second)
		{
			return true;
		}
	}

	public sealed class ButtTattoo : TattooablePart<ButtTattooLocation>
	{
		public ButtTattoo(IBodyPart source, CreatureStr allTattoosShort, CreatureStr allTattoosLong) : base(source, allTattoosShort, allTattoosLong)
		{ }

		public override int MaxTattoos => ButtTattooLocation.allLocations.Count;

		public override IEnumerable<ButtTattooLocation> availableLocations => ButtTattooLocation.allLocations;

		public override bool LocationsCompatible(ButtTattooLocation first, ButtTattooLocation second) => ButtTattooLocation.LocationsCompatible(first, second);
	}

	public sealed partial class Butt : SimpleSaveablePart<Butt, ButtData>, IShrinkable //Gro+ doesnt work on butt.
	{
		public const byte BUTTLESS = 0;
		public const byte TIGHT = 2;
		public const byte AVERAGE = 4;
		public const byte NOTICEABLE = 6;
		public const byte LARGE = 8;
		public const byte JIGGLY = 10;
		public const byte EXPANSIVE = 13;
		public const byte HUGE = 16;
		public const byte INCONCEIVABLY_BIG = 20;

		public readonly bool hasButt;// => size >= TIGHT;
		public int index => size;

		public byte smallestPossibleButtSize => hasButt ? (CreatureStore.GetCreatureClean(creatureID)?.perks.baseModifiers.minButtSize.GetValue() ?? TIGHT) : BUTTLESS;
		public byte largestPossibleButtSize => hasButt ? INCONCEIVABLY_BIG : BUTTLESS;

		public byte size
		{
			get => _buttSize;
			private set
			{

				Utils.Clamp(ref value, smallestPossibleButtSize, largestPossibleButtSize);
				if (_buttSize != value)
				{
					var oldData = AsReadOnlyData();
					_buttSize = value;
					NotifyDataChanged(oldData);
				}
			}
		}
		private byte _buttSize;

		internal void CheckButt()
		{
			size = size;
		}

		public readonly ButtTattoo tattoos;

		internal Butt(Guid creatureID, byte size) : base(creatureID)
		{
			if (size < TIGHT)
			{
				hasButt = false;
				_buttSize = BUTTLESS;
			}
			else
			{
				hasButt = true;
				_buttSize = Utils.Clamp2(size, TIGHT, INCONCEIVABLY_BIG);
			}

			tattoos = new ButtTattoo(this, AllTattoosShort, AllTattoosLong);
		}

		internal static Butt GenerateButtless(Guid creatureID)
		{
			return new Butt(creatureID, 0);
		}

		public override string BodyPartName() => Name();

		public override ButtData AsReadOnlyData()
		{
			return new ButtData(this);
		}

		public byte GrowButt(byte amount = 1)
		{
			byte oldSize = size;
			size = size.add(amount);
			return size.subtract(oldSize);
		}

		public byte ShrinkButt(byte amount = 1)
		{
			byte oldSize = size;
			size = size.subtract(amount);
			return oldSize.subtract(size);
		}

		public short SetButtSize(byte newSize)
		{
			var oldSize = size;
			size = newSize;
			return newSize.delta(oldSize);
		}
		#region Text
		public string SizeAsAdjective() => BuildStrings.ButtAdjective(size);

		public string ShortDescription() => BuildStrings.ButtShortDescription(size, false);
		public string SingleItemDescription() => BuildStrings.ButtShortDescription(size, true);

		public string LongDescription(bool alternateFormat, byte muscleTone) => BuildStrings.ButtLongDescription(size, muscleTone, alternateFormat);
		#endregion

		public override bool IsIdenticalTo(ButtData original, bool ignoreSexualMetaData)
		{
			return !(original is null) && size == original.size && tattoos.IsIdenticalTo(original.tattoos);
		}

		internal override bool Validate(bool correctInvalidData)
		{
			//auto-validates data.
			size = size;
			return true;
		}

		bool IShrinkable.CanReducto()
		{
			return size > smallestPossibleButtSize;
		}

		string IShrinkable.UseReducto()
		{
			var oldData = AsReadOnlyData();

			bool nothingHappened = false;
			if (!((IShrinkable)this).CanReducto())
			{
				nothingHappened = true;
			}
			if (size >= HUGE)
			{
				size = (byte)Math.Round(size * 2.0 / 3);
			}
			else if (size >= JIGGLY)
			{
				size -= 3;
			}
			else if (size > TIGHT)
			{
				//shrink by up to 3, to a minimum rating of TIGHT
				byte val = (byte)Math.Min(3, size - TIGHT);
				//increase by 1, as rand returns 0 to (val-1), we want 1 to val
				size -= (byte)(Utils.Rand(val) + 1);
			}
			return ReductoButt(oldData, nothingHappened);
		}
	}

	public sealed class ButtData : SimpleData
	{
		public readonly byte size;

		public readonly ReadOnlyTattooablePart<ButtTattooLocation> tattoos;

		#region Text
		public string SizeAsAdjective() => BuildStrings.ButtAdjective(size);

		public string ShortDescription() => BuildStrings.ButtShortDescription(size, false);
		public string SingleItemDescription() => BuildStrings.ButtShortDescription(size, true);

		public string LongDescription(bool alternateFormat, byte muscleTone) => BuildStrings.ButtLongDescription(size, muscleTone, alternateFormat);
		#endregion

		internal ButtData(Butt source) : base(source?.creatureID ?? throw new ArgumentNullException(nameof(source)))
		{
			size = source.size;
			tattoos = source.tattoos.AsReadOnlyData();
		}

		public ButtData(Guid creatureID, byte buttSize, ReadOnlyTattooablePart<ButtTattooLocation> buttTattoos) : base(creatureID)
		{
			size = buttSize;
			tattoos = buttTattoos ?? new ReadOnlyTattooablePart<ButtTattooLocation>();
		}
	}
}
