﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDiff.Test.TestObjects
{
    public class GenericDynamicObjectCopy
    {
        public string PublicString;
        public string SecondaryString;
        public double PublicNum;
        public SubObject SubObj;

        public GenericDynamicObjectCopy(
            double num=0.0, 
            string pubString="", 
            string secondString="",
            SubObject subobj=null)
        {
            PublicString = pubString;
            SecondaryString = secondString;
            PublicNum = num;
            SubObj = subobj;
        }
    }
}
