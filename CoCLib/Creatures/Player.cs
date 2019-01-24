//Player.cs
//Description:
//Author: JustSomeGuy
//12/30/2018, 10:36 PM
using  CoC.BodyParts;
using  CoC.BodyParts.SpecialInteraction;
using CoC.UI;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CoC.Creatures
{

	public class Player : CombatCreature, INotifyPropertyChanged
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

		public override void InitStats(out float strength, out float toughness, out float speed, out float intelligence, out float lust, out float sensitivity, out float libido, out float corruption, out float money)
		{
			throw new NotImplementedException();
		}

		//The following are overridden so controller item can be easily updated. allows communication with UI without UI needing to know what's going on here.
		public override float level
		{
			get => base.level;
			protected set
			{
				NotifyPropertyIfChanged(base.level, value);
				base.level = value;
			}
		}
		public override float experience
		{
			get => base.experience;
			protected set
			{
				NotifyPropertyIfChanged(base.experience, value);
				base.experience = value;
			}
		}
		public override float strength
		{
			get => base.strength;
			protected set
			{
				NotifyPropertyIfChanged(base.strength, value);
				base.strength = value;
			}
		}
		public override float toughness
		{
			get => base.toughness;
			protected set
			{
				NotifyPropertyIfChanged(base.toughness, value);
				base.toughness = value;
			}
		}
		public override float speed
		{
			get => base.speed;
			protected set
			{
				NotifyPropertyIfChanged(base.speed, value);
				base.speed = value;
			}
		}
		public override float intelligence
		{
			get => base.intelligence;
			protected set
			{
				NotifyPropertyIfChanged(base.intelligence, value);
				base.intelligence = value;
			}
		}
		public override float corruption
		{
			get => base.corruption;
			protected set
			{
				NotifyPropertyIfChanged(base.corruption, value);
				base.corruption = value;
			}
		}
		public override float hp
		{
			get => base.hp;
			protected set
			{
				NotifyPropertyIfChanged(base.hp, value);
				base.hp = value;
			}
		}
		public override float lust
		{
			get => base.lust;
			protected set
			{
				NotifyPropertyIfChanged(base.lust, value);
				base.lust = value;
			}
		}
		public override float fatigue
		{
			get => base.fatigue;
			protected set
			{
				NotifyPropertyIfChanged(base.fatigue, value);
				base.fatigue = value;
			}
		}
		public override float satiety
		{
			get => base.satiety;
			protected set
			{
				NotifyPropertyIfChanged(base.satiety, value);
				base.satiety = value;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyIfChanged<T>(T item, T newValue, [CallerMemberName] string propertyName = "")
		{
			if (!item.Equals(newValue))
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
