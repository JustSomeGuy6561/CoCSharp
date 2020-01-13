using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using System;

namespace CoC.Backend.BodyParts
{
	public sealed partial class Ovipositor : PartWithBehaviorAndEventBase<Ovipositor, OvipositorType, OvipositorData>
	{
		//if you want to add resource to this, implement body part lazy, make sure it's attached to list of lazies in the creature.
		//increment resouce as needed. provide function for removing resources.

		public Ovipositor(Guid creatureID) : base(creatureID)
		{
			type = TailType.defaultValue.ovipositorType;
		}
		public Ovipositor(Guid creatureID, TailType tailType) : base(creatureID)
		{
			type = tailType?.ovipositorType ?? throw new ArgumentNullException(nameof(tailType));
		}

		public override OvipositorType type { get; protected set; }

		public byte eggCount
		{
			get => _eggCount;
			private set => _eggCount = Utils.Clamp2(value, byte.MinValue, type.maxEggs);
		}
		private byte _eggCount;

		public override OvipositorData AsReadOnlyData()
		{
			return new OvipositorData(this);
		}

		//default update, restore fine. nothing else really required.

		public override string BodyPartName()
		{
			return Name();
		}
	}

	//this is dumbed down because it's not a true type, but a sub-part of tail. that said, it has a strange behavior where the creature may not have it, even if they have the tail for it.
	//as such, we can't just include it in the tail text, so we need a transform and restore here. However, we need more info than the standard transform/restore delegates give us -
	//are we losing the ovipositor because the type changed, or did we somehow lose it even though we still have the tail? Did we gain it with the tail on tail transform? did we enable it
	//while already having the prerequisite tail?
	public sealed partial class OvipositorType : BehaviorBase
	{
		public override int id => throw new NotImplementedException();

		public static OvipositorType defaultValue => NONE;

		public readonly byte maxEggs;

		public static bool Validate(ref OvipositorType type, bool correctInvalidData)
		{
			if (type is null)
			{
				if (correctInvalidData)
				{
					type = defaultValue;
				}
				return false;
			}
			else
			{
				return true;
			}
		}

		//note that the previous ovipositor data is stored within the tail data class, and the tail data class also lets us know if the tail type transformed as well.
		private delegate string OvipositorTransform(TailData tailPreTransform, PlayerBase player);
		//note that the previous ovipositor data is stored within the tail data class, and the tail data class also lets us know if the tail type was restored as well.
		private delegate string OvipositorRestore(TailData originalTailData, PlayerBase player);

		public static readonly OvipositorType NONE = new OvipositorType(NoneShortDesc, NoneLongDesc, NonePlayerStr, NoneTransformStr, NoneRestoreStr);
		public static readonly OvipositorType SPIDER = new OvipositorType(50, SpiderShortDesc, SpiderLongDesc, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr); //none
		public static readonly OvipositorType BEE = new OvipositorType(50, BeeShortDesc, BeeLongDesc, BeePlayerStr, BeeTransformStr, BeeRestoreStr); //none)

		private readonly PartDescriptor<OvipositorData> longDesc;
		private readonly PlayerBodyPartDelegate<Ovipositor> playerDesc;

		private readonly OvipositorTransform transformStr;
		private readonly OvipositorRestore restoreStr;


		private OvipositorType(ShortDescriptor shortDesc, PartDescriptor<OvipositorData> longDesc, PlayerBodyPartDelegate<Ovipositor> playerDesc,
			OvipositorTransform transform, OvipositorRestore restore) : this(0, shortDesc, longDesc, playerDesc, transform, restore) {}

		private OvipositorType(byte maxEggs, ShortDescriptor shortDesc, PartDescriptor<OvipositorData> longDesc, PlayerBodyPartDelegate<Ovipositor> playerDesc,
			OvipositorTransform transform, OvipositorRestore restore) : base(shortDesc)
		{
			this.maxEggs = maxEggs;

			this.longDesc = longDesc ?? throw new ArgumentNullException(nameof(longDesc));
			this.playerDesc = playerDesc ?? throw new ArgumentNullException(nameof(playerDesc));

			this.transformStr = transform ?? throw new ArgumentNullException(nameof(transform));
			restoreStr = restore ?? throw new ArgumentNullException(nameof(restore));
		}
	}

	public sealed class OvipositorData : BehavioralPartDataBase<OvipositorType>
	{
		public readonly byte eggCount;

		public byte maxEggs => type.maxEggs;

		public OvipositorData(Ovipositor source) : base(GetID(source), GetBehavior(source))
		{
			eggCount = source.eggCount;
		}

		private static Guid GetID(Ovipositor source)
		{
			if (source is null) throw new ArgumentNullException(nameof(source));
			return source.creatureID;
		}

		private static OvipositorType GetBehavior(Ovipositor source)
		{
			if (source is null) throw new ArgumentNullException(nameof(source));
			return source.type;
		}
	}
}
