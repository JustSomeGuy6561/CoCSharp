//AttackBase.cs
//Description:
//Author: JustSomeGuy
//4/28/2019, 10:05 PM
using CoC.Backend.Creatures;

namespace CoC.Backend.Attacks
{
	//attacks are going to be initialized each time they are attached to something. the simple fact of the matter is some of them require resources, and
	//that can't be static. the extra resource cost is small, especially considering how few enemies have them and how (relatively) infrequent combat is.
	//Generally speaking, only two combat creatures exist at any given time (PC and Enemy), so who cares? also, if attached to a body part behavior, only one ever exists.
	//correction, 3, if doing the Urta quest (the PC still exists). still, not worth the worries about overhead.

	//an aside: the same could be said about body parts, but body parts are created as such to implement a separation of concerns:

	public abstract class AttackBase
	{
		//libido affects lust damage.
		//toughness affects physical damage.
		//intellect affects magic damage.
		//speed affects evasion.
		//sensitivity: affects physical sexual stimulation. generally not in combat, but some attacks may use it.
		//libido: affects general lust generation over time, and any lust damage obtained during combat.
		//corruption: affects lust gain over time, and allows certain interactions or attacks. some combat abilities may be affected by corruption as well.

		//lust: causes combat loss if maxed.
		//hp: causes combat loss if drops to 0.

		//
		//Range stats: close, medium, far. knockback causes player to reach medium level.
		//


		//hit rate - should generally factor in attacker's speed, defender's evade, but can be overridden to ignore if needed.
		//crit rate - physical only? idk.
		//range type - ranged, melee?
		//attack type - physical, magical, mixed?
		//damage type - lust, hp, both.
		//on hit text - damage dealt, critical hit
		//generic on miss text

		//attack, tease, physical special, magical special. special attacks assume a 100% hitrate, though you can override this if necessary.
		//attack takes weapon attack, attack rate, etc.


		protected abstract bool CanUseAttack(CombatCreature attacker, CombatCreature defender);

		protected abstract SimpleDescriptor DoAttack(CombatCreature attacker, CombatCreature defender);

		protected readonly SimpleDescriptor attackName;
		protected abstract DescriptorWithArg<CombatCreature> AttackDescription();

		public AttackBase(SimpleDescriptor name)
		{
			attackName = name;
		}

		public static readonly AttackBase NO_ATTACK = NoAttack.instance;

		protected abstract bool isPhysical { get; }

	}



}
