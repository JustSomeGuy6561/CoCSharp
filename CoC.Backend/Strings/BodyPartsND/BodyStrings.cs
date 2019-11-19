//BodyStrings.cs
//Description:
//Author: JustSomeGuy
//1/4/2019, 8:22 PM
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;

namespace CoC.Backend.BodyParts
{

	public partial class Body
	{
		public static string Name()
		{
			return "Body";
		}

		private string MultiDye(HairFurColors dyeColor, HashSet<byte> indices)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private string SingleDye(HairFurColors dyeColor, byte index)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private string MultiLotions(SkinTexture lotionTexture, HashSet<byte> indices)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private string SingleLotion(SkinTexture lotionTexture, byte index)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		public bool IsFurry()
		{
			return type.epidermisType.usesFur;
		}
	}

	public partial class BodyType
	{
		#region Generics

		protected static string BodyDesc()
		{
			return "Body";
		}

		protected static string Body2Desc()
		{
			return "Body 2";
		}

		protected static string YourBodyDesc()
		{
			return " your body";
		}

		protected static string PartsOfFurPatternDesc()
		{
			return " parts of your fur to form a pattern";
		}

		protected static string UnderBodyDesc()
		{
			return "Underbody";
		}

		protected static string YourUnderBodyDesc()
		{
			return " your underbody";
		}

		protected static SimpleDescriptor YourDescriptor(EpidermisType epidermisType)
		{
			return () => YourDesc(epidermisType);
		}

		private static string YourDesc(EpidermisType epidermisType)
		{
			return " your " + epidermisType.shortDescription;
		}
		#endregion
		//primary fur: fur
		//secondary fur: fur on your underbody

		//but during apply land, it's fur|feathers on your underside. TY GAME!

		//primary tone: body
		//secondary tone: underbody

		//Apply <dye color> dye to the <this function>?
		//The <this function> is aleady <dye color>.
		//You applied the dye to <this function>. It is now <dye color>
		private string BodyDyeDesc()
		{
			//if (this.)
			return "the " + epidermisType.shortDescription() + " covering your body";
		}

		private string BodyToneDesc()
		{
			return "your " + secondaryEpidermisType.shortDescription();
		}


		private string UnderBodyDyeDesc()
		{
			return secondaryEpidermisType.shortDescription() + " covering your underbody";
		}

		//apply <tone color> lotion to <this function>?
		//
		private string UnderBodyToneDesc()
		{
			return "the " + secondaryEpidermisType.shortDescription() + " on your underside";
		}

		#region Skin
		private static string SkinDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SkinLongDesc(Body body)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SkinPlayerStr(Body body, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SkinTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SkinRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Scales
		private static string ScalesDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ScalesUnderbodyDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ScalesLongDesc(Body body)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ScalesPlayerStr(Body body, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ScalesTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ScalesRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string YourUnderScalesDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Naga
		private static string NagaDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NagaUnderbodyDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NagaLongDesc(Body body)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NagaPlayerStr(Body body, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NagaTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NagaRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string YourUnderNagaDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Cockatrice
		protected static string CockatriceDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		protected static string CockatriceUnderbodyDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		protected static string CockatriceLongDesc(Body body)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		protected static string CockatricePlayerStr(Body body, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		protected static string CockatriceTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		protected static string CockatriceRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Kitsune
		protected static string KitsuneDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		protected static string KitsuneUnderbodyDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		protected static string KitsuneLongDesc(Body body)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		protected static string KitsunePlayerStr(Body body, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		protected static string KitsuneTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		protected static string KitsuneRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		//the feathers|fur covering parts of your body.
		protected static string PartialDyeDesc(EpidermisType epidermis)
		{
			return "the" + epidermis.shortDescription() + "covering most of your body";
		}


		#endregion
		#region Bark
		private static string BarkDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BarkLongDesc(Body body)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BarkPlayerStr(Body body, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BarkTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BarkRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Fur
		private static string FurDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FurUnderbodyDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FurLongDesc(Body body)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FurPlayerStr(Body body, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FurTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FurRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string YourUnderFurDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Feathers
		private static string FeatherDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string UnderFeatherDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FeatherLongDesc(Body body)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FeatherPlayerStr(Body body, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FeatherTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FeatherRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string YourUnderFeatherDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Wool
		private static string WoolDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WoolUnderbodyDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WoolLongDesc(Body body)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WoolPlayerStr(Body body, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WoolTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WoolRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string YourUnderWoolDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Goo
		private static string GooDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GooLongDesc(Body body)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GooPlayerStr(Body body, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GooTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GooRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Rubber
		private static string RubberDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RubberLongDesc(Body body)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RubberPlayerStr(Body body, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RubberTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RubberRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Carapace
		private static string CarapaceStr()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CarapaceLongDesc(Body body)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CarapacePlayerStr(Body body, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CarapaceTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CarapaceRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Exoskeleton
		private static string ExoskeletonStr()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ExoskeletonLongDesc(Body body)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ExoskeletonPlayerStr(Body body, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ExoskeletonTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ExoskeletonRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
	}

}
