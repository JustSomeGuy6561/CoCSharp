namespace CoC.Backend.Areas
{
	public abstract class VisitableAreaBase : AreaBase
	{
		private protected VisitableAreaBase(SimpleDescriptor areaName) : base(areaName)
		{
		}

		public abstract bool isUnlocked { get; protected set; }
		public abstract int timesVisited { get; protected internal set; }

		

		internal string Unlock()
		{
			if (!isUnlocked)
			{
				isUnlocked = true;
				timesVisited = 1;
				OnUnlock();
				return UnlockText();
			}
			else
			{
				return null;
			}
		}

		//this function provides you with the ability to do additional actions on an unlock. Internally, this is called before the game calls UnlockText 
		//so if you have unlock text that is conditional, you may update it here. For example, you may gain access to a place either by forcing your way in or 
		//by employing a more diplomatic solution. This will let you print text accordingly.
		protected virtual void OnUnlock() { }

		protected abstract SimpleDescriptor UnlockText { get; }


		public bool Is<T>()
		{
			return this is T;
		}
	}
}
