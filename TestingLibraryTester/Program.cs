using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TestingClassLibrary;

namespace TestingLibraryTester
{
	class Program
	{
		static void Main(string[] args)
		{
			SaveSystem.AddSessionSaveInstance(new Tester(17));
			Tester tester = new Tester(17);
			//DataContractSerializer dcs = new DataContractSerializer(typeof(Tester));
			//XmlWriterSettings xws = new XmlWriterSettings()
			//{
			//	Encoding = Encoding.UTF8,
			//	NamespaceHandling = NamespaceHandling.OmitDuplicates,
			//	Indent = true
			//};
			//Console.WriteLine("tester, 17:");
			//using (XmlWriter writer = XmlWriter.Create(Console.Out, xws))
			//{
			//	dcs.WriteObject(writer, tester);
			//}
			//tester.setFoo(34);
			//tester.test3 = new Test4();
			//Console.WriteLine("tester, 34:");
			//using (XmlWriter writer = XmlWriter.Create(Console.Out, xws))
			//{
			//	dcs.WriteObject(writer, tester);
			//}
			//string input = "<?xml version=\"1.0\" encoding=\"IBM437\"?><Tester xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:x=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://schemas.datacontract.org/2004/07/TestingLibraryTester\"><foo i:type=\"x:int\" xmlns=\"\">74</foo></Tester>";
			//string input = "<?xml version=\"1.0\" encoding=\"IBM437\"?><Tester xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:x=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://schemas.datacontract.org/2004/07/TestingLibraryTester\"><theta i:type=\"x:int\" xmlns=\"\">99</theta><foo i:type=\"x:int\" xmlns=\"\">74</foo></Tester>";
			//string input = "<?xml version=\"1.0\" encoding=\"IBM437\"?><Tester xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:x=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://schemas.datacontract.org/2004/07/TestingLibraryTester\"><foo i:type=\"x:int\" xmlns=\"\">34</foo><test3 xmlns:d2p1=\"http://schemas.datacontract.org/2004/07/TestingLibraryTester\" i:type=\"d2p1:Test4\" xmlns=\"\" /></Tester>";
			//string input = "<?xml version=\"1.0\" encoding=\"IBM437\"?><Tester xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:x=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://schemas.datacontract.org/2004/07/TestingLibraryTester\"><foo i:type=\"x:int\" xmlns=\"\">17</foo></Tester>";
			//using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(input)))
			//using (XmlReader reader = XmlReader.Create(ms))
			//{
			//	tester = (Tester)dcs.ReadObject(reader);
			//}
			//Console.WriteLine("Assemblies: ");
			//Array.ForEach(AppDomain.CurrentDomain.GetAssemblies(), Console.WriteLine);
			SaveSystem.SaveSession("Test1.CoCSav");
			Console.WriteLine(SaveSystem.saveDirectory);

			Console.WriteLine();
			//Console.WriteLine(System.Reflection.Assembly.GetExecutingAssembly());
			Console.WriteLine();
		}
	}

	[Serializable]
	class Tester : SaveData2Test
	{
		private int foo;

		internal Test3 test3 = new Test3();

		int theta => 7;

		public Tester(int f) : base()
		{
			foo = f;
		}

		public void setFoo(int f)
		{
			foo = f;
		}

		public Tester(SerializationInfo info, StreamingContext context) : base(info, context) {}

		protected override void ToCurrentVersion(Dictionary<string, SerializationEntry> availableDataToParse)
		{}


		protected override Type[] KnownExternalTypes() { return null; }
	}

	[DataContract]
	class Test3
	{

	}

	[Serializable]
	class Test4 : Test3
	{

	}
}
