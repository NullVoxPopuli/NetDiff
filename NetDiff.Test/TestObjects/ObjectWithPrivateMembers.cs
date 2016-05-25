using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDiff.Test.TestObjects
{
    public class ObjectWithPrivateMembers
    {
        public int SomeIntField;

        private int _somePrivateIntField;
        private int _somePrivateIntProperty { get; set; }

        public ObjectWithPrivateMembers(int someIntField, int somePrivateIntField, int somePrivateIntProperty)
        {
            SomeIntField = someIntField;
            _somePrivateIntField = somePrivateIntField;
            _somePrivateIntProperty = somePrivateIntProperty;
        }
    }
}
