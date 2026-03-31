using Xunit;
using Olav.IntegrationTests.Generation.Fixtures;

namespace Olav.IntegrationTests.Generation.Collections;

[CollectionDefinition("GeneratedProject", DisableParallelization = true)]
public class GeneratedProjectCollection : ICollectionFixture<GeneratedProjectFixture>
{
}
