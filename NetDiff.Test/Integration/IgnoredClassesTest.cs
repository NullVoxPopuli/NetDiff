﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NetDiff.Test.Integration
{
    public class IgnoredClassesTest
    {
        [Fact]
        public void AClassIsIgnored()
        {
            var calculator = new DiffCalculator(ignoredClasses: new[] {typeof (double)});
            var a = new List<double> { 1.1, 2.1, 3.1 };
            var b = new List<double> { 5.1, 3.1, 4.1 };

            var result = calculator.Diff(a, b);
            var resultMatch = result.ValuesMatch;
            Assert.True(resultMatch);
        }

        [Fact]
        public void AClassIsNotIgnoredWhenItIsntDiffed()
        {
            var calculator = new DiffCalculator(ignoredClasses: new[] { typeof(decimal) });
            var a = new List<double> { 1.1, 2.1, 3.1 };
            var b = new List<double> { 5.1, 3.1, 4.1 };

            var result = calculator.Diff(a, b);

            Assert.False(result.ValuesMatch);
        }

        [Fact]
        public void NothingIsIgnoredWhenNothingIsSpecified()
        {
            var calculator = new DiffCalculator();
            var a = new List<double> { 1.1, 2.1, 3.1 };
            var b = new List<double> { 5.1, 3.1, 4.1 };

            var result = calculator.Diff(a, b);

            Assert.False(result.ValuesMatch);
        }
    }
}
