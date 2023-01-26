using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces.Parents;

namespace Dfe.PrepareConversions.DocumentGeneration.Interfaces
{
    public interface IParagraphBuilder : ITextParent
    {
        public void AddNewLine();
        public void Justify(ParagraphJustification paragraphJustification);
    }
}