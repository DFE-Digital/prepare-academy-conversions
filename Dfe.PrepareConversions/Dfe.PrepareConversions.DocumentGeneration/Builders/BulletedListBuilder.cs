using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;
using System.Linq;

namespace Dfe.PrepareConversions.DocumentGeneration.Builders;

public class BulletedListBuilder : ListBuilder
{
   public BulletedListBuilder(NumberingDefinitionsPart numberingDefinitionsPart)
   {
      NumberingDefinitionsPart = numberingDefinitionsPart;
      NumId = AddNumberingDefinitions();
   }

   private int AddNumberingDefinitions()
   {
      int numberingDefinitionCount = NumberingDefinitionsPart.Numbering.Descendants<AbstractNum>().Count() + 1;

      AbstractNum abstractNum = new(new Level(
         new List<OpenXmlElement>
         {
            new StartNumberingValue { Val = 1 },
            new NumberingFormat { Val = new EnumValue<NumberFormatValues>(NumberFormatValues.Bullet) },
            new LevelText { Val = "" },
            new LevelJustification { Val = new EnumValue<LevelJustificationValues>(LevelJustificationValues.Left) },
            new ParagraphProperties(new Indentation { Left = "720", Hanging = "360" }),
            new RunProperties(new RunFonts
            {
               Ascii = "Symbol", HighAnsi = "Symbol", ComplexScript = "Symbol", Hint = new EnumValue<FontTypeHintValues>(FontTypeHintValues.Default)
            })
         }
      ) { LevelIndex = 0 }) { AbstractNumberId = numberingDefinitionCount };

      NumberingInstance numberingInstance = new(new AbstractNumId { Val = numberingDefinitionCount }) { NumberID = numberingDefinitionCount };

      AddNumberingDefinitions(abstractNum, numberingInstance);

      return numberingDefinitionCount;
   }
}
