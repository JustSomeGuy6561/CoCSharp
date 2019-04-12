using CoC.Backend.CoC_Colors;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CoC.Backend.BodyParts
{
	//public enum HairStyles { NYI, NYI_2, REALLY_NYI}
	[DataContract]
	public class Hair : BehavioralSaveablePart<Hair, HairType>// itimeaware //possibly ICanAttackWith, if i can fit in a Shantae Easter Egg somewhere.
	{
		private static readonly HairFurColors DEFAULT_COLOR = HairFurColors.BLACK;

		//make sure to check this when deserializing - if you dont use the property is may cause errors.
		public HairFurColors hairColor
		{
			get => _hairColor;
			private set
			{
				if (!value.isEmpty)
				{
					_hairColor = value;
				}
			}
		}
		private HairFurColors _hairColor = DEFAULT_COLOR;
		public HairFurColors highlightColor { get; private set; } = HairFurColors.NO_HAIR_FUR;

		//public HairStyles hairStyle {get; private set;}
		public float length { get; private set; } = 10;
		public override HairType type { get; protected set; }

		public override bool isDefault => throw new NotImplementedException();

		internal static Hair GenerateDefault()
		{
			return new Hair();
		}

		internal static Hair GenerateDefaultOfType(HairType hairType)
		{
			throw new NotImplementedException();
		}
		internal static Hair GenerateWithLength(HairType hairType, float hairLength)
		{
			throw new NotImplementedException();
		}

		internal static Hair GenerateWithColor(HairType hairType, HairFurColors color, float? hairLength)
		{
			throw new NotImplementedException();
		}

		internal static Hair GenerateWithColorAndHighlight(HairType hairType, HairFurColors color, HairFurColors highlight, float? length)
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

		private protected override Type currentSaveVersion => typeof(HairSurrogateVersion1);

		private protected override Type[] saveVersions => new Type[] { typeof(HairSurrogateVersion1) };

		private protected override BehavioralSurrogateBase<Hair, HairType> ToCurrentSave()
		{
			return new HairSurrogateVersion1()
			{

			};
		}

		internal override bool Validate(bool correctDataIfInvalid = false)
		{
			throw new NotImplementedException();
		}
	}

	public class HairType : SaveableBehavior<HairType, Hair>
	{
		private static int indexMaker = 0;
		private readonly int _index;
		private static readonly List<HairType> hairTypes = new List<HairType>();

		public HairType(float defaultLength, HairFurColors defaultColor, bool overrideColorOnChange,
			SimpleDescriptor shortDesc, DescriptorWithArg<Hair> fullDesc, TypeAndPlayerDelegate<Hair> playerDesc, 
			ChangeType<Hair> transform, RestoreType<Hair> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			hairTypes.AddAt(this, _index);
		}

		public override int index => throw new NotImplementedException();
	}

	[DataContract]
	public class HairSurrogateVersion1 : BehavioralSurrogateBase<Hair, HairType>
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
		internal readonly float hairLength;
		internal HairData(HairType type, HairFurColors color, HairFurColors highlight, float hairLen)
		{
			hairType = type;
			hairColor = color;
			highlightColor = highlight;
			hairLength = hairLen;
		}

	}
}
