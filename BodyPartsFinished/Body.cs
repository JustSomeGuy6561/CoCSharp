using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.BodyParts.SpecialInteraction;
using CoC.EpidermalColors;
using CoC.Tools;
using static CoC.Strings.BodyParts.BodyStrings;
using static CoC.UI.TextOutput;
namespace CoC.BodyParts
{
	public enum NavelPiercings { TOP, BOTTOM }
	public class Core : EpidermalPiercableBodyPart<Core, CoreType, NavelPiercings>, IToneable, IDyeable
	{
		protected Core(CoreType type, Tones currentTone, FurColor currentFur, PiercingFlags flags) : base(type.epidermisType, currentTone, currentFur, flags) { }

		#region Base Body Part
		public override CoreType type { get; protected set; }

		public override bool Restore()
		{
			if (type == CoreType.SKIN)
			{
				return false;
			}
			type = CoreType.SKIN;

			return true;
		}

		public override bool RestoreAndDisplayMessage(Player player)
		{
			if (type == CoreType.SKIN)
			{
				return false;
			}
			OutputText(restoreString(this, player));
			type = CoreType.SKIN;
			return true;
		}
		#endregion

		#region Generate/Update
		public Core Generate(CoreType coreType, Tones currentTone, FurColor fur, PiercingFlags flags)
		{
			return new Core(coreType, currentTone, fur, flags);
		}

		public bool UpdateCore(CoreType coreType, Tones currentTone, FurColor furColor)
		{
			if (type == coreType)
			{
				return false;
			}
			type = coreType;
			epidermis.UpdateEpidermis(type.epidermisType, currentTone, furColor);
			return true;
		}

		public bool UpdateCoreAndDisplayMessage(CoreType coreType, Tones currentTone, FurColor furColor, Player player)
		{
			if (type == coreType)
			{
				return false;
			}
			OutputText(transformFrom(this, player));
			return UpdateCore(coreType, currentTone, furColor);
		}
		#endregion

		#region piercings
		protected override bool PiercingLocationUnlocked(NavelPiercings piercingLocation)
		{
			return true;
		}
		#endregion

		#region ToneAndDye
		public bool canToneLotion()
		{
			return epidermis.canToneLotion();
		}

		public bool attemptToUseLotion(Tones tone)
		{
			return epidermis.attemptToUseLotion(tone);
		}

		public bool canDye()
		{
			return epidermis.canDye();
		}

		public bool attemptToDye(HairFurColors dye)
		{
			return epidermis.attemptToDye(dye);
		}
		#endregion
	}

	public class CoreType : EpidermalPiercableBehavior<CoreType, Core, NavelPiercings>
	{
		public override int index => epidermisType.index;

		protected CoreType(EpidermisType epidermis, GenericDescription shortDesc, CreatureDescription<Core> creatureDesc, PlayerDescription<Core> playerDesc,
			ChangeType<Core> transform, ChangeType<Core> restore) : base(epidermis, shortDesc, creatureDesc, playerDesc, transform, restore) { }

		public static CoreType SKIN = new CoreType(EpidermisType.SKIN, SkinStr, SkinCreatureStr, SkinPlayerStr, SkinTransformStr, SkinRestoreStr);
		public static CoreType FUR = new CoreType(EpidermisType.FUR, FurStr, FurCreatureStr, FurPlayerStr, FurTransformStr, FurRestoreStr);
		public static CoreType SCALES = new CoreType(EpidermisType.SCALES, ScalesStr, ScalesCreatureStr, ScalesPlayerStr, ScalesTransformStr, ScalesRestoreStr);
		public static CoreType GOO = new CoreType(EpidermisType.GOO, GooStr, GooCreatureStr, GooPlayerStr, GooTransformStr, GooRestoreStr);
		public static CoreType WOOL = new CoreType(EpidermisType.WOOL, WoolStr, WoolCreatureStr, WoolPlayerStr, WoolTransformStr, WoolRestoreStr);
		public static CoreType FEATHERS = new CoreType(EpidermisType.FEATHERS, FeatherStr, FeatherCreatureStr, FeatherPlayerStr, FeatherTransformStr, FeatherRestoreStr);
		public static CoreType BARK = new CoreType(EpidermisType.BARK, BarkStr, BarkCreatureStr, BarkPlayerStr, BarkTransformStr, BarkRestoreStr);
		public static CoreType CARAPACE = new CoreType(EpidermisType.CARAPACE, CarapaceStr, CarapaceCreatureStr, CarapacePlayerStr, CarapaceTransformStr, CarapaceRestoreStr);
		public static CoreType EXOSKELETON = new CoreType(EpidermisType.EXOSKELETON, ExoskeletonStr, ExoskeletonCreatureStr, ExoskeletonPlayerStr, ExoskeletonTransformStr, ExoskeletonRestoreStr);
	}
}
