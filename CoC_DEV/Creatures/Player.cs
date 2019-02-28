//Player.cs
//Description:
//Author: JustSomeGuy
//12/30/2018, 10:36 PM
using CoC.BodyParts.SpecialInteraction;
using CoC.Serialization;
using CoC.Tools;
//list
using System.Collections.Generic;
//saveable
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
//serialization
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace CoC.Creatures
{
	public class Player : CombatCreature, INotifyPropertyChanged, ISerializable
	{
		public Player(string playerName, Gender gender, Species startingRace) : base(playerName, startingRace)
		{
			Init();
		}

		//
		protected Player(SerializationInfo info, StreamingContext context) : base(info.GetString(nameof(name)), Species.Dereference(info.GetInt32(nameof(startingRace))))
		{
			List<PropertyInfo> propertySaveables = saveableProperties();
			foreach (PropertyInfo saveable in propertySaveables)
			{
				saveable.SetValue(this, info.GetValue(saveable.Name, saveable.PropertyType));
			}
			List<FieldInfo> fieldSaveables = saveableFields();
			foreach (FieldInfo saveable in fieldSaveables)
			{
				saveable.SetValue(this, info.GetValue(saveable.Name, saveable.FieldType));
			}
			//custom deserialization here

			//
			Init();
		}

		protected void Init()
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

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			List<PropertyInfo> propertySaveables = saveableProperties();
			foreach (PropertyInfo saveable in propertySaveables)
			{
				info.AddValue(saveable.Name, saveable.GetValue(this), saveable.PropertyType);
			}
			List<FieldInfo> fieldSaveables = saveableFields();
			foreach (FieldInfo saveable in fieldSaveables)
			{
				info.AddValue(saveable.Name, saveable.GetValue(this), saveable.FieldType);
			}
		}

		private List<PropertyInfo> saveableProperties()
		{
			List<PropertyInfo> saveables = new List<PropertyInfo>();
			PropertyInfo[] props = typeof(Player).GetProperties();
			foreach (PropertyInfo prop in props)
			{
				SaveAttribute attrs = prop.GetCustomAttribute<SaveAttribute>();
				if (attrs != null)
				{
					saveables.Add(prop);
				}
			}
			return saveables;
		}

		private List<FieldInfo> saveableFields()
		{
			List<FieldInfo> saveables = new List<FieldInfo>();
			FieldInfo[] fields = typeof(Player).GetFields();
			foreach (FieldInfo field in fields)
			{
				SaveAttribute attrs = field.GetCustomAttribute<SaveAttribute>();
				if (attrs != null)
				{
					saveables.Add(field);
				}
			}
			return saveables;
		}

		public SimpleDescriptor gemsString { get; private set; }

		//The following are overridden so controller item can be easily updated. allows communication with UI without UI needing to know what's going on here.
		[Save]
		public override int level
		{
			get => base.level;
			protected set
			{
				NotifyPropertyIfChanged(base.level, value);
				base.level = value;
			}
		}
		[Save]
		public override float experience
		{
			get => base.experience;
			protected set
			{
				NotifyPropertyIfChanged(base.experience, value);
				base.experience = value;
			}
		}
		[Save]
		public override float strength
		{
			get => base.strength;
			protected set
			{
				NotifyPropertyIfChanged(base.strength, value);
				base.strength = value;
			}
		}
		[Save]
		public override float toughness
		{
			get => base.toughness;
			protected set
			{
				NotifyPropertyIfChanged(base.toughness, value);
				base.toughness = value;
			}
		}
		[Save]
		public override float speed
		{
			get => base.speed;
			protected set
			{
				NotifyPropertyIfChanged(base.speed, value);
				base.speed = value;
			}
		}
		[Save]
		public override float intelligence
		{
			get => base.intelligence;
			protected set
			{
				NotifyPropertyIfChanged(base.intelligence, value);
				base.intelligence = value;
			}
		}
		[Save]
		public override float corruption
		{
			get => base.corruption;
			protected set
			{
				NotifyPropertyIfChanged(base.corruption, value);
				base.corruption = value;
			}
		}
		[Save]
		public override int hp
		{
			get => base.hp;
			protected set
			{
				NotifyPropertyIfChanged(base.hp, value);
				base.hp = value;
			}
		}
		[Save]
		public override float lust
		{
			get => base.lust;
			protected set
			{
				NotifyPropertyIfChanged(base.lust, value);
				base.lust = value;
			}
		}
		[Save]
		public override float fatigue
		{
			get => base.fatigue;
			protected set
			{
				NotifyPropertyIfChanged(base.fatigue, value);
				base.fatigue = value;
			}
		}
		[Save]
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
