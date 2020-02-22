//BodyPartBehavior.cs
//Description: The base class for implementing how a body part behaves.
//Author: JustSomeGuy
//12/30/2018, 10:08 PM

using CoC.Backend.Creatures;
using System;

namespace CoC.Backend.BodyParts
{
	/* Behavior or ruleset base class.
	 * All body parts are made up of two things: a "ruleset", and then variables
	 * such as length or style that are unique to each body part.
	 * the "ruleset" defines how the body part behaves - if/how it grows, if/how it reacts
	 * to changes in hair color or skin tone, etc. The "ruleset" is defined as an
	 * BodyPartBehavior. the bodypart itself is a BodyPart
	 * "rulesets" are just that - they don't change, their internal variables are constant
	 * a "ruleset" accepts a variable and some conditions, then changes that variable based
	 * on what rules it has and what the conditions are.
	 * For Example:
	 * you have a dragon neck, and want to make it grow.
	 * 1) You call your neck's growth function, passing in your current length by reference
	 * 2) the the dragon neck behavior takes your value, and determines how to change it.
	 * 3) it then sets your neck length, and returns.
	 */

	public abstract class FullBehavior<ThisClass, ContainerClass, DataClass> : BehaviorBase
		where ThisClass : FullBehavior<ThisClass, ContainerClass, DataClass>
		where ContainerClass : FullBehavioralPart<ContainerClass, ThisClass, DataClass>
		where DataClass : FullBehavioralData<DataClass, ContainerClass, ThisClass>
	{


		//Function pointers. Wooo!
		//but they make the code significantly shorter and at the same time way more
		//functional. with these, it's possible (and easier) to support language packs.

		//A long description is a longer, more verbose form of a short description. However, this also means it's more likely to mess up grammar and sound weird, notably when used with
		//an article (for example, "you have 'a' no hair" (bad) versus "you have no hair" (ok) or "you have a bald head" (ok) vs "you have bald head" (bad). This gets even worse when
		//using different sentence structures (like ending in $"your {LongDescription()}"). in above example, "your bald head" is ok, but "your no hair" isn't.
		//to handle this, each long description has an alternate form flag. It's not 100% perfect, but it's good enough for most text.



		//In english, this generally means the first form will be without an article and work with "your ...", while the second will have an article and work with "you have...".
		//note that it's possible to have both versions return the same text, if the text works with both "your..." and "you have...". Use whichever makes the most sense.

		//This also works for non-English languages (if ever implemented), though what the alternate form entails in a given language may vary.
		private readonly PartDescriptor<DataClass> longStr;
		public string LongDescriptionPrimary(DataClass data)
		{
			return longStr(data);
		}

		public string LongDescriptionAlternate(DataClass data)
		{
			return longStr(data, true);
		}

		public string LongDescription(DataClass data, bool alternateFormat)
		{
			return longStr(data, alternateFormat);
		}

		//there are a few rare cases where the body part may be plural - a pair of wings or a pair of antennae, for example. It may be desired to have a single version of those, like
		//"your left wing"

		//to handle this, a simple plural descriptor and long plural descriptor delegate are provided. This is implemented on a case-by-case basis. The exact format and implementation
		//may vary between classes; be aware of what it is for the class you are using.


		//a full description of this part, with flavor text. it will be called whenever the player asks for their description. It generally will be entirely standalone, and
		//is expected to be a full sentence.
		private readonly PlayerBodyPartDelegate<ContainerClass> playerStr;
		public string PlayerDescription(ContainerClass source, PlayerBase player)
		{
			return playerStr(source, player);
		}

		//transform from: take old data, which was the previous settings. new data can be obtained from player if needed.
		//called on the behavior we transformed into.
		private readonly ChangeType<DataClass> transformFromStr;
		public string TransformFrom(DataClass oldData, PlayerBase player)
		{
			if (oldData is null) return "";

			return transformFromStr(oldData, player);
		}

		//this is called on the behavior we transformed from.
		//requires any old data. it should know how it restores the old data based on what it does internally, but if needed it can just get
		//any new data from hte player passed in.
		private readonly RestoreType<DataClass> restoredStr;
		public string RestoredString(DataClass originalData, PlayerBase player)
		{
			if (originalData is null) return "";

			return restoredStr(originalData, player);
		}

		private protected FullBehavior(ShortDescriptor shortDesc, PartDescriptor<DataClass> longDesc,
			PlayerBodyPartDelegate<ContainerClass> playerDesc, ChangeType<DataClass> transformDesc, RestoreType<DataClass> restoreDesc) : base(shortDesc)
		{
			longStr = longDesc ?? throw new ArgumentNullException(nameof(longDesc));
			playerStr = playerDesc ?? throw new ArgumentNullException(nameof(playerDesc));
			transformFromStr = transformDesc ?? throw new ArgumentNullException(nameof(transformDesc));
			restoredStr = restoreDesc ?? throw new ArgumentNullException(nameof(restoreDesc));
		}

		private protected FullBehavior(SimpleDescriptor standardShortDesc, SimpleDescriptor singleShortDesc, PartDescriptor<DataClass> longDesc,
			PlayerBodyPartDelegate<ContainerClass> playerDesc, ChangeType<DataClass> transformDesc, RestoreType<DataClass> restoreDesc) : base(standardShortDesc, singleShortDesc)
		{
			longStr = longDesc ?? throw new ArgumentNullException(nameof(longDesc));
			playerStr = playerDesc ?? throw new ArgumentNullException(nameof(playerDesc));
			transformFromStr = transformDesc ?? throw new ArgumentNullException(nameof(transformDesc));
			restoredStr = restoreDesc ?? throw new ArgumentNullException(nameof(restoreDesc));
		}

		protected static PartDescriptor<T> LongPluralHelper<T>(PluralPartDescriptor<T> longDescWithPluralFlag, bool defaultsToPlural = true)
		{
			if (longDescWithPluralFlag is null) throw new ArgumentNullException(nameof(longDescWithPluralFlag));

			return (arg, alternate) => longDescWithPluralFlag(arg, alternate, defaultsToPlural);
		}

		protected static PartDescriptor<T> LongPluralHelper<T>(MaybePluralPartDescriptor<T> longDescWithPluralFlag, bool defaultsToPlural = true)
		{
			if (longDescWithPluralFlag is null) throw new ArgumentNullException(nameof(longDescWithPluralFlag));

			return (arg, alternate) => longDescWithPluralFlag(arg, alternate, defaultsToPlural, out bool _);
		}

	}
}
