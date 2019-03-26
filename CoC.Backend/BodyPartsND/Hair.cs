using CoC.Backend.CoC_Colors;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts
{
	//public enum HairStyles { NYI, NYI_2, REALLY_NYI}
	public class Hair : BodyPartBase<Hair, HairType>// itimeaware //possibly ICanAttackWith, if i can fit in a Shantae Easter Egg somewhere.
	{
		public HairFurColors hairColor { get; private set; }
		public HairFurColors highlightColor { get; private set; }

		//public HairStyles hairStyle {get; private set;}
		public ushort length { get; private set; }
		public override HairType type { get; protected set; }

		public override bool isDefault => throw new NotImplementedException();

		internal static Hair GenerateDefault()
		{
			throw new NotImplementedException();
		}

		internal static Hair GenerateDefaultOfType(HairType hairType)
		{
			throw new NotImplementedException();
		}
		internal static Hair GenerateWithLength(HairType hairType, ushort hairLength)
		{
			throw new NotImplementedException();
		}

		internal static Hair GenerateWithColor(HairType hairType, HairFurColors color, ushort? hairLength)
		{
			throw new NotImplementedException();
		}

		internal static Hair GenerateWithColorAndHighlight(HairType hairType, HairFurColors color, HairFurColors highlight, ushort? length)
		{
			throw new NotImplementedException();
		}

		internal HairData ToHairData()
		{
			return new HairData(type, hairColor, highlightColor, length);
		}

		internal override bool Restore()
		{
			throw new NotImplementedException();
		}

		internal override Type currentSaveVersion => throw new NotImplementedException();

		internal override Type[] saveVersions => throw new NotImplementedException();

		internal override BodyPartSurrogate<Hair, HairType> ToCurrentSave()
		{
			throw new NotImplementedException();
		}

	}

	public class HairType : BodyPartBehavior<HairType, Hair>
	{
		private static int indexMaker = 0;
		private readonly int _index;
		private static readonly List<HairType> hairTypes = new List<HairType>();

		public HairType(ushort defaultLength, HairFurColors defaultColor, bool overrideColorOnChange,
			SimpleDescriptor shortDesc, DescriptorWithArg<Hair> fullDesc, TypeAndPlayerDelegate<Hair> playerDesc, 
			ChangeType<Hair> transform, RestoreType<Hair> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			hairTypes.AddAt(this, _index);
		}

		public override int index => throw new NotImplementedException();
	}

	public class HairSurrogateVersion1 : BodyPartSurrogate<Hair, HairType>
	{
		internal override Hair ToBodyPart()
		{
			throw new NotImplementedException();
		}
	}

	internal sealed class HairData
	{
		internal readonly HairFurColors hairColor;
		internal readonly HairFurColors highlightColor;
		internal readonly HairType hairType;
		internal readonly ushort hairLength;
		internal HairData(HairType type, HairFurColors color, HairFurColors highlight, ushort hairLen)
		{
			hairType = type;
			hairColor = color;
			highlightColor = highlight;
			hairLength = hairLen;
		}

	}
}
