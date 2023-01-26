using Dfe.PrepareConversions.DocumentGeneration.Elements;

namespace Dfe.PrepareConversions.DocumentGeneration.Interfaces.Parents
{
    public interface ITextParent
    {
        public void AddText(string text);
        public void AddText(TextElement textElement);
        public void AddText(TextElement[] text);
    }
}