using System;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces.Parents;

namespace Dfe.PrepareConversions.DocumentGeneration.Interfaces
{
    public interface IDocumentBodyBuilder : ITableParent, IParagraphParent
    {
        public void AddHeading(Action<IHeadingBuilder> action);
        public void AddNumberedList(Action<IListBuilder> action);
        public void AddBulletedList(Action<IListBuilder> action);
    }
}