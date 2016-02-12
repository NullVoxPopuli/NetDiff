﻿using System;
using System.Dynamic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDiff.Test.TestObjects;

namespace NetDiff.Test
{
    [TestClass]
    public class DiffCalculatorTest
    {
        private DiffCalculator _calculator;

        [TestInitialize]
        public void Initialize()
        {
            _calculator = new DiffCalculator();
        }

        [TestMethod]
        public void GetCorrelate_DoesNotPullEqualNameDifferentType()
        {
            var baseObj = new GenericDynamicObject();
            var evaluated = new AlmostGenericDynamicObject();

            var fields = _calculator.GetFields(baseObj);
            var result = _calculator.HasCorrelate(
                field: fields.First(n => n.Name.Equals("SecondaryString")),
                obj: evaluated);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsObjectField_DiscernsObject()
        {
            var baseObj = new GenericDynamicObject(subobj: new SubObject("derr"));

            var fields = _calculator.GetFields(baseObj);
            var result = _calculator.IsObjectField(
                field: fields.First(n => n.Name.Equals("SubObj")),
                obj: baseObj);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Intersect_ProducesAList()
        {
            var result = _calculator.Intersect(
                baseObj: new GenericDynamicObject(), 
                evaluated: new GenericDynamicObject());

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Intersect_ListHasFieldsSet()
        {
            var basePublicString = "This is the base object";
            var evaluatedPublicString = "This is the evaluated object";

            var baseObj = new GenericDynamicObject(
                num: 10.0,
                pubString: basePublicString);

            var evaluatedObject = new GenericDynamicObject(
                num: 0.0,
                pubString: evaluatedPublicString);

            var result = _calculator.Intersect(
                baseObj: baseObj,
                evaluated: evaluatedObject);

            Assert.IsTrue(result.Any(
                  n => string.Equals(n.BaseObjValue, basePublicString)
                    && string.Equals(n.EvaluatedValue, evaluatedPublicString)));
        }

        [TestMethod]
        public void Intersect_ListDisplaysAppropriateNumberOfEqualFields()
        {
            var identicalStrings = "These strings are identical";
            var notIdenticalString = "These strings are not identical";
            var identicalNumber = 0.000089;
            var identicalNumberOffset = 0.000000000000123;

            var baseObj = new GenericDynamicObject(
                num: identicalNumber,
                pubString: identicalStrings,
                secondString: identicalStrings);

            var evaluatedObject = new GenericDynamicObject(
                num: identicalNumber + identicalNumberOffset,
                pubString: identicalStrings,
                secondString: notIdenticalString);

            var result = _calculator.Intersect(
                baseObj: baseObj,
                evaluated: evaluatedObject);

            Assert.AreEqual(
                expected: 3,
                actual: result.Count(n => n.ValuesMatch));

            Assert.AreEqual(
                expected: 1,
                actual: result.Count(n => !n.ValuesMatch));
        }

        [TestMethod]
        public void Intersect_YieldsOnlyCommonFields()
        {
            var identicalStrings = "These strings are identical";
            var identicalNumber = 0.000089;

            var baseObj = new GenericDynamicObject(
                num: identicalNumber,
                pubString: identicalStrings,
                secondString: identicalStrings);

            var evaluatedObject = new SlightlyDifferentObject(
                num: identicalNumber,
                pubString: identicalStrings,
                secondString: identicalStrings);

            var result = _calculator.Intersect(
                baseObj: baseObj,
                evaluated: evaluatedObject);

            Assert.AreEqual(
                expected: 2,
                actual: result.Count);
        }

        [TestMethod]
        public void Intersect_EqualityReflectedAcrossDifferentObjects()
        {
            var identicalStrings = "These strings are identical";
            var notIdenticalString = "These strings are not identical";
            var identicalNumber = 0.000089;
            var identicalNumberOffset = 0.000000000000123;

            var baseObj = new GenericDynamicObject(
                num: identicalNumber,
                pubString: identicalStrings,
                secondString: identicalStrings);

            var evaluatedObject = new SlightlyDifferentObject(
                num: identicalNumber + identicalNumberOffset,
                pubString: notIdenticalString,
                secondString: notIdenticalString);

            var result = _calculator.Intersect(
                baseObj: baseObj,
                evaluated: evaluatedObject);

            Assert.AreEqual(
                expected: 1,
                actual: result.Count(n => n.ValuesMatch));

            Assert.AreEqual(
                expected: 1,
                actual: result.Count(n => !n.ValuesMatch));
        }

        [TestMethod]
        public void GetExclusiveFields_YieldsOnlyExclusiveFields()
        {
            var baseObj = new GenericDynamicObject();
            var evaluatedObject = new SlightlyDifferentObject();

            var result = _calculator.GetExclusiveFields(baseObj, evaluatedObject);

            Assert.IsTrue(result.ToList()
                .Any(n => string.Equals(n.Name, "SecondaryString")));
        }

        [TestMethod]
        public void GetObjectFields_YieldsOnlyObjects()
        {
            var baseObj = new GenericDynamicObject(subobj: new SubObject("An Object"));
            var result = _calculator.GetOjbectFields(baseObj);

            Assert.AreEqual(
                expected: 1,
                actual: result.Count());
        }

        [TestMethod]
        public void GetNonObjectFields_YieldsOnlyNonObjects()
        {
            var baseObj = new GenericDynamicObject(subobj: new SubObject("An Object"));
            var result = _calculator.GetNonOjbectFields(baseObj);

            Assert.AreEqual(
                expected: 3,
                actual: result.Count());
        }

        [TestMethod]
        public void MutuallyExclusive_EqualityReflectedAcrossDifferentObjects()
        {
            var identicalStrings = "These strings are identical";
            var notIdenticalString = "These strings are not identical";
            var identicalNumber = 0.000089;
            var identicalNumberOffset = 0.000000000000123;

            var baseObj = new GenericDynamicObject(
                num: identicalNumber,
                pubString: identicalStrings,
                secondString: identicalStrings);

            var evaluatedObject = new SlightlyDifferentObject(
                num: identicalNumber + identicalNumberOffset,
                pubString: notIdenticalString,
                secondString: notIdenticalString);

            var result = _calculator.MutuallyExclusive(
                baseObj: baseObj,
                evaluated: evaluatedObject);

            Assert.AreEqual(
                expected: 1,
                actual: result.Count(n => n.BaseObjValue != null));

            Assert.AreEqual(
                expected: 1,
                actual: result.Count(n => n.EvaluatedValue != null));
        }
    }
}
