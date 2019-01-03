using CoC.BodyParts;
using CoC.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Strings.BodyParts
{
	public static class ArmsStrings
	{
		public static string HARPYPlayerStr()
		{
			return "Feathers hang off your arms from shoulder to wrist, giving them a slightly wing-like look.";
		}


		public static string WOLFPlayerStr(Arms arms)
		{
			return "Your arms are shaped like a wolf's, overly muscular at your shoulders and biceps before quickly slimming down."
				+ " They're covered in " + arms.furColor() + " fur and end in paws with just enough flexibility to be used as hands."
				+ " They're rather difficult to move in directions besides back and forth.";
		}

		//we have carapace. if you want to make these colorable, we can do that!
		public static string SPIDERPlayerStr(Arms arms) { return arms.epidermis.FullDescript(Tones.BLACK,) + " covers your arms from the biceps down, resembling a pair of long black gloves from a distance."; }
		//
		public static string BEEPlayerStr(Arms arms) { return arms.epidermis.FullDescript(Tones.BLACK) + " covers your arms from the biceps down, resembling a pair of long black gloves that end with a yellow fuzz from a distance."; }

		public static string SALAMANDERPlayerStr(Arms arms) { return arms.epidermis.FullDescript() + "s cover your arms from the biceps down and your fingernails are now " + arms.type.hands.FullDescript(); }

		public static string PREDATORPlayerStr(Arms arms) { return "Your arms are covered by " + arms.epidermis.FullDescript(arms.toneColor()) + " and your fingernails are now " + arms.type.hands.FullDescription() + "."; }

		public static string COCKATRICEPlayerStr(Arms arms)
		{
			return "Your arms are covered in " + arms.furColor() + " feathers"
				+ " from the shoulder down to the elbow where they stop in a fluffy cuff. A handful of long feathers grow from your"
				+ " elbow in the form of vestigial wings, and while they may not let you fly, they certainly help you jump. Your lower"
				+ " arm is coated in leathery " + arms.toneColor() + " scales and your fingertips terminate in deadly looking avian talons.";
		}


		public static string RED_PANDAPlayerStr() { return "Soft, black-brown fluff cover your arms. Your paws have cute, pink paw pads and short claws."; }


		public static string FERRETPlayerStr()
		{
			return "Soft, [hairOrFurColor] fluff covers your arms, turning into"
+ " [if (hasFurryUnderBody)[underBody.furColor]|brown-black] fur from elbows to paws."
+ " The latter have cute, pink paw pads and short claws.";
		}


		public static string DOGPlayerStr()
		{
			return "Soft, [hairOrFurColor] fluff covers your arms. Your paw-like hands have cute, pink paw pads and short claws."
+ " They should assist you walking on all [if (isTaur)sixs|fours]"
+ " just like the hellhounds you saw lurking in the mountains.";
		}

		public static string CatFoxPlayerStr() { return "Soft, [hairOrFurColor] fluff covers your arms. Your paw-like hands have cute, pink paw pads and [claws]."; }
	}
}
