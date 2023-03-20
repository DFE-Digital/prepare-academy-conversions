using System;

namespace Dfe.PrepareConversions.DocumentGeneration.Interfaces.Parents;

public interface IParagraphParent
{
   public void AddParagraph(Action<IParagraphBuilder> action);
   public void AddParagraph(string text);
}
