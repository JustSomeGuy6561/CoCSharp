﻿//Clit.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 6:03 PM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.SaveData;
using CoC.Backend.Tools;
using System;
using System.Diagnostics;
using System.Linq;

namespace CoC.Backend.BodyParts
{

	public enum ClitPiercings { CHRISTINA, HOOD_VERTICAL, HOOD_HORIZONTAL, HOOD_TRIANGLE, CLIT_ITSELF, LARGE_CLIT_1, LARGE_CLIT_2, LARGE_CLIT_3 }

	//note: perks are guarenteed to be valid by the time this is created, so it's post perk init won't be called.
	public sealed partial class Clit : SimpleSaveablePart<Clit, ClitData>, IGrowable, IShrinkable
	{
		public override string BodyPartName() => Name();

		internal float clitGrowthMultiplier = 1;
		internal float clitShrinkMultiplier = 1;
		internal float minClitSize
		{
			get => _minClitSize;
			set
			{
				_minClitSize = value;
				if (length < minSize)
				{
					length = minSize;
				}
			}
		}
		private float _minClitSize = MIN_CLIT_SIZE;

		internal float minNewClitSize;

		private float resetSize => Math.Max(minNewClitSize, minClitSize);

		public uint penetrateCount { get; private set; } = 0;

		public uint orgasmCount => parent.orgasmCount;

		private readonly Vagina parent;
		private int vaginaIndex => CreatureStore.TryGetCreature(creatureID, out Creature creature) ? creature.genitals.vaginas.IndexOf(parent) : 0;

		private static readonly ClitPiercings[] requiresFetish = { ClitPiercings.LARGE_CLIT_1, ClitPiercings.LARGE_CLIT_2, ClitPiercings.LARGE_CLIT_3 };
		private const JewelryType SUPPORTED_CLIT_PIERCINGS = JewelryType.BARBELL_STUD | JewelryType.RING | JewelryType.SPECIAL;

		private bool piercingFetish => BackendSessionSave.data.piercingFetishEnabled;

		public const float MIN_CLIT_SIZE = 0.25f;
		public const float MIN_CLITCOCK_SIZE = 2f;
		public const float DEFAULT_CLIT_SIZE = 0.25f;
		public const float MAX_CLIT_SIZE = 100f;

		private float minSize
		{
			get
			{
				float currMin = minClitSize;
				if (omnibusClit && currMin < MIN_CLITCOCK_SIZE)
				{
					return MIN_CLITCOCK_SIZE;
				}
				return Math.Max(MIN_CLIT_SIZE, currMin);
			}
		}
		public float length
		{
			get => _length;
			private set
			{
				Utils.Clamp(ref value, minSize, MAX_CLIT_SIZE);
				if (_length != value)
				{
					var oldData = AsReadOnlyData();
					_length = value;
					NotifyDataChanged(oldData);
				}
			}
		}
		private float _length;

		public bool omnibusClit
		{
			get => _omnibusClit;
			private set
			{
				if (_omnibusClit != value)
				{
					ClitData oldData = AsReadOnlyData();
					_omnibusClit = value;
					if (length < minSize)
					{
						_length = minSize; //don't let the length fire the event. we'll handle it.
					}
					NotifyDataChanged(oldData);
				}
			}
		}
		private bool _omnibusClit;
		public readonly Piercing<ClitPiercings> clitPiercings;

		internal Clit(Guid creatureID, Vagina source, VaginaPerkHelper initialPerkData, bool isOmnibusClit = false)
			: this(creatureID, source, initialPerkData, null, isOmnibusClit)
		{ }

		internal Clit(Guid creatureID, Vagina source, VaginaPerkHelper initialPerkData, float clitSize, bool isOmnibusClit = false)
			: this(creatureID, source, initialPerkData, (float?)clitSize, isOmnibusClit)
		{ }

		private Clit(Guid creatureID, Vagina source, VaginaPerkHelper initialPerkData, float? clitSize, bool isOmnibusClit) : base(creatureID)
		{
			parent = source ?? throw new ArgumentNullException(nameof(source));
			_length = initialPerkData.NewClitSize(clitSize);
			if (isOmnibusClit && MIN_CLITCOCK_SIZE > length)
			{
				_length = MIN_CLITCOCK_SIZE;
			}

			_omnibusClit = isOmnibusClit;
			clitPiercings = new Piercing<ClitPiercings>(PiercingLocationUnlocked, JewelryTypeSupported);

			_minClitSize = initialPerkData.MinClitSize;
			minNewClitSize = initialPerkData.DefaultNewClitSize;
			clitGrowthMultiplier = initialPerkData.ClitGrowthMultiplier;
			clitShrinkMultiplier = initialPerkData.ClitShrinkMultiplier;
		}

		public override ClitData AsReadOnlyData()
		{
			return new ClitData(this, vaginaIndex);
		}

		public bool omnibusActive => CreatureStore.GetCreatureClean(creatureID)?.cocks.Count == 0 && omnibusClit;

		public Cock AsClitCock()
		{
			if (!omnibusClit)
			{
				return null;
			}

			if (clitCock == null)
			{
				clitCock = Cock.GenerateClitCock(creatureID, this);
			}
			else
			{
				clitCock.SetLength(length + 5);
			}
			return clitCock;
		}
		private Cock clitCock = null;

		internal uint asCockSexCount => clitCock?.sexCount ?? 0;
		internal uint asCockSoundCount => clitCock?.soundCount ?? 0;
		internal uint asCockOrgasmCount => clitCock?.orgasmCount ?? 0;
		internal uint asCockDryOrgasmCount => clitCock?.dryOrgasmCount ?? 0;


		public void Restore()
		{
			omnibusClit = false;
			length = MIN_CLIT_SIZE;
		}

		public void Reset()
		{
			Restore();
			clitPiercings.Reset();
		}

		internal bool ActivateOmnibusClit()
		{
			if (omnibusClit)
			{
				return false;
			}
			omnibusClit = true;
			return true;
		}

		internal bool DeactivateOmnibusClit()
		{
			if (!omnibusClit)
			{
				return false;
			}
			omnibusClit = false;
			return true;
		}

		internal float growClit(float amount, bool ignorePerks = false)
		{
			if (length >= MAX_CLIT_SIZE || amount <= 0)
			{
				return 0;
			}

			//hope this never matters but floats don't wrap. which means we're fine, though if it ever happens in debug land, we'll know.
			float oldLength = length;
			if (!ignorePerks)
			{
				length += amount * clitGrowthMultiplier;
			}
			else
			{
				length += amount;
			}
			return length - oldLength;
		}

		internal float shrinkClit(float amount, bool ignorePerks = false)
		{
			if (length <= MIN_CLIT_SIZE || amount <= 0)
			{
				return 0;
			}
			//hope this never matters but floats don't wrap. which means we're fine, though if it ever happens in debug land, we'll know.
			float oldLength = length;
			if (!ignorePerks)
			{
				length -= amount * clitShrinkMultiplier;
			}
			else
			{
				length -= amount;
			}
			return oldLength - length;
		}

		internal float SetClitSize(float newSize)
		{
			length = newSize;
			return length;
		}

		internal void DoPenetration()
		{
			penetrateCount++;
		}

		internal override bool Validate(bool correctInvalidData)
		{
			length = length;
			return clitPiercings.Validate(correctInvalidData);
		}



		#region Piercing Related
		private bool PiercingLocationUnlocked(ClitPiercings piercingLocation)
		{

			if (!requiresFetish.Contains(piercingLocation))
			{
				return true;
			}
			else if (!piercingFetish)
			{
				return false;
			}
			else if (piercingLocation == ClitPiercings.LARGE_CLIT_1)
			{
				return length >= 3;
			}
			else if (piercingLocation == ClitPiercings.LARGE_CLIT_2)
			{
				return length >= 5;
			}
			else if (piercingLocation == ClitPiercings.LARGE_CLIT_3)
			{
				return length >= 7;
			}
#if DEBUG
			Debug.WriteLine("Hit some edge case. probably should fix this as it always returns false.");
#endif
			return false;
		}

		private JewelryType JewelryTypeSupported(ClitPiercings piercingLocation)
		{
			return SUPPORTED_CLIT_PIERCINGS;
		}

		public bool isPierced => clitPiercings.isPierced;

		public bool wearingJewelry => clitPiercings.wearingJewelry;


		#endregion
		#region Grow/Shrinkable
		bool IGrowable.CanGroPlus()
		{
			return length < MAX_CLIT_SIZE;
		}

		bool IShrinkable.CanReducto()
		{
			return length > minSize;
		}

		float IGrowable.UseGroPlus()
		{
			if (!((IGrowable)this).CanGroPlus())
			{
				return 0;
			}
			float oldLength = length;
			length += 1;
			return length - oldLength;
		}

		float IShrinkable.UseReducto()
		{
			if (!((IShrinkable)this).CanReducto())
			{
				return 0;
			}
			float oldLength = length;
			length /= 1.7f;
			return oldLength - length;
		}


		#endregion
	}

	public sealed partial class ClitData : SimpleData
	{
		public readonly float length;
		public readonly bool isClitCock;
		public readonly bool clitCockActive;
		public readonly int VaginaIndex;

		public readonly ReadOnlyPiercing<ClitPiercings> clitPiercings;

		internal ClitData(Clit source, int currIndex) : base(source?.creatureID ?? throw new ArgumentNullException(nameof(source)))
		{
			length = source.length;
			isClitCock = source.omnibusClit;
			isClitCock = source.omnibusActive;
			VaginaIndex = currIndex;

			clitPiercings = source.clitPiercings.AsReadOnlyData();
		}
	}
}
