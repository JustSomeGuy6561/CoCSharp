//Clit.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 6:03 PM
using CoC.BodyParts.SpecialInteraction;
using CoC.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CoC.BodyParts
{


	public enum ClitPiercing { CHRISTINA, HOOD_VERTICAL, HOOD_HORIZONTAL, HOOD_TRIANGLE, CLIT_ITSELF, LARGE_CLIT_1, LARGE_CLIT_2, LARGE_CLIT_3 }

	public class Clit : SimplePiercing<ClitPiercing>, IGrowShrinkable
	{
		protected readonly ClitPiercing[] requiresFetish = { ClitPiercing.LARGE_CLIT_1, ClitPiercing.LARGE_CLIT_2, ClitPiercing.LARGE_CLIT_3 };

		public const float MIN_CLIT_SIZE = 0.25f;
		public const float DEFAULT_CLIT_SIZE = 0.25f;
		public const float MAX_CLIT_SIZE = 100f;

		public float length {
			get => _length;
			protected set
			{
				Utils.Clamp(ref value, MIN_CLIT_SIZE, MAX_CLIT_SIZE);
				_length = value;
			}
		}
		private float _length;

		public bool omnibusClit { get; protected set; }

		protected Clit(PiercingFlags flags, float clitSize = DEFAULT_CLIT_SIZE) : base(flags)
		{
			length = clitSize;
			omnibusClit = false;
		}

		public static Clit Generate(PiercingFlags flags)
		{
			return new Clit(flags);
		}

		public static Clit GenerateOmnibusClit(PiercingFlags flags)
		{
			return new Clit(flags, 5.0f)
			{
				omnibusClit = true
			};
		}

		protected override bool PiercingLocationUnlocked(ClitPiercing piercingLocation)
		{

			if (!requiresFetish.Contains(piercingLocation))
			{
				return true;
			}
			else if (!piercingFlags.piercingFetishEnabled)
			{
				return false;
			}
			else if (piercingLocation == ClitPiercing.LARGE_CLIT_1)
			{
				return length >= 3;
			}
			else if (piercingLocation == ClitPiercing.LARGE_CLIT_2)
			{
				return length >= 5;
			}
			else if (piercingLocation == ClitPiercing.LARGE_CLIT_3)
			{
				return length >= 7;
			}
			#if DEBUG
			Debug.WriteLine("Hit some edge case. probably should fix this as it always returns false.");
			#endif
			return false;
		}

		public Cock AsCock()
		{
			if (!omnibusClit)
			{
				return null;
			}
			clitCock.UpdateSize(length + 4);
			return clitCock;
		}
		private Cock clitCock = Cock.GenerateClitCock();
		public void Restore()
		{
			length = MIN_CLIT_SIZE;
			omnibusClit = false;
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

		public float growClit(float amount)
		{
			if (length >= MAX_CLIT_SIZE || amount <= 0)
			{
				return 0;
			}
			float oldLength = length;
			length += amount;
			return length - oldLength;
		}

		public float shrinkClit(float amount)
		{
			if (length <= MIN_CLIT_SIZE || amount <= 0)
			{
				return 0;
			}
			float oldLength = length;
			length -= amount;
			return oldLength - length;
		}

		#region Grow/Shrinkable
		public bool CanGrowPlus()
		{
			return length < MAX_CLIT_SIZE;
		}

		public bool CanReducto()
		{
			return length > MIN_CLIT_SIZE;
		}

		public float UseGroPlus()
		{
			if (!CanGrowPlus())
			{
				return 0;
			}
			float oldLength = length;
			length += 1;
			return length - oldLength;
		}

		public float UseReducto()
		{
			if (!CanReducto())
			{
				return 0;
			}
			float oldLength = length;
			length /= 1.7f;
			return oldLength - length;
		}
		#endregion
	}
}
