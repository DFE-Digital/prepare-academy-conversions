using System;

namespace DocumentGeneration;

[AttributeUsage(AttributeTargets.Property)]
public class DocumentTextAttribute : Attribute
{
   private readonly string _placeholder;

   public DocumentTextAttribute(string placeholder)
   {
      _placeholder = placeholder;
   }

   public string Placeholder => $"[{_placeholder}]";

   public bool IsRichText { get; set; }
}
