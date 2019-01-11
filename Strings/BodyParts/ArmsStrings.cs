using CoC.BodyParts;
using CoC.EpidermalColors;
using CoC.Tools;
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

		public static string HumanDescStr()
		{

		}
		public static string HumanFullDesc(Arms arm)
		{

		}
		public static string HumanPlayerStr(Arms arm, Player player)
		{

		}
		public static string HumanTransformStr(Arms oldArms, Player player)
		{

		}
		public static string HumanRestoreStr(Arms currentArms, Player player)
		{

		}
		public static string HarpyDescStr()
		{

		}
		public static string HarpyFullDesc(Arms arm)
		{

		}
		public static string HarpyPlayerStr(Arms arm, Player player)
		{

		}
		public static string HarpyTransformStr(Arms oldArms, Player player)
		{

		}
		public static string HarpyRestoreStr(Arms currentArms, Player player)
		{

		}
		public static string SpiderDescStr()
		{

		}
		public static string SpiderFullDesc(Arms arm)
		{

		}
		public static string SpiderPlayerStr(Arms arm, Player player)
		{

		}
		public static string SpiderTransformStr(Arms oldArms, Player player)
		{

		}
		public static string SpiderRestoreStr(Arms currentArms, Player player)
		{

		}
		public static string BeeDescStr()
		{

		}
		public static string BeeFullDesc(Arms arm)
		{

		}
		public static string BeePlayerStr(Arms arm, Player player)
		{

		}
		public static string BeeTransformStr(Arms oldArms, Player player)
		{

		}
		public static string BeeRestoreStr(Arms currentArms, Player player)
		{

		}
		public static string DragonDescStr()
		{

		}
		public static string DragonFullDesc(Arms arm)
		{

		}
		public static string DragonPlayerStr(Arms arm, Player player)
		{

		}
		public static string DragonTransformStr(Arms oldArms, Player player)
		{

		}
		public static string DragonRestoreStr(Arms currentArms, Player player)
		{

		}
		public static string ImpDescStr()
		{

		}
		public static string ImpFullDesc(Arms arm)
		{

		}
		public static string ImpPlayerStr(Arms arm, Player player)
		{

		}
		public static string ImpTransformStr(Arms oldArms, Player player)
		{

		}
		public static string ImpRestoreStr(Arms currentArms, Player player)
		{

		}
		public static string LizardDescStr()
		{

		}
		public static string LizardFullDesc(Arms arm)
		{

		}
		public static string LizardPlayerStr(Arms arm, Player player)
		{

		}
		public static string LizardTransformStr(Arms oldArms, Player player)
		{

		}
		public static string LizardRestoreStr(Arms currentArms, Player player)
		{

		}
		public static string SalamanderDescStr()
		{

		}
		public static string SalamanderFullDesc(Arms arm)
		{

		}
		public static string SalamanderPlayerStr(Arms arm, Player player)
		{

		}
		public static string SalamanderTransformStr(Arms oldArms, Player player)
		{

		}
		public static string SalamanderRestoreStr(Arms currentArms, Player player)
		{

		}
		public static string WolfDescStr()
		{

		}
		public static string WolfFullDesc(Arms arm)
		{

		}
		public static string WolfPlayerStr(Arms arm, Player player)
		{

		}
		public static string WolfTransformStr(Arms oldArms, Player player)
		{

		}
		public static string WolfRestoreStr(Arms currentArms, Player player)
		{

		}
		public static string CockatriceDescStr()
		{

		}
		public static string CockatriceFullDesc(Arms arm)
		{

		}
		public static string CockatricePlayerStr(Arms arm, Player player)
		{

		}
		public static string CockatriceTransformStr(Arms oldArms, Player player)
		{

		}
		public static string CockatriceRestoreStr(Arms currentArms, Player player)
		{

		}
		public static string Red_pandaDescStr()
		{

		}
		public static string Red_PandaFullDesc(Arms arm)
		{

		}
		public static string Red_PandaPlayerStr(Arms arm, Player player)
		{

		}
		public static string Red_PandaTransformStr(Arms oldArms, Player player)
		{

		}
		public static string Red_PandaRestoreStr(Arms currentArms, Player player)
		{

		}
		public static string FerretDescStr()
		{

		}
		public static string FerretFullDesc(Arms arm)
		{

		}
		public static string FerretPlayerStr(Arms arm, Player player)
		{

		}
		public static string FerretTransformStr(Arms oldArms, Player player)
		{

		}
		public static string FerretRestoreStr(Arms currentArms, Player player)
		{

		}
		public static string CatDescStr()
		{

		}
		public static string CatFullDesc(Arms arm)
		{

		}
		public static string CatPlayerStr(Arms arm, Player player)
		{

		}
		public static string CatTransformStr(Arms oldArms, Player player)
		{

		}
		public static string CatRestoreStr(Arms currentArms, Player player)
		{

		}
		public static string DogDescStr()
		{

		}
		public static string DogFullDesc(Arms arm)
		{

		}
		public static string DogPlayerStr(Arms arm, Player player)
		{

		}
		public static string DogTransformStr(Arms oldArms, Player player)
		{

		}
		public static string DogRestoreStr(Arms currentArms, Player player)
		{

		}
		public static string FoxDescStr()
		{

		}
		public static string FoxFullDesc(Arms arm)
		{

		}
		public static string FoxPlayerStr(Arms arm, Player player)
		{

		}
		public static string FoxTransformStr(Arms oldArms, Player player)
		{

		}
		public static string FoxRestoreStr(Arms currentArms, Player player)
		{

		}

	}
}