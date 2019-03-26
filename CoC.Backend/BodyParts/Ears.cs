﻿//Ears.cs
//Description:
//Author: JustSomeGuy
//12/27/2018, 12:22 AM

using CoC.Backend.CoC_Colors;
using CoC.Backend.Races;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CoC.Backend.BodyParts
{
	/*
	 * Here's to looking half of this shit up. Following your ear from bottom up around the edge:
	 * LOBE, AURICLE, HELIX, ANTI_HELIX.
	 * LOBE: Lowest part of ear. AURICLE: Thin part of the outside of the ear around the middle
	 * HELIX: the thin upper outside part of your ear. ANTI-HELIX: the thin upper inside part of your ear.
	 * I had more that i looked up but i can't realisticly make them part of anything not human.
	 * I am aware everyone's ears are different and you may have tiny lobes and long-ass helix bits
	 * (which i suppose is also true for elfin ears); i did what i could, sue me. 
	 * Also, each ear has a max of 14 values, as 14 is our max for one screen. right now, I use 13.
	 */
	public enum EarPiercings
	{
		LEFT_LOBE_1, LEFT_LOBE_2, LEFT_LOBE_3, LEFT_UPPER_LOBE,
		LEFT_AURICAL_1, LEFT_AURICAL_2, LEFT_AURICAL_3, LEFT_AURICAL_4,
		LEFT_HELIX_1, LEFT_HELIX_2, LEFT_HELIX_3, LEFT_HELIX_4,
		LEFT_ANTI_HELIX,
		RIGHT_LOBE_1, RIGHT_LOBE_2, RIGHT_LOBE_3, RIGHT_UPPER_LOBE,
		RIGHT_AURICAL_1, RIGHT_AURICAL_2, RIGHT_AURICAL_3, RIGHT_AURICAL_4,
		RIGHT_HELIX_1, RIGHT_HELIX_2, RIGHT_HELIX_3, RIGHT_HELIX_4,
		RIGHT_ANTI_HELIX
	}

	[DataContract]
	public class Ears : PiercableBodyPart<Ears, EarType, EarPiercings>
	{
		private readonly FurColor earFur = new FurColor();
		protected Ears()
		{
			type = EarType.HUMAN;
		}

		public override EarType type { get; protected set; }

		public override bool isDefault => type == EarType.HUMAN;

		protected override bool PiercingLocationUnlocked(EarPiercings piercingLocation)
		{
			return true;
		}

		internal static Ears GenerateDefault()
		{
			return new Ears();
		}
		internal static Ears GenerateDefaultOfType(EarType earType)
		{
			return new Ears() { type = earType };
		}

		internal override bool Restore()
		{
			if (type == EarType.HUMAN)
			{
				return false;
			}
			type = EarType.HUMAN;
			return true;
		}

		internal bool UpdateEars(EarType earType)
		{
			if (type == earType)
			{
				return false;
			}
			type = earType;
			return true;
		}

		internal bool UpdateFurEars(FurEarType furEar, FurColor primary, HairFurColors fallback)
		{
			if (type == furEar)
			{
				return false;
			}
			type = furEar;
			type.ParseFurColor(earFur, primary, fallback);
			return true;
		}

		public void ChangeFurColor(FurColor primary, HairFurColors fallback)
		{
			type.ParseFurColor(earFur, primary, fallback);
		}

		internal override Type currentSaveVersion => typeof(EarSurrogateVersion1);

		internal override Type[] saveVersions => new Type[] { typeof(EarSurrogateVersion1) };

		internal override BodyPartSurrogate<Ears, EarType> ToCurrentSave()
		{
			return new EarSurrogateVersion1()
			{
				earType = index,
				earPiercings = serializePiercings()
			};
		}

		internal Ears(EarSurrogateVersion1 surrogate)
		{
			type = EarType.Deserialize(surrogate.earType);
			deserializePiercings(surrogate.earPiercings);
		}
	}

	public partial class EarType : PiercableBodyPartBehavior<EarType, Ears, EarPiercings>
	{
		private static int indexMaker = 0;
		private static readonly List<EarType> ears = new List<EarType>();
		private readonly int _index;

		protected EarType(SimpleDescriptor shortDesc, DescriptorWithArg<Ears> fullDesc, TypeAndPlayerDelegate<Ears> playerDesc,
			ChangeType<Ears> transform, RestoreType<Ears> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			ears.AddAt(this, index);
		}

		public virtual void ParseFurColor(FurColor current, FurColor primary, HairFurColors fallback)
		{
			current.Reset();
		}

		internal static EarType Deserialize(int index)
		{
			if (index < 0 || index >= ears.Count)
			{
				throw new System.ArgumentException("index for back type deserialize out of range");
			}
			else
			{
				EarType ear = ears[index];
				if (ear != null)
				{
					return ear;
				}
				else
				{
					throw new System.ArgumentException("index for arm type points to an object that does not exist. this may be due to obsolete code");
				}
			}
		}

		public override int index => _index;

		public static readonly EarType HUMAN = new EarType(HumanDescStr, HumanFullDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly EarType HORSE = new EarType(HorseDescStr, HorseFullDesc, HorsePlayerStr, HorseTransformStr, HorseRestoreStr);
		public static readonly EarType DOG = new EarType(DogDescStr, DogFullDesc, DogPlayerStr, DogTransformStr, DogRestoreStr);
		public static readonly EarType COW = new EarType(CowDescStr, CowFullDesc, CowPlayerStr, CowTransformStr, CowRestoreStr);
		public static readonly EarType ELFIN = new EarType(ElfinDescStr, ElfinFullDesc, ElfinPlayerStr, ElfinTransformStr, ElfinRestoreStr);
		public static readonly EarType CAT = new EarType(CatDescStr, CatFullDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
		public static readonly EarType LIZARD = new EarType(LizardDescStr, LizardFullDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static readonly EarType BUNNY = new EarType(BunnyDescStr, BunnyFullDesc, BunnyPlayerStr, BunnyTransformStr, BunnyRestoreStr);
		public static readonly EarType KANGAROO = new EarType(KangarooDescStr, KangarooFullDesc, KangarooPlayerStr, KangarooTransformStr, KangarooRestoreStr);
		public static readonly EarType FOX = new EarType(FoxDescStr, FoxFullDesc, FoxPlayerStr, FoxTransformStr, FoxRestoreStr);
		public static readonly EarType DRAGON = new EarType(DragonDescStr, DragonFullDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr);
		public static readonly EarType RACCOON = new EarType(RaccoonDescStr, RaccoonFullDesc, RaccoonPlayerStr, RaccoonTransformStr, RaccoonRestoreStr);
		public static readonly EarType MOUSE = new EarType(MouseDescStr, MouseFullDesc, MousePlayerStr, MouseTransformStr, MouseRestoreStr);
		public static readonly FurEarType FERRET = new FurEarType(Species.FERRET.defaultUnderFur, FerretDescStr, FerretFullDesc, FerretPlayerStr, FerretTransformStr, FerretRestoreStr);
		public static readonly EarType PIG = new EarType(PigDescStr, PigFullDesc, PigPlayerStr, PigTransformStr, PigRestoreStr);
		public static readonly EarType RHINO = new EarType(RhinoDescStr, RhinoFullDesc, RhinoPlayerStr, RhinoTransformStr, RhinoRestoreStr);
		public static readonly EarType ECHIDA = new EarType(EchidnaDescStr, EchidnaFullDesc, EchidnaPlayerStr, EchidnaTransformStr, EchidnaRestoreStr);
		public static readonly EarType DEER = new EarType(DeerDescStr, DeerFullDesc, DeerPlayerStr, DeerTransformStr, DeerRestoreStr);
		public static readonly EarType WOLF = new EarType(WolfDescStr, WolfFullDesc, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static readonly EarType SHEEP = new EarType(SheepDescStr, SheepFullDesc, SheepPlayerStr, SheepTransformStr, SheepRestoreStr);
		public static readonly EarType IMP = new EarType(ImpDescStr, ImpFullDesc, ImpPlayerStr, ImpTransformStr, ImpRestoreStr);
		public static readonly EarType COCKATRICE = new EarType(CockatriceDescStr, CockatriceFullDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr);
		public static readonly EarType RED_PANDA = new EarType(RedPandaDescStr, RedPandaFullDesc, RedPandaPlayerStr, RedPandaTransformStr, RedPandaRestoreStr);


	}

	public class FurEarType : EarType
	{
		public readonly FurColor defaultFur;
		public FurEarType(FurColor defaultColor, SimpleDescriptor shortDesc, DescriptorWithArg<Ears> fullDesc, TypeAndPlayerDelegate<Ears> playerDesc,
			ChangeType<Ears> transform, RestoreType<Ears> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultFur = defaultColor;
		}

		public override void ParseFurColor(FurColor current, FurColor primary, HairFurColors fallback)
		{
			FurColor color = defaultFur;
			if (!primary.isNoFur())
			{
				color = primary;
			}
			else if (fallback != HairFurColors.NO_HAIR_FUR)
			{
				color = new FurColor(fallback);
			}
			current.UpdateFurColor(color);
		}
	}

	[DataContract]
	public sealed class EarSurrogateVersion1 : BodyPartSurrogate<Ears, EarType>
	{
		[DataMember]
		public int earType;
		[DataMember]
		public bool[] earPiercings;
		internal override Ears ToBodyPart()
		{
			return new Ears(this);
		}
	}
}
