//BodyPartBase.cs
//Description: base class for all body parts. it must have a ruleset attached.
//Author: JustSomeGuy
//12/30/2018, 10:08 PM

using CoC.Backend.BodyParts.EventHelpers;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using System;
using WeakEvent;

namespace CoC.Backend.BodyParts
{
	/*
	 * base class for all body parts. a new instance of this class will exist for every creature.
	 * it is made of two parts: the unique variables that make up this body type (like length for neck)
	 * and the ruleset or behavior that determines how these unique variables change when something happens
	 * for example: when you tell the neck to grow, the behavior will either allow or not allow it, and
	 * change the neck length value accordingly
	 */

	//super confusing generics ftw! Basically, you have to define what it is, and what it's behavior is.
	//TL;DR: BodyPart<[BodyPart], [BodyPartType]>
	//example:
	//internal class Arms : BodyPartBase<Arms, ArmType>
	//internal class ArmType : BodyPartBehavior<ArmType, Arms>


	public abstract class FullBehavioralPart<ThisClass, BehaviorClass, DataClass>
		: BehavioralSaveablePart<ThisClass, BehaviorClass, DataClass>
		where ThisClass : FullBehavioralPart<ThisClass, BehaviorClass, DataClass>
		where BehaviorClass : FullBehavior<BehaviorClass, ThisClass, DataClass>
		where DataClass : FullBehavioralData<DataClass, ThisClass, BehaviorClass>
	{
		public bool isDefault => type == defaultType;
		public abstract BehaviorClass defaultType { get; }

		private protected FullBehavioralPart(Guid creatureID) : base(creatureID)
		{
		}


		//each class may have additional updates for more specific or varied cases, notably cases that use extra variables unique to that class.
		//if this is the case, functions that can change these variables without updating the type may need to be implemented.

		//internal bool Update[Special Name](BehaviorClass type, [additional parameters]);
		//(optional) internal bool Change[Special Name]([additional parameters]);

		internal virtual bool Restore()
		{
			return UpdateType(defaultType);
		}

		//Text output.
		public string LongDescriptionPrimary() => type.LongDescriptionPrimary(AsReadOnlyData());

		public string LongDescriptionAlternate() => type.LongDescriptionAlternate(AsReadOnlyData());

		public string LongDescription(bool alternateFormat = false) => type.LongDescription(AsReadOnlyData(), alternateFormat);


		public string PlayerDescription()
		{
			if (CreatureStore.TryGetCreature(creatureID, out Creature creature) && creature is PlayerBase player)
			{
				return type.PlayerDescription((ThisClass)this, player);
			}
			else return "";
		}
		public string TransformFromText(DataClass previousTypeData)
		{
			if (CreatureStore.TryGetCreature(creatureID, out Creature creature) && creature is PlayerBase player)
			{
				return type.TransformFrom(previousTypeData, player);
			}
			else return "";
		}

		public string RestoredText(DataClass oldData)
		{
			if (CreatureStore.TryGetCreature(creatureID, out Creature creature) && creature is PlayerBase player)
			{
				return oldData.type.RestoredString(oldData, player);
			}
			else return "";
		}
	}
}

