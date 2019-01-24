//Antennae.cs
//Description:
//Author: JustSomeGuy
//12/30/2018, 10:08 PM
using CoC.Tools;
using CoC.Strings;
using CoC.Creatures;
using   CoC.BodyParts;

using static CoC.UI.TextOutput;
namespace  CoC.BodyParts
{
	public class Antennae : BodyPartBase<Antennae, AntennaeType>
	{
		public override AntennaeType type { get; protected set; }

		protected Antennae(AntennaeType antennaeType)
		{
			type = antennaeType;
		}


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
			OutputText(restoreString(p));
			type = AntennaeType.NONE;
			return type == AntennaeType.NONE;
		}

		public static Antennae Generate(AntennaeType antennaeType)
		{
			return new Antennae(antennaeType);
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
			OutputText(transformInto(newType, player));
			type = newType;
			return type == newType;
		}
	}

	public partial class AntennaeType : BodyPartBehavior<AntennaeType, Antennae>
	{
		private static int indexMaker = 0;


		protected AntennaeType(SimpleDescriptor desc, DescriptorWithArg<Antennae> fullDesc, TypeAndPlayerDelegate<Antennae> playerDesc,
			ChangeType<Antennae> transformMessage, RestoreType<Antennae> revertToDefault) : base(desc, fullDesc, playerDesc, transformMessage, revertToDefault)
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
		public static readonly AntennaeType NONE = new AntennaeType(GlobalStrings.None, (x) => GlobalStrings.None(), (x, y) => GlobalStrings.None(), RemoveAntennaeStr, GlobalStrings.RevertAsDefault);

		public static readonly AntennaeType BEE = new AntennaeType(BeeDesc, BeeFullDesc,
			(x, y) => BeePlayer(y), BeeTransform, BeeRestore);

		public static readonly AntennaeType COCKATRICE = new AntennaeType(CockatriceDesc, CockatriceFullDesc,
			(x, y) => CockatricePlayer(y), CockatriceTransform, CockatriceRestore);
	}
}

