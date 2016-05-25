using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDiff.Test.TestObjects
{
    public class ObjectWithReadOnlyCollectionProperty
    {
        public IReadOnlyCollection<ObjectWithPrivateMembers> Collection { get; set; } 
    }
}
