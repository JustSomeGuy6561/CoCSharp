//Player.cs
//Description:
//Author: JustSomeGuy
//12/30/2018, 10:36 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.BodyParts;
using CoC.BodyParts.SpecialInteraction;

namespace CoC
{

	public class Player : Creature
	{
		public Player(string creatureName) : base(creatureName)
		{
			#region attach all the shit
			if (antennae is IFurAware)
			{
				body.AttachIFurAware((IFurAware)antennae);
			}
			if (arms is IFurAware)
			{
				body.AttachIFurAware((IFurAware)arms);
			}
			if (back is IFurAware)
			{
				body.AttachIFurAware((IFurAware)back);
			}
			if (ears is IFurAware)
			{
				body.AttachIFurAware((IFurAware)ears);
			}
			if (face is IFurAware)
			{
				body.AttachIFurAware((IFurAware)face);
			}
			if (genitals is IFurAware)
			{
				body.AttachIFurAware((IFurAware)genitals);
			}
			if (gills is IFurAware)
			{
				body.AttachIFurAware((IFurAware)gills);
			}
			if (horns is IFurAware)
			{
				body.AttachIFurAware((IFurAware)horns);
			}
			if (lowerBody is IFurAware)
			{
				body.AttachIFurAware((IFurAware)lowerBody);
			}
			if (neck is IFurAware)
			{
				body.AttachIFurAware((IFurAware)neck);
			}
			if (tail is IFurAware)
			{
				body.AttachIFurAware((IFurAware)tail);
			}
			if (tongue is IFurAware)
			{
				body.AttachIFurAware((IFurAware)tongue);
			}
			if (wings is IFurAware)
			{
				body.AttachIFurAware((IFurAware)wings);
			}
			if (antennae is IToneAware)
			{
				body.AttachIToneAware((IToneAware)antennae);
			}
			if (arms is IToneAware)
			{
				body.AttachIToneAware((IToneAware)arms);
			}
			if (back is IToneAware)
			{
				body.AttachIToneAware((IToneAware)back);

			}
			if (ears is IToneAware)
			{
				body.AttachIToneAware((IToneAware)ears);
			}
			if (face is IToneAware)
			{
				body.AttachIToneAware((IToneAware)face);

			}
			if (genitals is IToneAware)
			{
				body.AttachIToneAware((IToneAware)genitals);
			}
			if (gills is IToneAware)
			{
				body.AttachIToneAware((IToneAware)gills);
			}
			if (horns is IToneAware)
			{
				body.AttachIToneAware((IToneAware)horns);
			}
			if (lowerBody is IToneAware)
			{
				body.AttachIToneAware((IToneAware)lowerBody);
			}
			if (neck is IToneAware)
			{
				body.AttachIToneAware((IToneAware)neck);
			}
			if (tail is IToneAware)
			{
				body.AttachIToneAware((IToneAware)tail);
			}
			if (tongue is IToneAware)
			{
				body.AttachIToneAware((IToneAware)tongue);
			}
			if (wings is IToneAware)
			{
				body.AttachIToneAware((IToneAware)wings);
			}
			if (antennae is IHairAware)
			{
				hair.AttachIHairAware((IHairAware)antennae);
			}
			if (arms is IHairAware)
			{
				hair.AttachIHairAware((IHairAware)arms);
			}
			if (back is IHairAware)
			{
				hair.AttachIHairAware((IHairAware)back);
			}
			if (ears is IHairAware)
			{
				hair.AttachIHairAware((IHairAware)ears);
			}
			if (face is IHairAware)
			{
				hair.AttachIHairAware((IHairAware)face);

			}
			if (genitals is IHairAware)
			{
				hair.AttachIHairAware((IHairAware)genitals);
			}
			if (gills is IHairAware)
			{
				hair.AttachIHairAware((IHairAware)gills);
			}
			if (horns is IHairAware)
			{
				hair.AttachIHairAware((IHairAware)horns);
			}
			if (lowerBody is IHairAware)
			{
				hair.AttachIHairAware((IHairAware)lowerBody);
			}
			if (neck is IHairAware)
			{
				hair.AttachIHairAware((IHairAware)neck);
			}
			if (tail is IHairAware)
			{
				hair.AttachIHairAware((IHairAware)tail);
			}
			if (tongue is IHairAware)
			{
				hair.AttachIHairAware((IHairAware)tongue);
			}
			if (wings is IHairAware)
			{
				hair.AttachIHairAware((IHairAware)wings);
			}
			#endregion
		}

		public CoC.Tools.SimpleDescriptor gemsString { get; private set; }

		public override void InitBody(out Antennae antennae, out Arms arms, out Back back, out Body body, out Ears ears, out Face face, out Genitals genitals, out Gills gills, out Horns horns, out LowerBody lowerBody, out Neck neck, out Tail tail, out Tongue tongue, out Wings wings, out FacialHair facialHair, PiercingFlags piercingFlags)
		{
			throw new NotImplementedException();
		}

		public override void InitCombat(out int level, out int experience, out Weapon weapon, out Armor armor, out Shield shield, out Jewelry jewelry, out UpperGarment upperGarment, out LowerGarment lowerGarment)
		{
			throw new NotImplementedException();
		}

		public override void InitStats(out int strength, out int toughness, out int speed, out int intelligence, out int lust, out int sensitivity, out int libido, out int corruption, out int money)
		{
			throw new NotImplementedException();
		}
	}
}
