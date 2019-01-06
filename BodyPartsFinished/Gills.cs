//Gills.cs
//Description:
//Author: JustSomeGuy
//12/27/2018, 7:29 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.Tools;
using CoC.Strings;
using static CoC.Strings.BodyParts.GillStrings;
using static CoC.UI.TextOutput;
//using CoC.
namespace CoC.BodyParts
{
	public class Gills : BodyPartBase<Gills, GillType>
	{
		public Gills()
		{
			type = GillType.NONE;
		}

		public override GillType type { get; protected set; }

		public bool UpdateType(GillType gillType)
		{
			if (type == gillType)
			{
				return false;
			}
			type = gillType;
			return type == gillType;

		}

		public bool UpdateTypeAndDisplayMessage(GillType gillType, Player player)
		{
			if (type == gillType)
			{
				return false;
			}
			OutputText(transformFrom(this, player));
			type = gillType;
			return type == gillType;
		}

		public override bool Restore()
		{
			if (type == GillType.NONE)
			{
				return false;
			}
			type = GillType.NONE;
			return type == GillType.NONE;
		}

		public override bool RestoreAndDisplayMessage(Player player)
		{
			if (type == GillType.NONE)
			{
				return false;
			}
			OutputText(restoreString(this, player));
			type = GillType.NONE;
			return type == GillType.NONE;
		}
	}

	public class GillType : BodyPartBehavior<GillType, Gills>
	{
		private static int indexMaker = 0;

		protected GillType(GenericDescription shortDesc, CreatureDescription<Gills> creatureDesc, PlayerDescription<Gills> playerDesc, 
			ChangeType<Gills> transform, ChangeType<Gills> restore) : base(shortDesc, creatureDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
		}

		//public 

		protected readonly int _index;
		public override int index => _index;

		public static readonly GillType NONE = new GillType(GlobalStrings.None, (x,y) => GlobalStrings.None(), (x,y) => GlobalStrings.None(), GlobalStrings.TransformToDefault<Gills, GillType>, GlobalStrings.RevertAsDefault);
		public static readonly GillType ANEMONE = new GillType(AnemoneDescStr, AnemoneCreatureStr, AnemonePlayerStr, AnemoneTransformStr, AnemoneRestoreStr);
		public static readonly GillType FISH = new GillType(FishDescStr, FishCreatureStr, FishPlayerStr, FishTransformStr, FishRestoreStr);
	}
}
