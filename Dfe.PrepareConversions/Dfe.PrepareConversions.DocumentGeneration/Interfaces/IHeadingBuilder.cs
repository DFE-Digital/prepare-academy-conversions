using Dfe.PrepareConversions.DocumentGeneration.Elements;

namespace Dfe.PrepareConversions.DocumentGeneration.Interfaces
{
    public interface IHeadingBuilder
    {
        public void SetHeadingLevel(HeadingLevel level);
        public void AddText(string text);
        public void AddText(TextElement text);
    }
}