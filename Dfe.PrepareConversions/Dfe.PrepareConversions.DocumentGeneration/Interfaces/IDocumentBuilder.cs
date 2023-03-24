using Dfe.PrepareConversions.DocumentGeneration.Builders;
using DocumentFormat.OpenXml.Packaging;
using System;

namespace Dfe.PrepareConversions.DocumentGeneration.Interfaces;

public interface IDocumentBuilder : IDocumentBodyBuilder
{
   public void ReplacePlaceholderWithContent(string placeholderText, Action<DocumentBodyBuilder> action);
   public void AddHeader(Action<IHeaderBuilder> action);
   public void AddFooter(Action<IFooterBuilder> action);
   public WordprocessingDocument GetCurrentDocument();
   public byte[] Build();
}
