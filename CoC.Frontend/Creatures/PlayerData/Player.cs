using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
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
			sb.Append(face.eyebrowPiercings.ShortPlayerDescription(this));
			sb.Append(ears.earPiercings.ShortPlayerDescription(this));
			sb.Append(face.nosePiercings.ShortPlayerDescription(this));
			sb.Append(face.lipPiercings.ShortPlayerDescription(this));
			sb.Append(tongue.piercings.ShortPlayerDescription(this));

			sb.Append(breasts[0].nipplePiercings.ShortPlayerDescription(this));
			sb.Append(body.navelPiercings.ShortPlayerDescription(this));
			sb.Append(body.hipPiercings.ShortPlayerDescription(this));

			sb.Append(tail.tailPiercings.ShortPlayerDescription(this));
			if (hasCock)
			{
				sb.Append(cocks[0].piercings.ShortPlayerDescription(this));
			}
			if (hasVagina)
			{
				sb.Append(vaginas[0].labiaPiercings.ShortPlayerDescription(this));
				sb.Append(vaginas[0].clit.piercings.ShortPlayerDescription(this));
			}
			//tattoos
			sb.Append(face.tattoos.ShortPlayerDescription(this));
			sb.Append(neck.tattoos.ShortPlayerDescription(this));
			sb.Append(arms.tattoos.ShortPlayerDescription(this));
			sb.Append(body.coreTattoos.ShortPlayerDescription(this));
			sb.Append(back.tattoos.ShortPlayerDescription(this));

			sb.Append(butt.tattoos.ShortPlayerDescription(this));
			sb.Append(lowerBody.tattoos.ShortPlayerDescription(this));

			sb.Append(genitals.tattoos.ShortPlayerDescription(this));
			//money

			//not yet implemented

			return sb.ToString();
		}
	}
}
