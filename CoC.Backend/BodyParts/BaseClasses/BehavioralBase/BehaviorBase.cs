//BehaviorBase.cs
//Description:
//Author: JustSomeGuy
//3/26/2019, 8:35 PM
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CoC.Backend.BodyParts
{
	//Stores a simple body part. if any rules need to apply, add the logic here.
	public abstract class BehaviorBase
	{
		public abstract int index { get; }

		private readonly SimpleDescriptor shortDesc;
		private readonly SimpleDescriptor singleDesc;


		//short description describes the whole body part in a concise manner. therefore, if the type has multiple members (i.e. wings), it defaults to plural.
		public string ShortDescription()
		{
			return shortDesc();
		}

		//a variant of the short description that will always return a single member. it will use formatting specific to a single item. In english, this means it
		//has the correct article (a/an) in front of it.
		public string ShortSingleItemDescription()
		{
			return singleDesc();
		}

		//Constructor for an behavior that only has one member. This format allows a common function to be used for both formats, and a flag to alter the text
		//to match the required type.
		private protected BehaviorBase(ShortDescriptor shortDescFn)
		{
			if (shortDescFn is null) throw new ArgumentNullException(nameof(shortDescFn));
			//doesn't matter, the value is ignored.

			shortDesc = () => shortDescFn(false);
			singleDesc = () => shortDescFn(true);
		}

		//alternate constructor that manually sets the string for the short and single item separately. this is particularly useful for behaviors with multiple members, which would
		//otherwise get really complicated and ugly, not to mention confusing to implement. it's possible to make this one callback with plural members, but it's
		//not really clear how it works and i don't want to force that on anyone lol.
		private protected BehaviorBase(SimpleDescriptor standardShortDesc, SimpleDescriptor singleItemDescriptor)
		{
			shortDesc = standardShortDesc ?? throw new ArgumentNullException(nameof(standardShortDesc));
			singleDesc = singleItemDescriptor ?? throw new ArgumentNullException(nameof(singleItemDescriptor));
		}

		protected static SimpleDescriptor PluralHelper(MaybePluralDescriptor shortDescWithMetaData)
		{
			if (shortDescWithMetaData is null) throw new ArgumentNullException(nameof(shortDescWithMetaData));

			return () => shortDescWithMetaData(out bool _);
		}

		protected static SimpleDescriptor PluralHelper(ShortPluralDescriptor shortDescWithPluralFlag, bool defaultsToPlural = true)
		{
			if (shortDescWithPluralFlag is null) throw new ArgumentNullException(nameof(shortDescWithPluralFlag));

			return () => shortDescWithPluralFlag(defaultsToPlural);
		}

		protected static SimpleDescriptor PluralHelper(ShortMaybePluralDescriptor shortDescWithPluralFlag, bool defaultsToPlural = true)
		{
			if (shortDescWithPluralFlag is null) throw new ArgumentNullException(nameof(shortDescWithPluralFlag));

			return () => shortDescWithPluralFlag(defaultsToPlural, out bool _);
		}

	}
}
