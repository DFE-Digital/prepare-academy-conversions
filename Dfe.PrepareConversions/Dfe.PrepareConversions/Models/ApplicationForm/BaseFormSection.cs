using System.Collections.Generic;
using System.Linq;

namespace Dfe.PrepareConversions.Models.ApplicationForm;

public abstract class BaseFormSection : FormSubSection
{
   protected BaseFormSection(string heading, IEnumerable<FormField> fields) : base(heading, fields)
   {
      SubSections = Enumerable.Empty<FormSubSection>();
   }

   protected BaseFormSection(string heading) : this(heading, Enumerable.Empty<FormField>()) { }

   public IEnumerable<FormSubSection> SubSections { get; protected set; }
}
