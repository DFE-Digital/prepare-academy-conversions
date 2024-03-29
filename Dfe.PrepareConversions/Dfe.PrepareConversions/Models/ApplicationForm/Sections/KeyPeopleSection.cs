﻿using Dfe.PrepareConversions.Data.Models.AcademisationApplication;
using Dfe.PrepareConversions.Data.Models.Application;
using Dfe.Academisation.ExtensionMethods;
using System.Collections.Generic;
using System.Linq;

namespace Dfe.PrepareConversions.Models.ApplicationForm.Sections
{
   public class KeyPeopleSection : BaseFormSection
   {
      public KeyPeopleSection(Application application) : base("Key people within the trust")
      {
         List<FormSubSection> formSubSections = new();
         foreach (var person in application.KeyPeople)
         {
            formSubSections.Add(new FormSubSection(person.Name, GenerateKeyPeople(person)));
         }

         SubSections = formSubSections.ToArray();
      }

      private static IEnumerable<FormField> GenerateKeyPeople(TrustKeyPerson person)
      {
         List<FormField> formFields = new();

         formFields.Add(new FormField("Position(s) within the trust", string.Join(", ", person.Roles.Select(r => {
            string role = r.Role.ToFirstUpper();
            if (role == "Financialdirector")
            {
               role = "Financial director";
            }
            return role;
         }))));
         formFields.Add(new FormField("Date of birth", person.DateOfBirth.ToDateString()));
         formFields.Add(new FormField("Biography", person.Biography));

         return formFields;
      }
   }
}
