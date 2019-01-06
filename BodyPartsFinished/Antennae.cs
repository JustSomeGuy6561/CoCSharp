//Antennae.cs
//Description:
//Author: JustSomeGuy
//12/30/2018, 10:08 PM
using System;
using CoC.Tools;
using CoC.Strings;
using CoC.Strings.BodyParts;
using static CoC.UI.TextOutput;
namespace CoC.BodyParts
{
	public class Antennae : BodyPartBase<Antennae, AntennaeType>
	{
		public override AntennaeType type { get; protected set; }

		public override bool Restore()
		{
			if (type == AntennaeType.NONE)
			{
				return false;
			}
			type = AntennaeType.NONE;
			return type == AntennaeType.NONE;
		}

		public override bool RestoreAndDisplayMessage(Player p)
		{
			if (type == AntennaeType.NONE)
			{
				return false;
			}
			OutputText(restoreString(this, p));
			type = AntennaeType.NONE;
			return type == AntennaeType.NONE;
		}

		public bool UpdateAntennae(AntennaeType newType)
		{
			if (type == newType)
			{
				return false;
			}
			type = newType;
			return type == newType;
		}

		public bool UpdateAntennaeAndDisplayMessage(AntennaeType newType, Player player)
		{
			if (type == newType)
			{
				return false;
			}
			OutputText(transformFrom(this, player));
			type = newType;
			return type == newType;
		}
	}

	public class AntennaeType : BodyPartBehavior<AntennaeType, Antennae>
	{
		private static int indexMaker = 0;


		protected AntennaeType(GenericDescription desc, CreatureDescription<Antennae> creatureDesc, PlayerDescription<Antennae> playerDesc,
			ChangeType<Antennae> transformMessage, ChangeType<Antennae> revertToDefault) : base(desc, creatureDesc, playerDesc, transformMessage, revertToDefault)
		{
			_index = indexMaker++;
		}

		public override int index
		{
			get { return _index; }
		}
		private readonly int _index;

		//Don't do this to this level lol. I just used lambdas everywhere because i changed the signature in the base to make things behave better globally, and didn't want to deal 
		//with doing that to everything in here. do use lambdas if you need something not there or you want to use the empty string. 
		public static readonly AntennaeType NONE = new AntennaeType(GlobalStrings.None, (x, y) => { return GlobalStrings.None(); },
			(x, y) => { return GlobalStrings.None(); }, AntennaeStrings.RemoveAntennaeStr, GlobalStrings.RevertAsDefault);

		public static readonly AntennaeType BEE = new AntennaeType(AntennaeStrings.BeeAntennae, (x, y) => { return AntennaeStrings.BeeAntennaCreature(y); },
			(x, y) => { return AntennaeStrings.BeeAntennaePlayer(y); }, AntennaeStrings.BeeAntennaeTransform, AntennaeStrings.BeeAntennaeRestore);

		public static readonly AntennaeType COCKATRICE = new AntennaeType(AntennaeStrings.CockatriceAntennae, (x, y) => { return AntennaeStrings.CockatriceAntennaCreature(y); },
			(x, y) => { return AntennaeStrings.CockatriceAntennaePlayer(y); }, AntennaeStrings.CockatriceAntennaeTransform, AntennaeStrings.CockatriceAntennaeRestore);
	}
}

