//Clit.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 6:03 PM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Tools;
using System.Diagnostics;
using System.Linq;
using CoC.Backend.SaveData;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.Perks;
using System;
using CoC.Backend.Creatures;

namespace CoC.Backend.BodyParts
{

	public enum ClitPiercings { CHRISTINA, HOOD_VERTICAL, HOOD_HORIZONTAL, HOOD_TRIANGLE, CLIT_ITSELF, LARGE_CLIT_1, LARGE_CLIT_2, LARGE_CLIT_3 }

	//note: perks are guarenteed to be valid by the time this is created, so it's post perk init won't be called. 

	public sealed class Clit : SimpleSaveablePart<Clit, ClitData>, IGrowable, IShrinkable
	{
		internal float clitGrowthMultiplier = 1;
		internal float clitShrinkMultiplier = 1;
		internal float minClitSize = MIN_CLIT_SIZE;
		internal float minNewClitSize;

		private float resetSize => Math.Max(minNewClitSize, minClitSize);
		
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
				_length = Utils.Clamp2(value, minSize, MAX_CLIT_SIZE);
			}
		}
		private float _length;

		public bool omnibusClit
		{
			get => _omnibusClit;
			private set
			{
				if (value && !_omnibusClit && length < MIN_CLITCOCK_SIZE)
				{
					_length = MIN_CLITCOCK_SIZE;
				}
				_omnibusClit = value;
			}
		}
		private bool _omnibusClit;

		public readonly Piercing<ClitPiercings> clitPiercings;

		internal Clit(Creature source, VaginaPerkHelper initialPerkData, bool isOmnibusClit = false) : base(source)
		{
			length = initialPerkData.NewClitSize();
			if (isOmnibusClit && MIN_CLITCOCK_SIZE > length)
			{
				length = MIN_CLITCOCK_SIZE;
			}

			omnibusClit = isOmnibusClit;
			clitPiercings = new Piercing<ClitPiercings>(PiercingLocationUnlocked, JewelryTypeSupported);

			minClitSize = initialPerkData.MinClitSize;
			minNewClitSize = initialPerkData.DefaultNewClitSize;
			clitGrowthMultiplier = initialPerkData.ClitGrowthMultiplier;
			clitShrinkMultiplier = initialPerkData.ClitShrinkMultiplier;
		}

		internal Clit(Creature source, VaginaPerkHelper initialPerkData, float clitSize, bool isOmnibusClit = false) : base(source)
		{
			length = initialPerkData.NewClitSize(clitSize);
			if (isOmnibusClit && MIN_CLITCOCK_SIZE > length)
			{
				length = MIN_CLITCOCK_SIZE;
			}

			omnibusClit = isOmnibusClit;
			clitPiercings = new Piercing<ClitPiercings>(PiercingLocationUnlocked, JewelryTypeSupported);

			minClitSize = initialPerkData.MinClitSize;
			minNewClitSize = initialPerkData.DefaultNewClitSize;
			clitGrowthMultiplier = initialPerkData.ClitGrowthMultiplier;
			clitShrinkMultiplier = initialPerkData.ClitShrinkMultiplier;
		}

		public override ClitData AsReadOnlyData()
		{
			return new ClitData(this);
		}

		//public Cock AsCock()
		//{
		//	if (!omnibusClit)
		//	{
		//		return null;
		//	}

		//	if (clitCock == null)
		//	{
		//		clitCock = Cock.GenerateClitCock(this);
		//	}
		//	else
		//	{
		//		clitCock.SetLength(length + 5);
		//	}
		//	return clitCock;
		//}
		//private Cock clitCock = null;
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

		public bool ActivateOmnibusClit()
		{
			if (omnibusClit)
			{
				return false;
			}
			omnibusClit = true;
			return true;
		}

		public bool DeactivateOmnibusClit()
		{
			if (!omnibusClit)
			{
				return false;
			}
			omnibusClit = false;
			return true;
		}

		public float growClit(float amount, bool ignorePerks = false)
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

		public float shrinkClit(float amount, bool ignorePerks = false)
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

		public float SetClitSize(float newSize)
		{
			length = newSize;
			return length;
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

	public sealed class ClitData
	{
		public readonly float length;
		public readonly bool isClitCock;

		internal ClitData(Clit source)
		{
			length = source.length;
			isClitCock = source.omnibusClit;
		}
	}
}
