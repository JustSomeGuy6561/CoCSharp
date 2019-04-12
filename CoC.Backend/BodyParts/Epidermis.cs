//Epidermis.cs
//Description: Epidermis Sub-part. it is used in pther parts to determine their tone, fur color, etc.
//Author: JustSomeGuy
//12/26/2018, 7:58 PM
using CoC.Backend.CoC_Colors;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CoC.Backend.BodyParts
{
	//Epidermis represents the equivalent to skin on all species.

	/* Epidermis is a weird case in that it cannot be alone; it must help make up something else. It is expressly for storage
	 * so it should never be public. Epidermal Data is provided for public access, so if you want people to have access to the data
	 * stored in the epidermis in the frontend, provide a means of obtaining the epidermal data instance. 
	 *   (generally, a property like so: public EpidermalData EpidermisName => EpidermisStorage.GetEpidermalData();)
	 * 
	 * Note that different body parts will use the epidermis differently - be sure to check them on how they work.
	 */


	//feel free to add more of these. i just did these because they were there, and i didn't want to use a string.
	public enum SkinTexture { NONDESCRIPT, SHINY, SOFT, SMOOTH, SEXY, ROUGH, THICK, FRECKLED, SLIMY }
	public enum FurTexture { NONDESCRIPT, SHINY, SOFT, SMOOTH, MANGEY }

	public sealed partial class Epidermis : BehavioralPartBase<EpidermisType>
	{
		public FurColor fur { get; private set; }
		public Tones tone { get; private set; }

		public SkinTexture skinTexture { get; private set; }
		public FurTexture furTexture { get; private set; }

		public bool usesFur => type.usesFur;
		public bool usesTone => type.usesTone;

		public override EpidermisType type { get; protected set; }


		private Epidermis()
		{
			this.type = EpidermisType.EMPTY;
			fur = new FurColor();
			tone = Tones.NOT_APPLICABLE;
			skinTexture = SkinTexture.NONDESCRIPT;
			furTexture = FurTexture.NONDESCRIPT;
		}
		public EpidermalData GetEpidermalData()
		{
			if (type.usesFur)
			{
				return new EpidermalData(type, fur, furTexture);
			}
			else return new EpidermalData(type, tone, skinTexture);

		}
		internal bool Validate(bool correctDataIfInvalid = false)
		{
			EpidermisType epidermis = type;
			FurColor furColor = fur;
			Tones tones = tone;

			bool valid = EpidermisType.Validate(ref epidermis, ref furColor, ref tones, correctDataIfInvalid);
			tone = tones;
			fur = furColor;
			type = epidermis;
			return valid;
		}
		#region Generate

		public static Epidermis GenerateEmpty()
		{
			return new Epidermis();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="epidermisType"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">Thrown if epidermisType is null</exception>
		public static Epidermis GenerateDefaultOfType(EpidermisType epidermisType)
		{
			if (epidermisType == null) throw new ArgumentNullException();

			Epidermis retVal = new Epidermis();
			if (epidermisType is ToneBasedEpidermisType toneType)
			{
				retVal.UpdateEpidermis(toneType, toneType.defaultTone);
			}
			else if (epidermisType is FurBasedEpidermisType furType)
			{
				retVal.UpdateEpidermis(furType, furType.defaultFur);
			}
			return retVal;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="toneType"></param>
		/// <param name="initialTone"></param>
		/// <param name="texture"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"> thrown if toneType is null </exception>
		public static Epidermis Generate(ToneBasedEpidermisType toneType, Tones initialTone, SkinTexture texture = SkinTexture.NONDESCRIPT)
		{
			//null checks
			if (toneType == null) throw new ArgumentNullException(); //can't determine behavior, so throw.
			if (Tones.isNullOrEmpty(initialTone)) initialTone = toneType.defaultTone; //can be null. we can survive this, so don't throw.

			return new Epidermis()
			{
				type = toneType,
				tone = initialTone,
				skinTexture = texture
			};
		}

		public static Epidermis Generate(Epidermis other)
		{
			if (other == null) throw new ArgumentNullException();
			return new Epidermis()
			{
				type = other.type,
				tone = other.tone,
				fur = other.fur,
				furTexture = other.furTexture,
				skinTexture = other.skinTexture
			};
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="furType"></param>
		/// <param name="furColor">the fur color to use. if null or empty, uses type default.</param>
		/// <param name="texture"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">Thrown if furType is null</exception>
		public static Epidermis Generate(FurBasedEpidermisType furType, FurColor furColor, FurTexture texture = FurTexture.NONDESCRIPT)
		{
			//null checks
			if (furType == null) throw new ArgumentNullException(); //we can't survive this, so throw.
			if (FurColor.IsNullOrEmpty(furColor)) furColor = furType.defaultFur; //can be null. we can survive this, though, so don't throw

			Epidermis retVal = new Epidermis()
			{
				type = furType,
				furTexture = texture
			};
			retVal.fur.UpdateFurColor(furColor);
			return retVal;
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
		#region Updates
		public bool UpdateEpidermis(EpidermisType epidermisType, bool resetOther = false)
		{
			if (epidermisType == null || type == epidermisType)
			{
				return false;
			}
			type = epidermisType;
			if (epidermisType is ToneBasedEpidermisType && tone.isEmpty) //can't be null
			{
				tone = ((ToneBasedEpidermisType)type).defaultTone;
				if (resetOther)
				{
					fur.Reset();
				}
			}
			else if (epidermisType is FurBasedEpidermisType && fur.isEmpty) //can't be null
			{
				fur.UpdateFurColor(((FurBasedEpidermisType)type).defaultFur);
				if (resetOther)
				{
					tone = Tones.NOT_APPLICABLE;
				}
			}
			return true;
		}

		public bool UpdateEpidermis(FurBasedEpidermisType furType, FurColor overrideColor, bool resetTone = false)
		{
			return UpdateEpidermis(furType, overrideColor, furTexture, resetTone);
		}
		public bool UpdateEpidermis(ToneBasedEpidermisType toneType, Tones overrideTone, bool resetFur = false)
		{
			return UpdateEpidermis(toneType, overrideTone, skinTexture, resetFur);
		}

		public bool UpdateEpidermis(FurBasedEpidermisType furType, FurTexture texture, bool resetTone = false)
		{
			return UpdateEpidermis(furType, fur, texture, resetTone);
		}
		public bool UpdateEpidermis(ToneBasedEpidermisType toneType, SkinTexture texture, bool resetFur = false)
		{
			return UpdateEpidermis(toneType, tone, texture, resetFur);
		}

		public bool UpdateEpidermis(FurBasedEpidermisType furType, FurColor overrideColor, FurTexture texture, bool resetTone = false)
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
			if (resetTone)
			{
				tone = Tones.NOT_APPLICABLE;
			}
			furTexture = texture;
			return true;
		}
		public bool UpdateEpidermis(ToneBasedEpidermisType toneType, Tones overrideTone, SkinTexture texture, bool resetFur = false)
		{
			if (toneType == null || type == toneType)
			{
				return false;
			}
			type = toneType;
			if (!Tones.isNullOrEmpty(overrideTone)) //can be null.
			{
				tone = overrideTone;
			}
			else if (tone.isEmpty) //can't be null
			{
				tone = toneType.defaultTone;
			}
			if (resetFur)
			{
				fur.Reset();
			}
			skinTexture = texture;
			return true;
		}
		#endregion Updates
		#region Change
		public bool ChangeTone(Tones newTone, bool resetFur = false)
		{
			if (resetFur && !type.usesFur) //don't erase if it's in use
			{
				fur.Reset();
			}
			if (type.toneMutable && tone != newTone && !Tones.isNullOrEmpty(newTone)) //can be null
			{
				tone = newTone;
				return true;
			}
			return false;
		}

		public bool ChangeFur(FurColor furColor, bool resetTone = false)
		{
			if (resetTone && !type.usesTone)
			{
				tone = Tones.NOT_APPLICABLE;
			}
			if (fur != furColor && type.furMutable && !FurColor.IsNullOrEmpty(furColor)) //can be null
			{
				fur.UpdateFurColor(furColor);
				return true;
			}
			return false;
		}

		public bool ChangeTexture(FurTexture newTexture)
		{
			if (furTexture == newTexture) return false;
			furTexture = newTexture;
			return true;
		}

		public bool ChangeTexture(SkinTexture newTexture)
		{
			if (skinTexture == newTexture) return false;
			skinTexture = newTexture;
			return true;
		}
		#endregion
		#region Update Or Change
		//Useful Helpers. Update if different, change if same. I'm not overly fond of the idea as the behavior is not identical in all instances, but
		//considering how often the if/else check would be used this makes more sense. use these only if you are truly doing it - if you know the type is 
		//correct, then just call change.

		//NOTE: while these may not throw themselves, they may call something that throws. this is why i don't like these helpers. 

		public bool UpdateOrChange(FurBasedEpidermisType furType, FurColor overrideColor, bool resetTone = false)
		{
			if (furType != type)
			{
				return UpdateEpidermis(furType, overrideColor, resetTone);
			}
			else return ChangeFur(overrideColor, resetTone);
		}

		public bool UpdateOrChange(ToneBasedEpidermisType toneType, Tones overrideColor, bool resetFur = false)
		{
			if (toneType != type)
			{
				return UpdateEpidermis(toneType, overrideColor, resetFur);
			}
			else return ChangeTone(overrideColor, resetFur);
		}
		public bool UpdateOrChange(FurBasedEpidermisType furType, FurTexture texture, bool resetTone = false)
		{
			if (furType != type)
			{
				return UpdateEpidermis(furType, texture, resetTone);
			}
			else return ChangeFur(furType.defaultFur, resetTone);
		}

		public bool UpdateOrChange(ToneBasedEpidermisType toneType, Tones overrideColor, SkinTexture texture, bool resetFur = false)
		{
			if (toneType != type)
			{
				return UpdateEpidermis(toneType, overrideColor, texture, resetFur);
			}
			else return ChangeTone(overrideColor, resetFur);
		}
		public bool UpdateOrChange(FurBasedEpidermisType furType, FurColor overrideColor, FurTexture texture, bool resetTone = false)
		{
			if (furType != type)
			{
				return UpdateEpidermis(furType, overrideColor, texture, resetTone);
			}
			else return ChangeFur(overrideColor, resetTone);
		}
		#endregion
	}


	//IMMUTABLE
	public abstract partial class EpidermisType : BehaviorBase
	{
		private static int indexMaker = 0;
		private static readonly List<EpidermisType> epidermi = new List<EpidermisType>();
		public abstract bool usesTone { get; }
		public virtual bool usesFur => !usesTone;

		protected readonly bool updateable;
		protected readonly int _index;

		//we can change fur if it's a tone type (just for storage when changing classes mind you)
		public bool furMutable => !usesFur || updateable;
		public bool toneMutable => !usesTone || updateable;

		private protected EpidermisType(SimpleDescriptor desc, bool canChange) : base(desc)
		{
			_index = indexMaker++;
			epidermi.AddAt(this, _index);
			updateable = canChange;
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

		internal static bool Validate(ref EpidermisType epidermisType, ref FurColor fur, ref Tones tone, bool correctInvalidData = false)
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
			valid &= epidermisType.ValidateData(ref fur, ref tone, correctInvalidData);
			return valid;
		}

		private protected abstract bool ValidateData(ref FurColor fur, ref Tones tone, bool correctInvalidData = false);

		public override int index => _index;

		public static readonly ToneBasedEpidermisType SKIN = new ToneBasedEpidermisType(SkinStr, true, Tones.LIGHT);
		public static readonly FurBasedEpidermisType FUR = new FurBasedEpidermisType(FurStr, true, new FurColor(HairFurColors.BLACK));
		public static readonly ToneBasedEpidermisType SCALES = new ToneBasedEpidermisType(ScalesStr, true, Tones.GREEN);
		public static readonly ToneBasedEpidermisType GOO = new ToneBasedEpidermisType(GooStr, true, Tones.DEEP_BLUE);
		public static readonly FurBasedEpidermisType WOOL = new FurBasedEpidermisType(WoolStr, true, new FurColor(HairFurColors.WHITE)); //i'd like to merge this with fur but it's more trouble than it's worth
		public static readonly FurBasedEpidermisType FEATHERS = new FurBasedEpidermisType(FeathersStr, true, new FurColor(HairFurColors.WHITE));
		public static readonly ToneBasedEpidermisType BARK = new ToneBasedEpidermisType(BarkStr, true, Tones.WOODLY_BROWN); //do you want the bark to change colors? idk? maybe make that false.
		public static readonly ToneBasedEpidermisType CARAPACE = new ToneBasedEpidermisType(CarapaceStr, true, Tones.BLACK);
		//cannot be changed by lotion. May convert this to a perk, which affects everything.
		public static readonly ToneBasedEpidermisType RUBBER = new ToneBasedEpidermisType(RubberStr, false, Tones.GRAY); //now its own type. it's simpler this way imo - for now. may become a perk. 
		public static readonly EmptyEpidermisType EMPTY = new EmptyEpidermisType();
	}

	public class FurBasedEpidermisType : EpidermisType
	{
		public FurColor defaultFur => new FurColor(_defaultFur);
		private readonly FurColor _defaultFur;
		internal FurBasedEpidermisType(SimpleDescriptor desc, bool canChange, FurColor defaultColor) : base(desc, canChange)
		{
			_defaultFur = new FurColor(defaultColor);
		}

		public override bool usesTone => false;

		private protected override bool ValidateData(ref FurColor fur, ref Tones tone, bool correctInvalidData = false)
		{
			if (!fur.isEmpty)
			{
				return true;
			}
			else if (correctInvalidData)
			{
				fur = defaultFur;
			}
			return true;
		}
	}

	public class ToneBasedEpidermisType : EpidermisType
	{
		public readonly Tones defaultTone;
		internal ToneBasedEpidermisType(SimpleDescriptor desc, bool canChange, Tones defaultColor) : base(desc, canChange)
		{
			defaultTone = defaultColor;
		}

		public override bool usesTone => true;

		private protected override bool ValidateData(ref FurColor fur, ref Tones tone, bool correctInvalidData = false)
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
		internal EmptyEpidermisType() : base(GlobalStrings.None, false) { }

		public override bool usesTone => false;
		public override bool usesFur => false;

		private protected override bool ValidateData(ref FurColor fur, ref Tones tone, bool correctInvalidData = false)
		{
			return true;
		}

	}

	/// <summary>
	/// </summary>
	public partial class EpidermalData
	{

		public EpidermisType epidermisType { get; private set; }
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
		public EpidermalData(EpidermisType type, FurColor furColor, FurTexture texture)
		{
			epidermisType = type ?? throw new ArgumentNullException();
			if (furColor == null) throw new ArgumentNullException();
			_fur = new FurColor(furColor);
			_tone = Tones.NOT_APPLICABLE;
			_furTexture = texture;
			_skinTexture = SkinTexture.NONDESCRIPT;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <param name="tones"></param>
		/// <param name="texture"></param>
		/// <exception cref="ArgumentNullException">Thrown is type or tones is null</exception>
		public EpidermalData(EpidermisType type, Tones tones, SkinTexture texture)
		{
			epidermisType = type ?? throw new ArgumentNullException();
			_tone = tones ?? throw new ArgumentNullException();
			_fur = new FurColor();
			_skinTexture = texture;
			_furTexture = FurTexture.NONDESCRIPT;
		}

		public EpidermalData()
		{
			this.epidermisType = EpidermisType.EMPTY;
			_fur = new FurColor();
			_tone = Tones.NOT_APPLICABLE;
			_furTexture = FurTexture.NONDESCRIPT;
			_skinTexture = SkinTexture.NONDESCRIPT;
		}


		public bool usesFur => epidermisType.usesFur;
		public bool usesTone => epidermisType.usesTone;

		public FurColor fur => epidermisType.usesFur ? _fur : new FurColor();
		public Tones tone => epidermisType.usesTone ? _tone : Tones.NOT_APPLICABLE;

		public SkinTexture skinTexture => epidermisType.usesTone ? _skinTexture : SkinTexture.NONDESCRIPT;
		public FurTexture furTexture => epidermisType.usesTone ? _furTexture : FurTexture.NONDESCRIPT;

		public string shortDescription()
		{
			return epidermisType.shortDescription();
		}
		public string FullDescription()
		{
			if (epidermisType == EpidermisType.EMPTY) return "";
			else if (epidermisType.usesTone) return fullStr(skinTexture.AsString(), tone, epidermisType.shortDescription);
			else return fullStr(furTexture.AsString(), fur, epidermisType.shortDescription);
		}

		public string descriptionWithColor()
		{
			if (epidermisType == EpidermisType.EMPTY) return "";
			else if (epidermisType.usesTone) return ColoredStr(tone, epidermisType.shortDescription);
			else return ColoredStr(fur, epidermisType.shortDescription);
		}

		public string justTexture()
		{
			if (epidermisType == EpidermisType.EMPTY) return "";
			else if (epidermisType.usesTone) return skinTexture.AsString();
			else return furTexture.AsString();
		}

		public string justColor()
		{
			if (epidermisType == EpidermisType.EMPTY) return "";
			else if (epidermisType.usesTone) return tone.AsString();
			else return fur.AsString();
		}
	}
}
