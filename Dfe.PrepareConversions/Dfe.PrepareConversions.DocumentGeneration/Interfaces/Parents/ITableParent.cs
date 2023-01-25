using System;
using System.Collections.Generic;
using Dfe.PrepareConversions.DocumentGeneration.Elements;

namespace Dfe.PrepareConversions.DocumentGeneration.Interfaces.Parents
{
    public interface ITableParent
    {
        public void AddTable(Action<ITableBuilder> action);
        public void AddTable(IEnumerable<TextElement[]> rows);
    }
}