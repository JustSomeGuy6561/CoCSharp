//Epidermis.cs
//Description: Epidermis Sub-part. it is used in pther parts to determine their tone, fur color, etc.
//Author: JustSomeGuy
//12/26/2018, 7:58 PM
using CoC.Backend.CoC_Colors;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CoCBackendUnitTests")]
[assembly: InternalsVisibleTo("Testing")]
namespace CoC.Backend.BodyParts
{
	//Epidermis represents the equivalent to skin on all species.

	/* Epidermis is a weird case in that it cannot be alone; it must help make up something else. It is expressly for storage
	 * so it should never be public. Epidermal Data is provided for public access, so if you want people to have access to the data
	 * stored in the epidermis in the frontend, provide a means of obtaining the epidermal data instance. 
	 *   (generally, a property like so: public ReadOnlyEpidermis EpidermisName => EpidermisStorage.GetReadOnlyEpidermis();)
	 * 
	 * Note that different body parts will use the epidermis differently - be sure to check them on how they work.
	 */

	//NOTE: Epidermis is mutually exclusive - either it stores a fur color and furTexture, or the tone and skinTexture, not both.
	//some body parts have a primary and secondary epidermis strictly for this reason (i suppose more is also possibe). It gets awful complicated awful fast if you store both and
	//have conditionals and such - belive me, I tried.


	//feel free to add more of these. i just did these because they were there, and i didn't want to use a string.
	public enum SkinTexture { NONDESCRIPT, SHINY, SOFT, SMOOTH, SEXY, ROUGH, THICK, FRECKLED, SLIMY }
	public enum FurTexture { NONDESCRIPT, SHINY, SOFT, SMOOTH, FLUFFY, MANGEY, THICK }

	public sealed partial class Epidermis : BehavioralPartBase<Epidermis, EpidermisType, EpidermalData>
	{
		//public FurColor fur { get; private set; }
		public readonly FurColor fur;

		public Tones tone { get; private set; }

		public SkinTexture skinTexture
		{
			get => _skinTexture;
			private set
			{
				if (Enum.IsDefined(typeof(SkinTexture), value))
				{
					_skinTexture = value;
				}
			}
		}
		private SkinTexture _skinTexture;

		public FurTexture furTexture
		{
			get => _furTexture;
			private set
			{
				if (Enum.IsDefined(typeof(FurTexture), value))
				{
					_furTexture = value;
				}
			}
		}
		private FurTexture _furTexture;

		public bool usesFur => type.usesFur;
		public bool usesTone => type.usesTone;

		public bool mutable => type.updateable;

		public bool toneMutable => usesTone && mutable;
		public bool furMutable => usesFur && mutable;

		public override EpidermisType type { get; protected set; }

		//private bool resetOther;


		public Epidermis()
		{
			this.type = EpidermisType.EMPTY;
			fur = new FurColor();
			tone = Tones.NOT_APPLICABLE;
			skinTexture = SkinTexture.NONDESCRIPT;
			furTexture = FurTexture.NONDESCRIPT;

		}

		/// <summary>
		/// Constructor for Epidermis, but this time with a type defined.
		/// </summary>
		/// <param name="epidermisType"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">Thrown if epidermisType is null</exception>
		public Epidermis(EpidermisType epidermisType)
		{
			type = epidermisType ?? throw new ArgumentNullException();

			tone = Tones.NOT_APPLICABLE;
			fur = null;
			skinTexture = SkinTexture.NONDESCRIPT;
			furTexture = FurTexture.NONDESCRIPT;

			if (epidermisType is ToneBasedEpidermisType toneType)
			{
				tone = toneType.defaultTone;
			}
			else if (epidermisType is FurBasedEpidermisType furType)
			{
				fur = new FurColor(furType.defaultFur);
			}

			if (fur == null)
			{
				fur = new FurColor();
			}
		}

		public bool isEmpty => type == EpidermisType.EMPTY;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="furType"></param>
		/// <param name="furColor">the fur color to use. if null or empty, uses type default.</param>
		/// <param name="texture"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">Thrown if furType is null</exception>
		public Epidermis(FurBasedEpidermisType furType, FurColor furColor, FurTexture texture = FurTexture.NONDESCRIPT) : this(furType)
		{
			if (FurColor.IsNullOrEmpty(furColor)) furColor = furType.defaultFur; //can be null. we can survive this, though, so don't throw
			fur.UpdateFurColor(furColor);
			furTexture = texture;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="toneType"></param>
		/// <param name="initialTone"></param>
		/// <param name="texture"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"> thrown if toneType is null </exception>
		public Epidermis(ToneBasedEpidermisType toneType, Tones initialTone, SkinTexture texture = SkinTexture.NONDESCRIPT) : this(toneType)
		{
			if (Tones.IsNullOrEmpty(initialTone)) initialTone = toneType.defaultTone; //can be null. we can survive this, so don't throw.
			tone = initialTone;
			skinTexture = texture;
		}

		public Epidermis(Epidermis other)
		{
			if (other == null) throw new ArgumentNullException();

			type = other.type;
			tone = other.tone;
			fur = other.fur;
			furTexture = other.furTexture;
			skinTexture = other.skinTexture;
		}


		#region Updates
		public bool UpdateEpidermis(EpidermisType epidermisType)
		{
			if (epidermisType == null || type == epidermisType)
			{
				return false;
			}
			type = epidermisType;
			if (epidermisType is ToneBasedEpidermisType && tone.isEmpty) //can't be null
			{
				tone = ((ToneBasedEpidermisType)type).defaultTone;
				fur.Reset();
			}
			else if (epidermisType is FurBasedEpidermisType && fur.isEmpty) //can't be null
			{
				fur.UpdateFurColor(((FurBasedEpidermisType)type).defaultFur);
				tone = Tones.NOT_APPLICABLE;
			}
			return true;
		}

		public bool UpdateEpidermis(FurBasedEpidermisType furType, FurColor overrideColor)
		{
			return UpdateEpidermis(furType, overrideColor, furTexture);
		}
		public bool UpdateEpidermis(ToneBasedEpidermisType toneType, Tones overrideTone)
		{
			return UpdateEpidermis(toneType, overrideTone, skinTexture);
		}

		public bool UpdateEpidermis(FurBasedEpidermisType furType, FurTexture texture)
		{
			return UpdateEpidermis(furType, fur, texture);
		}
		public bool UpdateEpidermis(ToneBasedEpidermisType toneType, SkinTexture texture)
		{
			return UpdateEpidermis(toneType, tone, texture);
		}

		public bool UpdateEpidermis(FurBasedEpidermisType furType, FurColor overrideColor, FurTexture texture)
		{
			if (furType == null || type == furType)
			{
				return false;
			}
			type = furType;
			if (!FurColor.IsNullOrEmpty(overrideColor)) //can be null
			{
				fur.UpdateFurColor(overrideColor);
			}
			else if (fur.isEmpty) //can't be null
			{
				fur.UpdateFurColor(furType.defaultFur);
			}
			tone = Tones.NOT_APPLICABLE;

			furTexture = texture;
			return true;
		}
		public bool UpdateEpidermis(ToneBasedEpidermisType toneType, Tones overrideTone, SkinTexture texture)
		{
			if (toneType == null || type == toneType)
			{
				return false;
			}
			type = toneType;
			if (!Tones.IsNullOrEmpty(overrideTone)) //can be null.
			{
				tone = overrideTone;
			}
			else if (tone.isEmpty) //can't be null
			{
				tone = toneType.defaultTone;
			}

			fur.Reset();
			skinTexture = texture;
			return true;
		}

		internal void copyFrom(Epidermis other)
		{
			if (other == null) throw new ArgumentNullException();

			type = other.type;
			tone = other.tone;
			fur.UpdateFurColor(other.fur);
			furTexture = other.furTexture;
			skinTexture = other.skinTexture;
		}

		#endregion Updates
		#region Change
		public bool ChangeTone(Tones newTone)
		{
			if (type.updateable && tone != newTone && !Tones.IsNullOrEmpty(newTone)) //can be null
			{
				tone = newTone;
				return true;
			}
			return false;
		}

		public bool ChangeToneAndTexture(Tones newTone, SkinTexture skinTexture)
		{
			return ChangeTone(newTone) | ChangeTexture(skinTexture); //single bar - run both regardless of if first returned true.
		}

		public bool ChangeFur(FurColor furColor)
		{
			if (!fur.Equals(furColor) && type.updateable && !FurColor.IsNullOrEmpty(furColor)) //can be null
			{
				fur.UpdateFurColor(furColor);
				return true;
			}
			return false;
		}

		public bool ChangeFur(HairFurColors color)
		{
			if (!fur.IsIdenticalTo(color) && type.updateable && !HairFurColors.IsNullOrEmpty(color)) //can be null
			{
				fur.UpdateFurColor(color);
				return true;
			}
			return false;
		}

		public bool ChangeFurAndTexture(FurColor furColor, FurTexture texture)
		{
			return ChangeFur(furColor) | ChangeTexture(texture);
		}

		public bool ChangeFurAndTexture(HairFurColors colors, FurTexture texture)
		{
			return ChangeFur(colors) | ChangeTexture(texture);
		}

		public bool ChangeTexture(FurTexture newTexture)
		{
			if (furTexture == newTexture || !usesFur) return false;
			furTexture = newTexture;
			return true;
		}

		public bool ChangeTexture(SkinTexture newTexture)
		{
			if (skinTexture == newTexture || !usesTone) return false;
			skinTexture = newTexture;
			return true;
		}
		#endregion
		#region Update Or Change
		//Useful Helpers. Update if different, change if same. I'm not overly fond of the idea as the behavior is not identical in all instances, but
		//considering how often the if/else check would be used this makes more sense. use these only if you are truly doing it - if you know the type is 
		//correct, then just call change.

		//NOTE: while these may not throw themselves, they may call something that throws. this is why i don't like these helpers. 

		public bool UpdateOrChange(FurBasedEpidermisType furType, FurColor overrideColor)
		{
			if (furType != type)
			{
				return UpdateEpidermis(furType, overrideColor);
			}
			else return ChangeFur(overrideColor);
		}

		public bool UpdateOrChange(ToneBasedEpidermisType toneType, Tones overrideColor)
		{
			if (toneType != type)
			{
				return UpdateEpidermis(toneType, overrideColor);
			}
			else return ChangeTone(overrideColor);
		}
		public bool UpdateOrChange(FurBasedEpidermisType furType, FurTexture texture)
		{
			if (furType != type)
			{
				return UpdateEpidermis(furType, texture);
			}
			else return ChangeFur(furType.defaultFur);
		}

		public bool UpdateOrChange(ToneBasedEpidermisType toneType, Tones overrideColor, SkinTexture texture)
		{
			if (toneType != type)
			{
				return UpdateEpidermis(toneType, overrideColor, texture);
			}
			else return ChangeToneAndTexture(overrideColor, texture);
		}
		public bool UpdateOrChange(FurBasedEpidermisType furType, FurColor overrideColor, FurTexture texture)
		{
			if (furType != type)
			{
				return UpdateEpidermis(furType, overrideColor, texture);
			}
			else return ChangeFurAndTexture(overrideColor, texture);
		}
		#endregion

		public void Reset()
		{
			type = EpidermisType.EMPTY;
			fur.Reset();
			tone = Tones.NOT_APPLICABLE;
			furTexture = FurTexture.NONDESCRIPT;
			skinTexture = SkinTexture.NONDESCRIPT;
		}

		internal bool Validate(bool correctInvalidData)
		{
			EpidermisType epidermis = type;
			Tones tones = tone;

			bool valid = EpidermisType.Validate(ref epidermis, fur, ref tones, correctInvalidData);
			tone = tones;
			type = epidermis;
			return valid;
		}

		public override EpidermalData AsReadOnlyReference()
		{
			if (type.usesFur) return new EpidermalData((FurBasedEpidermisType)type, fur, furTexture);
			else if (type.usesTone) return new EpidermalData((ToneBasedEpidermisType)type, tone, skinTexture);
			else return new EpidermalData();
		}


		public string LongDescription()
		{
			if (type == EpidermisType.EMPTY) return "";
			else if (type.usesTone) return fullStr(skinTexture.AsString(), tone, type.shortDescription);
			else return fullStr(furTexture.AsString(), fur, type.shortDescription);
		}

		public string DescriptionWithColor()
		{
			if (type == EpidermisType.EMPTY) return "";
			else if (type.usesTone) return ColoredStr(tone, type.shortDescription);
			else return ColoredStr(fur, type.shortDescription);
		}

		public string JustTexture()
		{
			if (type == EpidermisType.EMPTY) return "";
			else if (type.usesTone) return skinTexture.AsString();
			else return furTexture.AsString();
		}

		public string JustColor()
		{
			if (type == EpidermisType.EMPTY) return "";
			else if (type.usesTone) return tone.AsString();
			else return fur.AsString();
		}

		public string ShortAdjectiveDescription()
		{
			return type.adjectiveStr();
		}

		public string LongAdjectiveDescription()
		{
			if (type == EpidermisType.EMPTY) return "";
			else if (type.usesTone) return fullStr(skinTexture.AsString(), tone, type.adjectiveStr, true);
			else return fullStr(furTexture.AsString(), fur, type.adjectiveStr, true);
		}

		public string AdjectiveWithColor()
		{
			if (type == EpidermisType.EMPTY) return "";
			else if (type.usesTone) return ColoredStr(tone, type.adjectiveStr, true);
			else return ColoredStr(fur, type.adjectiveStr, true);
		}
	}


	//IMMUTABLE
	public abstract partial class EpidermisType : BehaviorBase
	{
		private static int indexMaker = 0;
		private static readonly List<EpidermisType> epidermi = new List<EpidermisType>();
		public static readonly ReadOnlyCollection<EpidermisType> availableTypes = new ReadOnlyCollection<EpidermisType>(epidermi);
		public abstract bool usesTone { get; }
		public virtual bool usesFur => !usesTone;

		public readonly bool updateable;
		protected readonly int _index;

		public readonly SimpleDescriptor adjectiveStr;

		//we can change fur if it's a tone type (just for storage when changing classes mind you)
		//public bool furMutable => usesFur && updateable;
		//public bool toneMutable => usesTone && updateable;

		private protected EpidermisType(SimpleDescriptor nounDescription, SimpleDescriptor adjectiveDescription, bool canChange) : base(nounDescription)
		{
			_index = indexMaker++;
			epidermi.AddAt(this, _index);
			updateable = canChange;
			adjectiveStr = adjectiveDescription ?? throw new ArgumentNullException(nameof(adjectiveDescription));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if index is out of range</exception>
		/// <exception cref="ArgumentException">Thrown if index points to a non-existant object</exception> 
		internal static EpidermisType Deserialize(int index)
		{
			if (index < 0 || index >= epidermi.Count)
			{
				throw new System.ArgumentOutOfRangeException();
			}
			else
			{
				EpidermisType epidermis = epidermi[index];
				if (epidermis != null)
				{
					return epidermis;
				}
				else
				{
					throw new System.ArgumentException("index for arm type points to an object that does not exist. this may be due to obsolete code");
				}
			}
		}

		internal static bool Validate(ref EpidermisType epidermisType, FurColor fur, ref Tones tone, bool correctInvalidData)
		{
			bool valid = true;
			if (!epidermi.Contains(epidermisType))
			{
				if (!correctInvalidData)
				{
					return false;
				}
				epidermisType = SKIN;
				valid = false;
			}
			valid &= epidermisType._ValidateWrapper_(fur, ref tone, correctInvalidData);
			return valid;
		}

		private protected abstract bool _ValidateWrapper_(FurColor fur, ref Tones tone, bool correctInvalidData);

		public override int index => _index;

		public static readonly ToneBasedEpidermisType SKIN = new ToneBasedEpidermisType(SkinStr, SkinAdjectiveStr, true, Tones.LIGHT);
		public static readonly FurBasedEpidermisType FUR = new FurBasedEpidermisType(FurStr, FurAdjectiveStr, true, new FurColor(HairFurColors.BLACK));
		public static readonly ToneBasedEpidermisType SCALES = new ToneBasedEpidermisType(ScalesStr, ScalesAdjectiveStr, true, Tones.GREEN);
		public static readonly ToneBasedEpidermisType GOO = new ToneBasedEpidermisType(GooStr, GooAdjectiveStr, true, Tones.DEEP_BLUE);
		public static readonly FurBasedEpidermisType WOOL = new FurBasedEpidermisType(WoolStr, WoolAdjectiveStr, true, new FurColor(HairFurColors.WHITE)); //i'd like to merge this with fur but it's more trouble than it's worth
		public static readonly FurBasedEpidermisType FEATHERS = new FurBasedEpidermisType(FeathersStr, FeathersAdjectiveStr, true, new FurColor(HairFurColors.WHITE));
		public static readonly ToneBasedEpidermisType BARK = new ToneBasedEpidermisType(BarkStr, BarkAdjectiveStr, true, Tones.WOODLY_BROWN); //do you want the bark to change colors? idk? maybe make that false.
		public static readonly ToneBasedEpidermisType CARAPACE = new ToneBasedEpidermisType(CarapaceStr, CarapaceAdjectiveStr, true, Tones.BLACK);
		//cannot be changed by lotion. May convert this to a perk, which affects everything.
		public static readonly ToneBasedEpidermisType RUBBER = new ToneBasedEpidermisType(RubberStr, RubberAdjectiveStr, false, Tones.GRAY); //now its own type. it's simpler this way imo - for now. may become a perk. 
		public static readonly EmptyEpidermisType EMPTY = new EmptyEpidermisType();
	}

	public class FurBasedEpidermisType : EpidermisType
	{
		public FurColor defaultFur => new FurColor(_defaultFur);
		private readonly FurColor _defaultFur;
		internal FurBasedEpidermisType(SimpleDescriptor nounDescription, SimpleDescriptor adjectiveDescription, bool canChange, FurColor defaultColor) :
			base(nounDescription, adjectiveDescription, canChange)
		{
			_defaultFur = new FurColor(defaultColor);
		}

		public override bool usesTone => false;

		private protected override bool _ValidateWrapper_(FurColor fur, ref Tones tone, bool correctInvalidData)
		{
			if (!fur.isEmpty)
			{
				return true;
			}
			else if (correctInvalidData)
			{
				fur.UpdateFurColor(defaultFur);
			}
			return true;
		}
	}

	public class ToneBasedEpidermisType : EpidermisType
	{
		public readonly Tones defaultTone;
		internal ToneBasedEpidermisType(SimpleDescriptor nounDescription, SimpleDescriptor adjectiveDescription, bool canChange, Tones defaultColor) : 
			base(nounDescription, adjectiveDescription, canChange)
		{
			defaultTone = defaultColor;
		}

		public override bool usesTone => true;

		private protected override bool _ValidateWrapper_(FurColor fur, ref Tones tone, bool correctInvalidData)
		{
			if (!tone.isEmpty)
			{
				return true;
			}
			else if (correctInvalidData)
			{
				tone = defaultTone;
			}
			return false;
		}
	}

	public class EmptyEpidermisType : EpidermisType
	{
		internal EmptyEpidermisType() : base(GlobalStrings.None, GlobalStrings.None, false) { }

		public override bool usesTone => false;
		public override bool usesFur => false;

		private protected override bool _ValidateWrapper_(FurColor fur, ref Tones tone, bool correctInvalidData)
		{
			return true;
		}

	}

	/// <summary>
	/// </summary>
	public partial class EpidermalData : BehavioralWrapperBase<EpidermalData, Epidermis, EpidermisType>, IEquatable<EpidermalData>
	{

		private readonly FurColor _fur; //NOT NULL EVER!
		private readonly Tones _tone; //NOT NULL EVER!

		private readonly SkinTexture _skinTexture;
		private readonly FurTexture _furTexture;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <param name="furColor"></param>
		/// <param name="texture"></param>
		/// <exception cref="ArgumentNullException">Thrown if type or tones is null</exception>
		/// <exception cref="ArgumentException">Thrown if fur color is empty</exception>
		internal EpidermalData(FurBasedEpidermisType type, FurColor furColor, FurTexture texture) : base(new Epidermis(type, furColor, texture))
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="toneType"></param>
		/// <param name="tones"></param>
		/// <param name="texture"></param>
		/// <exception cref="ArgumentNullException">Thrown is type or tones is null</exception>
		/// <exception cref="ArgumentException">Thrown if tones is empty</exception>
		internal EpidermalData(ToneBasedEpidermisType toneType, Tones tones, SkinTexture texture) : base(new Epidermis(toneType, tones, texture))
		{}

		internal EpidermalData() : base(new Epidermis())
		{
			_fur = new FurColor();
			_tone = Tones.NOT_APPLICABLE;
			_furTexture = FurTexture.NONDESCRIPT;
			_skinTexture = SkinTexture.NONDESCRIPT;
		}


		public bool usesFur => type.usesFur;
		public bool usesTone => type.usesTone;

		public bool isEmpty => type == EpidermisType.EMPTY;

		public FurColor fur => type.usesFur ? _fur : new FurColor();
		public Tones tone => type.usesTone ? _tone : Tones.NOT_APPLICABLE;

		public FurTexture furTexture => type.usesFur ? _furTexture : FurTexture.NONDESCRIPT;
		public SkinTexture skinTexture => type.usesTone ? _skinTexture : SkinTexture.NONDESCRIPT;

		public string LongDescription() => sourceData.LongDescription();

		public string DescriptionWithColor() => sourceData.DescriptionWithColor();

		public string JustTexture() => sourceData.JustTexture();

		public string JustColor() => sourceData.JustColor();

		public string ShortAdjectiveDescription() => sourceData.ShortAdjectiveDescription();

		public string LongAdjectiveDescription() => sourceData.LongAdjectiveDescription();

		public string AdjectiveWithColor() => sourceData.AdjectiveWithColor();
		

		public bool Equals(EpidermalData other)
		{
			return ReferenceEquals(this, other) || (!(other is null) && type == other.type && fur.Equals(other.fur) && furTexture == other.furTexture &&
				tone == other.tone && skinTexture == other.skinTexture);
		}
	}
}
