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

namespace CoC.Backend.BodyParts
{

	public enum ClitPiercings { CHRISTINA, HOOD_VERTICAL, HOOD_HORIZONTAL, HOOD_TRIANGLE, CLIT_ITSELF, LARGE_CLIT_1, LARGE_CLIT_2, LARGE_CLIT_3 }

	public sealed class Clit : SimpleSaveablePart<Clit>, IGrowable, IShrinkable, IBaseStatPerkAware
	{
		private float clitGrowthMultiplier => basePerkData?.Invoke().ClitGrowthMultiplier ?? 1;
		private float clitShrinkMultiplier => basePerkData?.Invoke().ClitShrinkMultiplier ?? 1;
		
		private static readonly ClitPiercings[] requiresFetish = { ClitPiercings.LARGE_CLIT_1, ClitPiercings.LARGE_CLIT_2, ClitPiercings.LARGE_CLIT_3 };
		private const JewelryType SUPPORTED_CLIT_PIERCINGS = JewelryType.BARBELL_STUD | JewelryType.RING | JewelryType.SPECIAL;

		private bool piercingFetish => BackendSessionSave.data.piercingFetishEnabled;

		private PerkStatBonusGetter basePerkData;

		public const float MIN_CLIT_SIZE = 0.25f;
		public const float MIN_CLITCOCK_SIZE = 2f;
		public const float DEFAULT_CLIT_SIZE = 0.25f;
		public const float MAX_CLIT_SIZE = 100f;

		private float minSize
		{
			get
			{
				float currMin = basePerkData?.Invoke().MinClitSize ?? MIN_CLIT_SIZE;
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

		private Clit(float clitSize = MIN_CLIT_SIZE)
		{
			length = clitSize;
			omnibusClit = false;
			clitPiercings = new Piercing<ClitPiercings>(PiercingLocationUnlocked, JewelryTypeSupported);
		}

		public static Clit Generate()
		{
			return new Clit();
		}

		public static Clit GenerateWithLength(float clitLength)
		{
			return new Clit(clitLength);
		}

		public static Clit GenerateOmnibusClit(float clitLength = 2.0f)
		{
			if (clitLength < 2)
			{
				clitLength = 2;
			}
			return new Clit(clitLength)
			{
				omnibusClit = true
			};
		}

		public Cock AsCock()
		{
			if (!omnibusClit)
			{
				return null;
			}

			if (clitCock == null)
			{
				clitCock = Cock.GenerateClitCock(this);
			}
			else
			{
				clitCock.SetLength(length + 5);
			}
			return clitCock;
		}
		private Cock clitCock = null;
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

		void IBaseStatPerkAware.GetBasePerkStats(PerkStatBonusGetter getter)
		{
			basePerkData = getter;
		}

		internal void DoLateInit(BasePerkModifiers statModifiers, bool isInit)
		{
			if (isInit)
			{
				float minLength = statModifiers.MinNewClitSize;
				if (length < minLength)
				{
					length = minLength;
				}
			}
			else
			{
				length += statModifiers.NewClitSizeDelta;
			}
		}

		private IBaseStatPerkAware perkAware => this;

		internal void GetBasePerkStats(PerkStatBonusGetter getter)
		{
			perkAware.GetBasePerkStats(getter);
		}
	}
}
