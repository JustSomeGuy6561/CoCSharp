using CoC.Backend.Creatures;
using CoC.Frontend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Perks;
using System;
using System.Linq;

namespace CoC.Frontend.Transformations
{
	internal abstract class TransformationBase<TransformationResult> where TransformationResult : class
	{
		//tfs can be applied to any creature, potentially - don't assume it's the player. but you can always check if the target is a Player object, 
		//and if it is, do Player related things. 
		protected internal abstract TransformationResult DoTransformation(Creature target, byte strength = 1);

		protected int GenerateChanceCount(Creature target, int[] extraRolls = null, int initialCount = 1, int minimumCount = 1)
		{
			initialCount += target.GetExtraData()?.deltaTransforms ?? 0;
			if (extraRolls != null)
			{
				initialCount += extraRolls.Sum(x => Utils.Rand(x) == 0 ? 1 : 0);
			}

			return Math.Max(initialCount, minimumCount);
		}
	}
}
