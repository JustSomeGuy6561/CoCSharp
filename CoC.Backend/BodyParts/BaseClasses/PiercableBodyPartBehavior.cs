//PiercableBodyPartBehavior.cs
//Description:
//Author: JustSomeGuy
//1/1/2019, 9:09 AM


namespace CoC.Backend.BodyParts
{
	//i suppose you could prevent piercings if the body type doesn't support it - dragon ears (which are just slits iirc) might not support earrings for example.
	//but i'm not implementing that. if you want to, you have the option using this class and rewriting the implementations accordingly.
	public abstract class PiercableBodyPartBehavior<ThisClass, ContainerClass, PiercingEnum> : BodyPartBehavior<ThisClass, ContainerClass> where ThisClass : PiercableBodyPartBehavior<ThisClass, ContainerClass, PiercingEnum>
		where ContainerClass : PiercableBodyPart<ContainerClass, ThisClass, PiercingEnum> where PiercingEnum : System.Enum
	{
		protected PiercableBodyPartBehavior(SimpleDescriptor shortDesc, DescriptorWithArg<ContainerClass> fullDesc, TypeAndPlayerDelegate<ContainerClass> playerDesc,
			ChangeType<ContainerClass> transform, RestoreType<ContainerClass> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore) { }
	}
}