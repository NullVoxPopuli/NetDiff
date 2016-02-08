﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetDiff
{
    public class DiffCalculator
    {
        private double _tolerance;

        public DiffCalculator(double tolerance=1e-6)
        {
            _tolerance = tolerance;
        }

        public ICollection<DiffedItem> Diff(DynamicObject baseObj, DynamicObject evaluated)
        {
            var baseFields = GetObjectFields(baseObj);
            var evaluatedFields = GetObjectFields(evaluated);

            // Check for objects which lie in the intersection
            var intersected = baseFields.Intersect(evaluatedFields, new FieldInfoIntersector());

            var diffed = baseFields.Select(field => new DiffedItem()
            {
                Field = field,
                BaseObjValue = field.GetValue(baseObj),
                EvaluatedValue = field.GetValue(evaluated),
                Tolerance = _tolerance
            });

            return diffed.ToList();
        }

        public FieldInfo[] GetObjectFields(DynamicObject obj)
        {
            return obj.GetType().GetFields(
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance);
        }
    }
}
