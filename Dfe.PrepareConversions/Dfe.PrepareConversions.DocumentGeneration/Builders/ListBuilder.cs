using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dfe.PrepareConversions.DocumentGeneration.Builders;

public abstract class ListBuilder : IListBuilder, IElementBuilder<List<Paragraph>>
{
   private readonly List<Paragraph> _items;
   protected NumberingDefinitionsPart NumberingDefinitionsPart;
   protected int NumId;

   protected ListBuilder()
   {
      _items = new List<Paragraph>();
   }

   public List<Paragraph> Build()
   {
      return _items;
   }

   public void AddItem(Action<IParagraphBuilder> action)
   {
      Paragraph paragraph = new()
      {
         ParagraphProperties = new ParagraphProperties
         {
            NumberingProperties = new NumberingProperties(new List<OpenXmlElement> { new NumberingLevelReference { Val = 0 }, new NumberingId { Val = NumId } })
         }
      };
      ParagraphBuilder paragraphBuilder = new(paragraph);
      action(paragraphBuilder);
      _items.Add(paragraphBuilder.Build());
   }

   public void AddItem(string item)
   {
      AddItem(new TextElement(item));
   }


   public void AddItem(TextElement item)
   {
      Paragraph paragraph = new()
      {
         ParagraphProperties = new ParagraphProperties
         {
            NumberingProperties = new NumberingProperties(new List<OpenXmlElement> { new NumberingLevelReference { Val = 0 }, new NumberingId { Val = NumId } })
         }
      };
      ParagraphBuilder paragraphBuilder = new(paragraph);
      paragraphBuilder.AddText(item);
      _items.Add(paragraphBuilder.Build());
   }

   public void AddItem(TextElement[] elements)
   {
      Paragraph paragraph = new()
      {
         ParagraphProperties = new ParagraphProperties
         {
            NumberingProperties = new NumberingProperties(new List<OpenXmlElement> { new NumberingLevelReference { Val = 0 }, new NumberingId { Val = NumId } })
         }
      };
      ParagraphBuilder paragraphBuilder = new(paragraph);
      paragraphBuilder.AddText(elements);
      _items.Add(paragraphBuilder.Build());
   }

   protected void AddNumberingDefinitions(AbstractNum abstractNum, NumberingInstance numberingInstance)
   {
      int numberingDefinitionCount = NumberingDefinitionsPart.Numbering.Descendants<AbstractNum>().Count();

      if (numberingDefinitionCount > 0)
      {
         AbstractNum lastAbstract = NumberingDefinitionsPart.Numbering.Descendants<AbstractNum>().Last();
         NumberingDefinitionsPart.Numbering.InsertAfter(abstractNum, lastAbstract);

         NumberingInstance lastNumberingInstance = NumberingDefinitionsPart.Numbering.Descendants<NumberingInstance>().Last();
         NumberingDefinitionsPart.Numbering.InsertAfter(numberingInstance, lastNumberingInstance);
      }
      else
      {
         NumberingDefinitionsPart.Numbering.AppendChild(abstractNum);
         NumberingDefinitionsPart.Numbering.AppendChild(numberingInstance);
      }
   }
}
