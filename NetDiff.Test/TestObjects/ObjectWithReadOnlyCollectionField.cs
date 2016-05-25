using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDiff.Test.TestObjects
{
    public class ObjectWithReadOnlyCollectionField
    {
        public IReadOnlyCollection<ObjectWithPrivateMembers> Collection;
    }
}
