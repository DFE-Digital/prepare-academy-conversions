using System;

namespace Dfe.PrepareConversions.DocumentGeneration.Interfaces;

public interface ITableBuilder
{
   public void AddRow(Action<ITableRowBuilder> action);
}
