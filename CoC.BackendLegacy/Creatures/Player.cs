using CoC.Backend.BodyParts;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Save;
using CoC.Backend.Save.Internals;
using System;
using System.Runtime.Serialization;

namespace CoC.Backend.Creatures
{
	[DataContract]
	public sealed class Player : CombatCreature, ISaveableBase
	{

		public Player(PlayerCreator creator) : base(creator)
		{
			DataContractSystem.AddSurrogateData(this);

			//TODO: Add player specific items or whatever.
		}

		internal Player(SurrogatePlayerCreator surrogateCreator) : base(surrogateCreator)
		{
		}

		Type ISaveableBase.currentSaveType => typeof(PlayerSurrogateVersion1);

		Type[] ISaveableBase.saveVersionTypes => new Type[] { typeof(PlayerSurrogateVersion1) };
		object ISaveableBase.ToCurrentSaveVersion()
		{
			return new PlayerSurrogateVersion1()
			{
				antennae = this.antennae,
				arms = this.arms,
				back = this.back,
				body = this.body,
				ears = this.ears,
				eyes = this.eyes,
				gills = this.gills,
				hair = this.hair,
				horns = this.horns,
				tongue = this.tongue,
				wings = this.wings,

			};
		}
	}

	[DataContract]
	public abstract class PlayerSurrogateBase : ISurrogateBase
	{
		internal PlayerSurrogateBase() { }
		object ISurrogateBase.ToSaveable()
		{
			return new Player(ToCreator());
		}

		internal abstract SurrogatePlayerCreator ToCreator();
	}

	[DataContract]
	public sealed class PlayerSurrogateVersion1 : PlayerSurrogateBase
	{
		internal PlayerSurrogateVersion1() : base() { }

		[DataMember]
		public BehavioralSaveablePart<Antennae, AntennaeType> antennae;
		[DataMember]
		public BehavioralSaveablePart<Arms, ArmType> arms;
		[DataMember]
		public BehavioralSaveablePart<Back, BackType> back;
		[DataMember]
		public BehavioralSaveablePart<Body, BodyType> body;
		//[DataMember]
		public BehavioralSaveablePart<Ears, EarType> ears;
		[DataMember]
		public BehavioralSaveablePart<Eyes, EyeType> eyes;
		[DataMember]
		public BehavioralSaveablePart<Gills, GillType> gills;
		[DataMember]
		public BehavioralSaveablePart<Hair, HairType> hair;
		[DataMember]
		public BehavioralSaveablePart<Horns, HornType> horns;
		[DataMember]
		public BehavioralSaveablePart<Tongue, TongueType> tongue;
		[DataMember]
		public BehavioralSaveablePart<Wings, WingType> wings;

		internal override SurrogatePlayerCreator ToCreator()
		{
			return new SurrogatePlayerCreator()
			{
				antennae = (Antennae)this.antennae,
				arms = (Arms)this.arms,
				back = (Back)this.back,
				body = (Body)this.body,
				ears = (Ears)this.ears,
				eyes = (Eyes)this.eyes,
				gills = (Gills)this.gills,
				hair = (Hair)this.hair,
				horns = (Horns)this.horns,
				tongue = (Tongue)this.tongue,
				wings = (Wings)this.wings,
			};
		}
	}
}