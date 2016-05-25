using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetDiff.Extensions;
using NetDiff.Model;
using NetDiff.Test.TestObjects;
using Xunit;

namespace NetDiff.Test.Unit.DiffCalculator
{
    public class DiffObjectsTest
    {

        [Fact]
        public void ObjectsMayBeOfDifferentType()
        {
            var baseObject = 2;
            var antagonist = "hi";

            var result = baseObject.DiffAgainst(antagonist);

            Assert.Equal(false, result.ValuesMatch);
        }

        [Fact]
        public void AntagonistIsNotIterable()
        {
            var baseObject = new List<int> {1, 2, 3};
            var antagonist = 2;

            var result = baseObject.DiffAgainst(antagonist);

            Assert.Equal(DiffMessage.DiffersInType, result.Message);
        }

        [Fact]
        public void PrivateFieldsShouldNotBeComparedByDefault()
        {
            var baseObject =
                new ObjectWithPrivateMembers(
                    someIntField: 1,
                    somePrivateIntField: 2,
                    somePrivateIntProperty: 2);

            var antagonist = new ObjectWithPrivateMembers(
                someIntField: 1,
                somePrivateIntField: 3,
                somePrivateIntProperty: 3);

            var result = baseObject.DiffAgainst(antagonist);

            Assert.Equal(true, result.ValuesMatch);
        }


        [Fact]
        public void PrivateFieldsShouldNotBeComparedWithReadOnlyCollections()
        {
            //ReadOnlyCollections will throw exceptions on accessing _syncRoot
            var baseCollection = new List<ObjectWithPrivateMembers>()
            {
                new ObjectWithPrivateMembers(
                    someIntField: 1,
                    somePrivateIntField: 2,
                    somePrivateIntProperty: 2),
                new ObjectWithPrivateMembers(
                    someIntField: 1,
                    somePrivateIntField: 3,
                    somePrivateIntProperty: 3)
            };

            var antagonistCollection = new List<ObjectWithPrivateMembers>()
            {
                new ObjectWithPrivateMembers(
                    someIntField:1,
                    somePrivateIntField:2,
                    somePrivateIntProperty:4),
                new ObjectWithPrivateMembers(
                    someIntField:1,
                    somePrivateIntField:3,
                    somePrivateIntProperty:5)
            };

            var baseObject = new ObjectWithReadOnlyCollectionField() {Collection = baseCollection.AsReadOnly()};
            var antagonist = new ObjectWithReadOnlyCollectionField() {Collection = antagonistCollection.AsReadOnly()};

            var calc = new NetDiff.DiffCalculator();

            var result = calc.Diff(baseObject, antagonist);

            Assert.Equal(true, result.ValuesMatch);
        }
    }
}
