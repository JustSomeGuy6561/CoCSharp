//ResourceAttackBase.cs
//Description:
//Author: JustSomeGuy
//4/28/2019, 10:05 PM

using System;

namespace CoC.Backend.Attacks
{
	public abstract class ResourceAttackBase : AttackBase
	{
		public readonly ushort maxResource;
		public readonly ushort maxRechargeRate;
		public readonly ushort initialResource;
		public readonly ushort initialRechargeRate;

		protected ushort resourceCount
		{
			get => getResources();
			set => setResources(value);
		}
		private readonly Func<ushort> getResources;
		private readonly Action<ushort> setResources;

		protected ResourceAttackBase(ushort maxResource, ushort maxRechargeRate, ushort initialResource, ushort initialRechargeRate, Func<ushort> getResourceCount, Action<ushort> setResourceCount,
			SimpleDescriptor attackName) : base(attackName)
		{
			this.maxResource = maxResource;
			this.maxRechargeRate = maxRechargeRate;
			this.initialResource = initialResource;
			this.initialRechargeRate = initialRechargeRate;
			getResources = getResourceCount;
			setResources = setResourceCount;
		}
	}
}
