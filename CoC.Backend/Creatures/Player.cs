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
			//TODO: Add player specific items or whatever.
		}

		internal Player(SurrogatePlayerCreator surrogateCreator) : base(surrogateCreator)
		{

		}

		internal override void AddSurrogateData()
		{
			base.AddSurrogateData();
			DataContractSystem.AddSurrogateData(this);
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

				eyebrow = this.eyebrow,
				lip = this.lip,
				nose = this.nose,
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
		public Antennae antennae;
		[DataMember]
		public Arms arms;
		[DataMember]
		public Back back;
		[DataMember]
		public Body body;
		[DataMember]
		public Ears ears;
		[DataMember]
		public Eyes eyes;
		[DataMember]
		public Gills gills;
		[DataMember]
		public Hair hair;
		[DataMember]
		public Horns horns;
		[DataMember]
		public Tongue tongue;
		[DataMember]
		public Wings wings;
		//piercings
		[DataMember]
		public Eyebrow eyebrow;
		[DataMember]
		public Lip lip;
		[DataMember]
		public Nose nose;


		internal override SurrogatePlayerCreator ToCreator()
		{
			return new SurrogatePlayerCreator()
			{
				antennae = this.antennae,
				arms = this.arms,
				back = this.back,
				body = this.body,
				ears = this.ears,
				eyebrow = this.eyebrow,
				eyes = this.eyes,
				gills = this.gills,
				hair = this.hair,
				horns = this.horns,
				lip = this.lip,
				nose = this.nose,
				tongue = this.tongue,
				wings = this.wings,
			};
		}
	}
}