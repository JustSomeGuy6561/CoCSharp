using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoCWinDesktop.Helpers
{

	[Serializable]
	public sealed class HotKey : IEquatable<HotKey>, ISerializable
	{
		private const string KEY_SERIALIZER = "Key";
		private const string MODIFIER_SERIALIZER = "Modifer";

		public Key key { get; }
		public ModifierKeys modifier { get; }

		public HotKey(Key selectedKey, ModifierKeys selectedModifiers)
		{
			key = selectedKey;
			modifier = selectedModifiers;
		}

		public bool Equals(HotKey other)
		{
			return other != null && key == other.key && modifier == other.modifier;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = -1496688484;
				hashCode = hashCode * -1521134295 + key.GetHashCode();
				hashCode = hashCode * -1521134295 + modifier.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(HotKey first, HotKey other)
		{
			return (first is null == other is null) && (first is null || first.Equals(other));
		}

		public static bool operator !=(HotKey first, HotKey other)
		{
			return (first is null != other is null) || (!(first is null) && !first.Equals(other));
		}

		public override bool Equals(object obj)
		{
			if (obj is HotKey other)
			{
				return Equals(other);
			}
			return base.Equals(obj);
		}

		public string AsGestureString()
		{
			KeyBinding keyBinding = new KeyBinding
			{
				Key = key,
				Modifiers = modifier
			};

			KeyGesture gesture = (KeyGesture)keyBinding.Gesture;
			return gesture.GetDisplayStringForCulture(CultureInfo.CurrentCulture);
		}

		public override string ToString()
		{
			return AsGestureString();
		}

		private HotKey(SerializationInfo info, StreamingContext context)
		{
			key = (Key)info.GetValue(KEY_SERIALIZER, typeof(Key));
			modifier = (ModifierKeys)info.GetValue(MODIFIER_SERIALIZER, typeof(ModifierKeys));
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(KEY_SERIALIZER, key, typeof(Key));
			info.AddValue(MODIFIER_SERIALIZER, modifier, typeof(ModifierKeys));
		}
	}
}
