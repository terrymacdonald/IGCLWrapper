using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using Xunit;
using Xunit.Abstractions;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
[assembly: TestCollectionOrderer("IGCLWrapper.FacadeTests.ActiveTestCollectionOrderer", "IGCLWrapper.FacadeTests")]

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    [CollectionDefinition("Passive", DisableParallelization = true)]
    public class PassiveCollection
    {
    }

    [SupportedOSPlatform("windows")]
    [CollectionDefinition("ActiveDisplay", DisableParallelization = true)]
    public class ActiveDisplayCollection
    {
    }

    [SupportedOSPlatform("windows")]
    [CollectionDefinition("ActiveCombined", DisableParallelization = true)]
    public class ActiveCombinedCollection
    {
    }

    public sealed class ActiveTestCollectionOrderer : ITestCollectionOrderer
    {
        private static readonly Dictionary<string, int> CollectionOrder = new(StringComparer.Ordinal)
        {
            ["Passive"] = 0,
            ["ActiveDisplay"] = 1,
            ["ActiveCombined"] = 2
        };

        public IEnumerable<ITestCollection> OrderTestCollections(IEnumerable<ITestCollection> testCollections)
        {
            return testCollections
                .OrderBy(collection => GetOrder(collection.DisplayName))
                .ThenBy(collection => collection.DisplayName, StringComparer.Ordinal);
        }

        private static int GetOrder(string displayName)
        {
            foreach (var entry in CollectionOrder)
            {
                if (displayName.Contains(entry.Key, StringComparison.Ordinal))
                    return entry.Value;
            }

            return int.MaxValue;
        }
    }
}
