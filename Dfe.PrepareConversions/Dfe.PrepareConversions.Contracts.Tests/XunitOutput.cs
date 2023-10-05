using PactNet.Infrastructure.Outputters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Dfe.PrepareConversions.Contracts.Tests
{
   internal class XunitOutput : IOutput
   {
      private readonly ITestOutputHelper _output;

      public XunitOutput(ITestOutputHelper output)
      {
         _output = output;
      }


      public void WriteLine(string line)
      {
         _output.WriteLine(line);
      }
   }
}
