using CoC.Backend;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CoC.Frontend.Strings.Engine;

namespace CoC.Frontend.Engine
{
	public static class InstructionsEtc
	{
		public static readonly ReadOnlyCollection<InstructionItem> instructions;

		static InstructionsEtc()
		{
			List<InstructionItem> items = new List<InstructionItem>()
			{
				new InstructionItem(Instructions.HowToPlayTitle, Instructions.HowToPlayDesc),
				new InstructionItem(Instructions.ExplorationTitle, Instructions.ExplorationDesc),
				new InstructionItem(Instructions.CombatTitle, Instructions.CombatDesc),
				new InstructionItem(Instructions.TipsTitle, Instructions.TipsDesc),
			};

			instructions = new ReadOnlyCollection<InstructionItem>(items);
		}
	}

	public class InstructionItem
	{
		public readonly SimpleDescriptor header;
		public readonly SimpleDescriptor description;

		public InstructionItem(SimpleDescriptor header, SimpleDescriptor description)
		{
			this.header = header ?? throw new ArgumentNullException(nameof(header));
			this.description = description ?? throw new ArgumentNullException(nameof(description));
		}
	}
}

