//IBulgeArmor.cs
//Description: Adds an option interface that an armor can implement. bulge armor perk will check to see if the armor implements this to determine its state.
//Author: JustSomeGuy
//2/7/2020, 12:01 AM
using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Wearables.Armor;

namespace CoC.Frontend.Items.Wearables
{
	//bulge armor is weird, it's a standard perk, because it's activated manually, but it's conditional on having exgartuan and the armor as well.
	//generally, it affects how the item is described, too. You should probably override the remove text as well, and explain how any bulged armor reverts to its normal state
	//when you remove it. i suppose it's possible for armor to remain permanently bulged out, but, uhhh... that's not the default behavior, i guess.
	interface IBulgeArmor
	{
		//does this item support bulge armor?
		bool supportsBulgeArmor { get; }

		//current bulged state of armor. if supportsBulgeArmor is false, this is ignored.
		bool isBulged { get; }

		//attempt to update the armor to match the bulgified state passed in. Note that this will never be called internally if supports bulge armor is false.
		//if it is called, however, it's expected to simply do nothing, and its results can be empty (or you can write some flavor text if you want, i guess).
		//if supports bulge armor is true, however, this updates the armor to match the bulgified state passed in, and then returns any flavor text describing how the
		//armor has changed to comply to your new bulged/unbulged state.
		string SetBulgeState(Creature wearer, bool bulgified);
	}

	internal static class ArmorPerkGenericText
	{
		internal static string GenericBulgeAwareRemoveText<T>(this T item, bool bulgified, string defaultText) where T:ArmorBase, IBulgeArmor
		{
			if (bulgified)
			{
				return "As you remove your " + item.ItemDescription() + ", you notice the modifications that made them hug your crotch and " +
					"prominently display your bulge gradually disappear, as if by magic.";
			}
			else
			{
				return defaultText;
			}
		}
	}
}
