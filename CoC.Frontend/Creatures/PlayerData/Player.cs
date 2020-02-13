using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using CoC.Frontend.Perks;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Creatures.PlayerData
{
	public sealed class Player : PlayerBase, IExtendedCreature
	{

		public Player(PlayerCreator creator) : base(creator)
		{
			extendedPerkModifiers = new ExtendedPerkModifiers(this);
			extendedData = new ExtendedCreatureData(this, extendedPerkModifiers);
		}

		//the player uses the various forms of you for this.
		public override string possessiveNoun => Conjugate.YOU.PossessiveNoun();
		public override string objectNoun => Conjugate.YOU.ObjectNoun();
		public override string personalNoun => Conjugate.YOU.PersonalNoun();
		public override string possessiveAdjective => Conjugate.YOU.PossessiveAdjective();
		public override string reflexiveNoun => Conjugate.YOU.ReflexiveNoun();
		public override string personalNounWithAre => Conjugate.YOU.PersonalNounWithAre();
		public override string personalNounWithHave => Conjugate.YOU.PersonalNounWithHave();


		public override string Article(bool definitiveArticle)
		{
			return "";
		}

		public ExtendedCreatureData extendedData { get; }

		public ExtendedPerkModifiers extendedPerkModifiers { get; }

		//contrast this with the 2000 line monstrosity that is the old PlayerAppearance class. holy shit is this easier to maintain.
		public override string Appearance()
		{
			StringBuilder sb = new StringBuilder();

			//starting race text.

			//current build and race text

			//current gear text.

			//face
			sb.Append(FaceAppearance());
			//facial features. should be handled by beard, but beard is not implemented right now.
			sb.Append(Face.FacialStructureText(femininity.AsReadOnlyData()));
			//eyes
			sb.Append(EyesAppearance());
			//mouth/tongue
			sb.Append(TongueAppearance());
			//hair and ears.
			sb.Append(HairAppearance());
			//antennae.
			sb.Append(AntennaeAppearance());
			//horns
			sb.Append(HornsAppearance());
			//neck.
			sb.Append(NeckAppearance());
			//gills.
			sb.Append(GillsAppearance());
			//body
			sb.Append(BodyAppearance());
			//arms
			sb.Append(ArmsAppearance());
			//back
			sb.Append(BackAppearance());
			//wings
			sb.Append(WingsAppearance());
			//Build (Hips+Butt)

			//tail(s)
			sb.Append(TailAppearance());
			//legs
			sb.Append(LowerBodyAppearance());
			//incorporeal perk not implemented yet, but would appear here. need some way to handle appearance perks for all body parts.

			//pregnancy
			//sb.Append()

			//boobs
			sb.Append(AllBreastsAppearance());
			//lower body genitals text. (not implemented)

			//cocks
			sb.Append(AllCocksAppearance());
			//balls
			sb.Append(BallsAppearance());
			//vaginas
			sb.Append(AllVaginasAppearance());
			//if genderless
			if (gender == Gender.GENDERLESS)
			{
				//NYI
				//sb.Append
			}
			sb.Append(AssAppearance());
			//piercings
			sb.Append(face.eyebrowPiercings.ShortCreatureDescription(this));
			sb.Append(ears.earPiercings.ShortCreatureDescription(this));
			sb.Append(face.nosePiercings.ShortCreatureDescription(this));
			sb.Append(face.lipPiercings.ShortCreatureDescription(this));
			sb.Append(tongue.piercings.ShortCreatureDescription(this));

			sb.Append(breasts[0].nipplePiercings.ShortCreatureDescription(this));
			sb.Append(body.navelPiercings.ShortCreatureDescription(this));
			sb.Append(body.hipPiercings.ShortCreatureDescription(this));

			sb.Append(tail.tailPiercings.ShortCreatureDescription(this));
			if (hasCock)
			{
				sb.Append(cocks[0].piercings.ShortCreatureDescription(this));
			}
			if (hasVagina)
			{
				sb.Append(vaginas[0].labiaPiercings.ShortCreatureDescription(this));
				sb.Append(vaginas[0].clit.piercings.ShortCreatureDescription(this));
			}
			//tattoos
			sb.Append(face.tattoos.ShortCreatureDescription(this));
			sb.Append(neck.tattoos.ShortCreatureDescription(this));
			sb.Append(arms.tattoos.ShortCreatureDescription(this));
			sb.Append(body.coreTattoos.ShortCreatureDescription(this));
			sb.Append(back.tattoos.ShortCreatureDescription(this));

			sb.Append(butt.tattoos.ShortCreatureDescription(this));
			sb.Append(lowerBody.tattoos.ShortCreatureDescription(this));

			sb.Append(genitals.tattoos.ShortCreatureDescription(this));
			//money

			//not yet implemented

			return sb.ToString();
		}
	}
}
