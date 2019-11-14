using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public sealed partial class Ovipositor : BehavioralSaveablePart<Ovipositor, OvipositorType, OvipositorWrapper>
	{
		//if you want to add resource to this, implement body part lazy, make sure it's attached to list of lazies in the creature. 
		//increment resouce as needed. provide function for removing resources.

		public Ovipositor(Guid creatureID) : base(creatureID) { }

		public Ovipositor(Guid creatureID, OvipositorType ovipositorType) : base(creatureID)
		{
			type = ovipositorType ?? throw new ArgumentNullException(nameof(ovipositorType));
		}

		public override OvipositorType defaultType => OvipositorType.defaultValue;

		public override OvipositorType type { get; protected set; }

		public override OvipositorWrapper AsReadOnlyReference()
		{
			return new OvipositorWrapper(this);
		}

		//default update, restore fine. nothing else really required.

		public override string BodyPartName()
		{
			return Name();
		}

		internal override bool Validate(bool correctInvalidData)
		{
			var typeVal = type;
			bool retVal = OvipositorType.Validate(ref typeVal, correctInvalidData);
			type = typeVal;
			return retVal;
		}
	}

	public sealed partial class OvipositorType : SaveableBehavior<OvipositorType, Ovipositor, OvipositorWrapper>
	{
		public override int index => throw new NotImplementedException();

		public static OvipositorType defaultValue => NONE;

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

		public static readonly OvipositorType NONE = new OvipositorType(NoneShortDesc, NoneLongDesc, NonePlayerStr, NoneTransformStr, NoneRestoreStr);
		public static readonly OvipositorType SPIDER = new OvipositorType(SpiderShortDesc, SpiderLongDesc, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr); //none
		public static readonly OvipositorType BEE = new OvipositorType(BeeShortDesc, BeeLongDesc, BeePlayerStr, BeeTransformStr, BeeRestoreStr); //none)

		public OvipositorType(SimpleDescriptor shortDesc, DescriptorWithArg<Ovipositor> fullDesc, TypeAndPlayerDelegate<Ovipositor> playerDesc, 
			ChangeType<Ovipositor> transform, RestoreType<Ovipositor> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore) { }
	}

	public sealed class OvipositorWrapper : BehavioralSaveablePartWrapper<OvipositorWrapper, Ovipositor, OvipositorType>
	{
		public OvipositorWrapper(Ovipositor source) : base(source)
		{
		}
	}
}
