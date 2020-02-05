using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Perks;
using CoC.Frontend.Perks.SpeciesPerks;
using System.Text;

namespace CoC.Frontend.Transformations
{
	internal abstract class GhostTransformations : GenericTransformationBase
	{
		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;

			int changeCount = GenerateChangeCount(target, new int[] { 2, 2 });
			int remainingChanges = changeCount;

			StringBuilder sb = new StringBuilder();

			sb.Append(InitialTransformationText(target));

			//No cost stat changes.

			//Effect script 1:  (higher intelligence)
			if (target is CombatCreature cc && cc.relativeIntelligence < 100 && Utils.Rand(3) == 0)
			{
				cc.IncreaseIntelligence(1);
				if (cc.relativeIntelligence < 50) cc.IncreaseIntelligence(1);
			}
			//Effect script 2:  (lower sensitivity)
			if (target.relativeSensitivity >= 20 && Utils.Rand(3) == 0)
			{
				target.DeltaCreatureStats(sens: -2);
				if (target.relativeSensitivity >= 75) target.DeltaCreatureStats(sens: -2);
			}
			//Effect script 3:  (higher libido)
			if (target.relativeLibido < 100 && Utils.Rand(3) == 0)
			{
				target.DeltaCreatureStats(lib: 1);
				if (target.relativeLibido < 50) target.DeltaCreatureStats(lib: 1);
			}

			//this will handle the edge case where the change count starts out as 0.
			if (remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);

			//Effect script a:  (human wang)
			if (target.hasCock && target.cocks[0].type != CockType.defaultValue)
			{
				//sb.Append("\n\nA strange tingling begins behind your " + target.cockDescript(0) + ", slowly crawling up across its entire length.  While neither particularly arousing nor uncomfortable, you do shift nervously as the feeling intensifies.  You resist the urge to undo your " + target.armorName + " to check, but by the feel of it, your penis is shifting form.  Eventually the transformative sensation fades, <b>leaving you with a completely human penis.</b>");
				target.genitals.RestoreCock(0);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Neck restore
			if (!target.neck.isDefault && Utils.Rand(4) == 0)
			{
				target.RestoreNeck();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);

			}
			//Rear body restore
			if (!target.back.isDefault && Utils.Rand(5) == 0)
			{
				target.RestoreBack();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Ovi perk loss
			if (target.womb.canRemoveOviposition && Utils.Rand(5) == 0)
			{
				target.womb.ClearOviposition();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Appearnace Change
			//Hair
			if (Utils.Rand(4) == 0 && !target.hair.isSemiTransparent)
			{
				//this isn't a type anymore, so you can apply it to all types. it's now just a flag.

				//sb.Append("\n\nA sensation of weightlessness assaults your scalp. You reach up and grab a handful of hair, confused.
				//Your perplexion only heightens when you actually feel the follicles becoming lighter in your grasp, before you can hardly tell you're holding anything.
				//Plucking a strand, you hold it up before you, surprised to see... it's completely transparent!  You have transparent hair!");
				target.hair.SetTransparency(true);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Skin
			if (Utils.Rand(4) == 0 && target.body.type != BodyType.HUMANOID || (target.body.mainEpidermis.tone != Tones.SABLE && target.body.mainEpidermis.tone != Tones.WHITE &&
				target.body.mainEpidermis.tone != Tones.MILKY_WHITE))
			{
				Tones tone;
				Tones veins;

				if (Utils.RandBool())
				{
					tone = Tones.MILKY_WHITE;
					veins = Tones.BLACK;
				}
				else
				{
					tone = Tones.SABLE;
					veins = Tones.WHITE;
				}

				target.UpdateBody(BodyType.HUMANOID, tone);

				//sb.Append("\n\nA warmth begins in your belly, slowly spreading through your torso and appendages. The heat builds, becoming uncomfortable, then painful, then nearly unbearable. Your eyes unfocus from the pain, and by the time the burning sensation fades, you can already tell something's changed. You raise a hand, staring at the {tone} flesh. Your eyes are drawn to the veins in the back of your {hand}, {(tone == MILKY_WHITE ? "brightening to an ashen tone" : "darkening to a jet black")} as you watch.  <b>You have {tone} skin, with {vein} veins!</b>");

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			if (!target.HasPerk<Incorporeal>() && target.hair.isSemiTransparent &&
				(target.body.mainEpidermis.tone == Tones.SABLE || target.body.mainEpidermis.tone == Tones.WHITE || target.body.mainEpidermis.tone == Tones.MILKY_WHITE))
			{
				//(ghost-legs!  Absolutely no problem with regular encounters, though! [if you somehow got this with a centaur it'd probably do nothing cuz you're not supposed to be a centaur with ectoplasm ya dingus])
				//sb.Append("\n\nAn otherworldly sensation begins in your belly, working its way to your [hips]. Before you can react, your [legs]"
				//+ " begin to tingle, and you fall on your rump as a large shudder runs through them. As you watch, your lower body shimmers,"
				//+ " becoming ethereal, wisps rising from the newly ghost-like [legs]. You manage to rise, surprised to find your new,"
				//+ " ghostly form to be as sturdy as its former corporeal version. Suddenly, like a dam breaking,"
				//+ " fleeting visions and images flow into your head, never lasting long enough for you to concentrate on one."
				//+ " You don't even realize it, but your arms fly up to your head, grasping your temples as you groan in pain."
				//+ " As fast as the mental bombardment came, it disappears, leaving you with a surprising sense of spiritual superiority."
				//+ "  <b>You have ghost legs!</b>\n\n");
				//sb.Append("<b>(Gained Perk:  Incorporeality</b>)");
				target.perks.AddPerk<Incorporeal>();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Effect Script 8: 100% chance of healing
			if (remainingChanges == changeCount && target is CombatCreature cc2)
			{
				//sb.Append("You feel strangely refreshed, as if you just gobbled down a bottle of sunshine.  A smile graces your lips as vitality fills you.  ");
				cc2.AddHP((uint)cc2.level * 5 + 10);
			}
			//Incorporeality Perk Text:  You seem to have inherited some of the spiritual powers of the residents of the afterlife!  While you wouldn't consider doing it for long due to its instability, you can temporarily become incorporeal for the sake of taking over enemies and giving them a taste of ghostly libido.

			//Sample possession text (>79 int, perhaps?):  With a smile and a wink, your form becomes completely intangible, and you waste no time in throwing yourself into your opponent's frame. Before they can regain the initiative, you take control of one of their arms, vigorously masturbating for several seconds before you're finally thrown out. Recorporealizing, you notice your enemy's blush, and know your efforts were somewhat successful.
			//Failure:  With a smile and a wink, your form becomes completely intangible, and you waste no time in throwing yourself into the opponent's frame. Unfortunately, it seems they were more mentally prepared than you hoped, and you're summarily thrown out of their body before you're even able to have fun with them. Darn, you muse. Gotta get smarter.

			//this is the fallthrough that occurs when a tf item goes through all the changes, but does not proc enough of them to exit early. it will apply however many changes
			//occurred, then return the contents of the stringbuilder.
			return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
		}


		//the abstract string calls that you create above should be declared here. they should be protected. if it is a body part change or a generic text that has already been
		//defined by the base class, feel free to make it virtual instead.
		protected abstract bool InitialTransformationText(Creature target);
	}
}