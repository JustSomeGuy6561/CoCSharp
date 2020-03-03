using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Tools;
using CoC.LinqHelpers;

namespace CoC.Backend.Perks
{
	//perk modifiers can
	public enum ValueModifierType { FLAT_ADD, MINIMUM, MAXIMUM, RELATIVE }

	internal interface IRemovableModifier
	{
		bool RemoveModifier(PerkBase perk);
	}

	//this is necessary because perks handle the add/remove for you and if we had the U in it, that'd require reflection. what a pain in the ass.
	public abstract class PerkModifierBase<T> : IRemovableModifier
	{
		protected PerkModifierBase()
		{
		}

		public abstract bool hasAnyActiveMembers { get; }

		//these are internal. we expose these indirectly through PerkBase. this way, we can guarentee the perk modifiers are always up to date, and you don't
		//have to manually add and remove them all the time - simply tell the perk to do it, and it'll automatically add and remove when activated or deactivated, respectively.

		protected internal abstract bool AddModifier(PerkBase perk, T valueModifier, bool overwriteExisting = false);

		protected internal abstract bool RemoveModifier(PerkBase perk);
		protected internal abstract bool HasModifier(PerkBase perk);

		bool IRemovableModifier.RemoveModifier(PerkBase perk) => RemoveModifier(perk);
	}

	//helper abstract class so the GetValue function exists in each one. it's not used in any abstract context (afaik), but i need it to be implemented
	//so you can actually use it lol.

	public abstract class PerkModifierWithValue<T, U> : PerkModifierBase<T>
	{
		protected readonly Action<U> onChange;
		protected PerkModifierWithValue(Action<U> changeAction) : base()
		{
			onChange = changeAction;
		}

		public abstract U GetValue();
	}

	public abstract class ValuePerkModifier<T> : PerkModifierWithValue<ValueModifierStore<T>, T>
	{
		//Note: this can be null.

		protected readonly Dictionary<Type, ValueModifierStore<T>> valueModifiers = new Dictionary<Type, ValueModifierStore<T>>();

		public override bool hasAnyActiveMembers => valueModifiers.Count > 0;

		protected ValuePerkModifier(Action<T> onChange) : base(onChange)
		{
		}

		protected abstract bool CheckChanged(T oldValue);

		protected internal override bool AddModifier(PerkBase perk, ValueModifierStore<T> valueModifier, bool overwriteExisting = false)
		{
			Type type = perk.GetType();

			if (valueModifiers.ContainsKey(type) && !overwriteExisting)
			{
				return false;
			}
			else
			{
				T oldT = GetValue();
				valueModifiers[type] = valueModifier;
				if (CheckChanged(oldT))
				{
					onChange?.Invoke(oldT);
				}
				return true;
			}
		}

		protected internal override bool RemoveModifier(PerkBase perk)
		{
			Type type = perk.GetType();
			T oldT = GetValue();
			if (valueModifiers.Remove(type))
			{
				if (CheckChanged(oldT))
				{
					onChange?.Invoke(oldT);
				}
				return true;
			}

			return false;
		}

		protected internal override bool HasModifier(PerkBase perk)
		{
			return valueModifiers.ContainsKey(perk.GetType());
		}
	}

	public sealed class ValueModifierStore<T>
	{
		public readonly ValueModifierType modifierType;
		public readonly T value;

		public ValueModifierStore(ValueModifierType modifierType, T value)
		{
			this.modifierType = modifierType;
			this.value = value;
		}
	}


	public class DoublePerkModifier : ValuePerkModifier<double>
	{
		private readonly double init;
		private readonly double? minCap;
		private readonly double? maxCap;


		public DoublePerkModifier(double initialValue, double? minumumCap = null, double? maximumCap = null, Action<double> onChange = null) : base(onChange)
		{
			init = initialValue;
			minCap = minumumCap;
			maxCap = maximumCap;
		}

		public override double GetValue()
		{
			double value = init;

			foreach (ValueModifierStore<double> modifier in valueModifiers.Values.Where(x => x.modifierType == ValueModifierType.RELATIVE))
			{
				value += modifier.value;
			}
			foreach (ValueModifierStore<double> modifier in valueModifiers.Values.Where(x => x.modifierType == ValueModifierType.FLAT_ADD))
			{
				value += modifier.value;
			}

			double? min = valueModifiers.Where(x => x.Value.modifierType == ValueModifierType.MINIMUM).MinOrNull(x => x.Value.value);

			if (minCap is double && min is double)
			{
				min = Math.Max((double)min, (double)minCap);
			}
			else if (minCap is double)
			{
				min = minCap;
			}

			double? max = valueModifiers.Where(x => x.Value.modifierType == ValueModifierType.MAXIMUM).MaxOrNull(x => x.Value.value);
			if (maxCap is double && max is double)
			{
				max = Math.Min((double)max, (double)maxCap);
			}

			else if (maxCap is double)
			{
				max = maxCap;
			}

			if (min is double && max is double)
			{
				return Utils.Clamp2(value, (double)min, (double)max);
			}
			else if (min is double minD && value < minD)
			{
				return minD;
			}
			else if (max is double maxD && value > maxD)
			{
				return maxD;
			}
			else
			{
				return value;
			}
		}

		protected override bool CheckChanged(double oldValue)
		{
			return GetValue() != oldValue;
		}
	}

	public class BytePerkModifier : ValuePerkModifier<byte>
	{
		private readonly byte init;
		private readonly byte? minCap;
		private readonly byte? maxCap;


		public BytePerkModifier(byte initialValue, byte? minumumCap = null, byte? maximumCap = null,
			Action<byte> onChange = null) : base(onChange)
		{
			init = initialValue;
			minCap = minumumCap;
			maxCap = maximumCap;
		}

		public override byte GetValue()
		{
			byte value = init;

			foreach (ValueModifierStore<byte> modifier in valueModifiers.Values.Where(x => x.modifierType == ValueModifierType.RELATIVE))
			{
				value += modifier.value;
			}
			foreach (ValueModifierStore<byte> modifier in valueModifiers.Values.Where(x => x.modifierType == ValueModifierType.FLAT_ADD))
			{
				value += modifier.value;
			}

			byte? min = valueModifiers.Where(x => x.Value.modifierType == ValueModifierType.MINIMUM).MinOrNull(x => x.Value.value);

			if (minCap is byte && min is byte)
			{
				min = Math.Max((byte)min, (byte)minCap);
			}
			else if (minCap is byte)
			{
				min = minCap;
			}

			byte? max = valueModifiers.Where(x => x.Value.modifierType == ValueModifierType.MAXIMUM).MaxOrNull(x => x.Value.value);
			if (maxCap is byte && max is byte)
			{
				max = Math.Min((byte)max, (byte)maxCap);
			}

			else if (maxCap is byte)
			{
				max = maxCap;
			}

			if (min is byte && max is byte)
			{
				return Utils.Clamp2(value, (byte)min, (byte)max);
			}
			else if (min is byte minD && value < minD)
			{
				return minD;
			}
			else if (max is byte maxD && value > maxD)
			{
				return maxD;
			}
			else
			{
				return value;
			}
		}
		protected override bool CheckChanged(byte oldValue)
		{
			return GetValue() != oldValue;
		}
	}

	public class SignedBytePerkModifier : ValuePerkModifier<sbyte>
	{
		private readonly sbyte init;
		private readonly sbyte? minCap;
		private readonly sbyte? maxCap;


		public SignedBytePerkModifier(sbyte initialValue, sbyte? minumumCap = null, sbyte? maximumCap = null, Action<sbyte> onChange = null) : base(onChange)
		{
			init = initialValue;
			minCap = minumumCap;
			maxCap = maximumCap;
		}

		public override sbyte GetValue()
		{
			sbyte value = init;

			foreach (ValueModifierStore<sbyte> modifier in valueModifiers.Values.Where(x => x.modifierType == ValueModifierType.RELATIVE))
			{
				value += modifier.value;
			}
			foreach (ValueModifierStore<sbyte> modifier in valueModifiers.Values.Where(x => x.modifierType == ValueModifierType.FLAT_ADD))
			{
				value += modifier.value;
			}

			sbyte? min = valueModifiers.Where(x => x.Value.modifierType == ValueModifierType.MINIMUM).MinOrNull(x => x.Value.value);

			if (minCap is sbyte && min is sbyte)
			{
				min = Math.Max((sbyte)min, (sbyte)minCap);
			}
			else if (minCap is sbyte)
			{
				min = minCap;
			}

			sbyte? max = valueModifiers.Where(x => x.Value.modifierType == ValueModifierType.MAXIMUM).MaxOrNull(x => x.Value.value);
			if (maxCap is sbyte && max is sbyte)
			{
				max = Math.Min((sbyte)max, (sbyte)maxCap);
			}

			else if (maxCap is sbyte)
			{
				max = maxCap;
			}

			if (min is sbyte && max is sbyte)
			{
				return Utils.Clamp2(value, (sbyte)min, (sbyte)max);
			}
			else if (min is sbyte minD && value < minD)
			{
				return minD;
			}
			else if (max is sbyte maxD && value > maxD)
			{
				return maxD;
			}
			else
			{
				return value;
			}
		}

		protected override bool CheckChanged(sbyte oldValue)
		{
			return GetValue() != oldValue;
		}
	}

	public class UnsignedShortPerkModifier : ValuePerkModifier<ushort>
	{
		private readonly ushort init;
		private readonly ushort? minCap;
		private readonly ushort? maxCap;


		public UnsignedShortPerkModifier(ushort initialValue, ushort? minumumCap = null, ushort? maximumCap = null, Action<ushort> onChange = null) : base(onChange)
		{
			init = initialValue;
			minCap = minumumCap;
			maxCap = maximumCap;
		}

		public override ushort GetValue()
		{
			ushort value = init;

			foreach (ValueModifierStore<ushort> modifier in valueModifiers.Values.Where(x => x.modifierType == ValueModifierType.RELATIVE))
			{
				value += modifier.value;
			}
			foreach (ValueModifierStore<ushort> modifier in valueModifiers.Values.Where(x => x.modifierType == ValueModifierType.FLAT_ADD))
			{
				value += modifier.value;
			}

			ushort? min = valueModifiers.Where(x => x.Value.modifierType == ValueModifierType.MINIMUM).MinOrNull(x => x.Value.value);

			if (minCap is ushort && min is ushort)
			{
				min = Math.Max((ushort)min, (ushort)minCap);
			}
			else if (minCap is ushort)
			{
				min = minCap;
			}

			ushort? max = valueModifiers.Where(x => x.Value.modifierType == ValueModifierType.MAXIMUM).MaxOrNull(x => x.Value.value);
			if (maxCap is ushort && max is ushort)
			{
				max = Math.Min((ushort)max, (ushort)maxCap);
			}

			else if (maxCap is ushort)
			{
				max = maxCap;
			}

			if (min is ushort && max is ushort)
			{
				return Utils.Clamp2(value, (ushort)min, (ushort)max);
			}
			else if (min is ushort minD && value < minD)
			{
				return minD;
			}
			else if (max is ushort maxD && value > maxD)
			{
				return maxD;
			}
			else
			{
				return value;
			}
		}
		protected override bool CheckChanged(ushort oldValue)
		{
			return GetValue() != oldValue;
		}
	}

	public class UnsignedIntegerPerkModifier : ValuePerkModifier<uint>
	{
		private readonly uint init;
		private readonly uint? minCap;
		private readonly uint? maxCap;


		public UnsignedIntegerPerkModifier(uint initialValue, uint? minumumCap = null, uint? maximumCap = null, Action<uint> onChange = null) : base(onChange)
		{
			init = initialValue;
			minCap = minumumCap;
			maxCap = maximumCap;
		}

		public override uint GetValue()
		{
			uint value = init;

			foreach (ValueModifierStore<uint> modifier in valueModifiers.Values.Where(x => x.modifierType == ValueModifierType.RELATIVE))
			{
				value += modifier.value;
			}
			foreach (ValueModifierStore<uint> modifier in valueModifiers.Values.Where(x => x.modifierType == ValueModifierType.FLAT_ADD))
			{
				value += modifier.value;
			}

			uint? min = valueModifiers.Where(x => x.Value.modifierType == ValueModifierType.MINIMUM).MinOrNull(x => x.Value.value);

			if (minCap is uint && min is uint)
			{
				min = Math.Max((uint)min, (uint)minCap);
			}
			else if (minCap is uint)
			{
				min = minCap;
			}

			uint? max = valueModifiers.Where(x => x.Value.modifierType == ValueModifierType.MAXIMUM).MaxOrNull(x => x.Value.value);
			if (maxCap is uint && max is uint)
			{
				max = Math.Min((uint)max, (uint)maxCap);
			}

			else if (maxCap is uint)
			{
				max = maxCap;
			}

			if (min is uint && max is uint)
			{
				return Utils.Clamp2(value, (uint)min, (uint)max);
			}
			else if (min is uint minD && value < minD)
			{
				return minD;
			}
			else if (max is uint maxD && value > maxD)
			{
				return maxD;
			}
			else
			{
				return value;
			}
		}
		protected override bool CheckChanged(uint oldValue)
		{
			return GetValue() != oldValue;
		}
	}

	public class ByteEnumPerkModifier<T> : PerkModifierWithValue<ValueModifierStore<byte>, T> where T : struct, Enum
	{
		private readonly T init;
		private readonly T? minCap;
		private readonly T? maxCap;

		public override bool hasAnyActiveMembers => valueModifiers.IsEmpty();

		public ByteEnumPerkModifier(T initialValue, T? minumumCap = null, T? maximumCap = null, Action<T> onChange = null) : base(onChange)
		{
			init = initialValue;
			minCap = minumumCap;
			maxCap = maximumCap;
		}

		protected readonly Dictionary<Type, ValueModifierStore<byte>> valueModifiers = new Dictionary<Type, ValueModifierStore<byte>>();

		public override T GetValue()
		{
			ushort converter = Convert.ToByte(init);

			foreach (ValueModifierStore<byte> modifier in valueModifiers.Values.Where(x => x.modifierType == ValueModifierType.RELATIVE))
			{
				converter += modifier.value;
				if (converter > byte.MaxValue)
				{
					converter = byte.MaxValue;
					break;
				}
			}
			foreach (ValueModifierStore<byte> modifier in valueModifiers.Values.Where(x => x.modifierType == ValueModifierType.FLAT_ADD))
			{
				converter += modifier.value;
				if (converter > byte.MaxValue)
				{
					converter = byte.MaxValue;
					break;
				}
			}

			T value = (T)Enum.Parse((typeof(T)), converter.ToString());

			byte? minVal = valueModifiers.Where(x => x.Value.modifierType == ValueModifierType.MINIMUM).MinOrNull(x => x.Value.value);
			T? min = Enum.TryParse(minVal?.ToString(), out T result) ? result : (T?)null;

			if (minCap is T && min is T)
			{
				min = EnumHelper.Max((T)min, (T)minCap);
			}
			else if (minCap is T)
			{
				min = minCap;
			}

			byte? maxVal = valueModifiers.Where(x => x.Value.modifierType == ValueModifierType.MAXIMUM).MaxOrNull(x => x.Value.value);
			T? max = Enum.TryParse(maxVal?.ToString(), out result) ? result : (T?)null;

			if (maxCap is T && max is T)
			{
				max = EnumHelper.Min((T)max, (T)maxCap);
			}

			else if (maxCap is T)
			{
				max = maxCap;
			}

			if (min is T && max is T)
			{
				return Utils.ClampEnum2(value, (T)min, (T)max);
			}
			else if (min is T minD && value.CompareTo(minD) < 0)
			{
				return minD;
			}
			else if (max is T maxD && value.CompareTo(maxD) > 0)
			{
				return maxD;
			}
			else
			{
				return value;
			}
		}

		protected internal override bool AddModifier(PerkBase perk, ValueModifierStore<byte> valueModifier, bool overwriteExisting = false)
		{
			Type type = perk.GetType();
			if (valueModifiers.ContainsKey(type) && !overwriteExisting)
			{
				return false;
			}
			else
			{
				valueModifiers[type] = valueModifier;
				return true;
			}
		}

		protected internal override bool RemoveModifier(PerkBase perk)
		{
			Type type = perk.GetType();
			return valueModifiers.Remove(type);
		}

		protected internal override bool HasModifier(PerkBase perk)
		{
			return valueModifiers.ContainsKey(perk.GetType());
		}
	}

	//the initial state is the default value. if any perk included here sets the flag to NOT EQUAL the default state, the opposite of the default state is returned.
	//for example, if default state is true, GetState will return false if any perk sets their respective flag to false.
	//We will store any perk that sets the value the same as the default state, even though this seems redundant - derived members may decide to override GetState and
	//use these as they see fit (maybe returning the state which has the majority, for example).
	//Afaik, no code currently sets the state her unless it's not the default, so that is moot. but it could change, i guess
	public class ConditionalPerkModifier : PerkModifierWithValue<bool, bool>
	{
		protected readonly bool initialSetting;
		protected readonly Dictionary<Type, bool> stateSetters = new Dictionary<Type, bool>();

		public override bool hasAnyActiveMembers => stateSetters.IsEmpty();

		public ConditionalPerkModifier(bool initialState, Action<bool> onChange = null) : base(onChange)
		{
			initialSetting = initialState;
		}

		public override bool GetValue()
		{
			//if all match the initial state, return the initial state. if any are not, return the opposite state.
			if (stateSetters.Count == 0)
			{
				return initialSetting;
			}

			return stateSetters.All(x => x.Value == initialSetting) ? initialSetting : !initialSetting;
		}

		protected internal override bool AddModifier(PerkBase perk, bool valueModifier, bool overwriteExisting = false)
		{
			Type type = perk.GetType();

			if (stateSetters.ContainsKey(type) && !overwriteExisting)
			{
				return false;
			}
			else
			{
				stateSetters[type] = valueModifier;
				return true;
			}
		}

		protected internal override bool RemoveModifier(PerkBase perk)
		{
			Type type = perk.GetType();
			return stateSetters.Remove(type);
		}

		protected internal override bool HasModifier(PerkBase perk)
		{
			return stateSetters.ContainsKey(perk.GetType());
		}
	}

	public class VaginalWetnessPerkModifier : ByteEnumPerkModifier<VaginalWetness>
	{
		public VaginalWetnessPerkModifier(VaginalWetness initialValue, VaginalWetness? minumumCap = null, VaginalWetness? maximumCap = null,
			Action<VaginalWetness> onChange = null) : base(initialValue, minumumCap, maximumCap, onChange)
		{ }
	}

	public class VaginalLoosenessPerkModifier : ByteEnumPerkModifier<VaginalLooseness>
	{
		public VaginalLoosenessPerkModifier(VaginalLooseness initialValue, VaginalLooseness? minumumCap = null, VaginalLooseness? maximumCap = null,
			Action<VaginalLooseness> onChange = null)
			: base(initialValue, minumumCap, maximumCap, onChange)
		{ }
	}

	public class AnalWetnessPerkModifier : ByteEnumPerkModifier<AnalWetness>
	{
		public AnalWetnessPerkModifier(AnalWetness initialValue, AnalWetness? minumumCap = null, AnalWetness? maximumCap = null, Action<AnalWetness> onChange = null)
			: base(initialValue, minumumCap, maximumCap, onChange)
		{ }
	}

	public class AnalLoosenessPerkModifier : ByteEnumPerkModifier<AnalLooseness>
	{
		public AnalLoosenessPerkModifier(AnalLooseness initialValue, AnalLooseness? minumumCap = null, AnalLooseness? maximumCap = null,
			Action<AnalLooseness> onChange = null) : base(initialValue, minumumCap, maximumCap, onChange)
		{ }
	}

	public class CupSizePerkModifier : ByteEnumPerkModifier<CupSize>
	{
		public CupSizePerkModifier(CupSize initialValue, CupSize? minumumCap = null, CupSize? maximumCap = null, Action<CupSize> onChange = null)
			: base(initialValue, minumumCap, maximumCap, onChange)
		{ }
	}

}
