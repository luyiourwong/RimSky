using System.IO;
using NUnit.Framework;

namespace RimSky.Tests
{
    [TestFixture]
    public class RimSkySourceTests
    {
        private static string FindFileUpwards(string startingDirectory, string targetFileName, int maxDepth = 10)
        {
            var current = new DirectoryInfo(startingDirectory);
            for (int i = 0; i < maxDepth && current != null; i++)
            {
                var candidate = Path.Combine(current.FullName, targetFileName);
                if (File.Exists(candidate))
                    return candidate;

                current = current.Parent;
            }
            return null;
        }

        [Test]
        public void RimSky_ShouldLogHelloWorld_OnConstruction_SourceContainsLogMessage()
        {
            // Arrange
            var testDir = TestContext.CurrentContext.TestDirectory;
            var rimSkySourcePath = FindFileUpwards(testDir, "RimSky.cs", maxDepth: 12);

            // Assert that we can find the source file
            Assert.IsNotNull(rimSkySourcePath, "Could not locate RimSky.cs by walking up from: " + testDir);
            Assert.IsTrue(File.Exists(rimSkySourcePath), "RimSky.cs not found at: " + rimSkySourcePath);

            // Read the source and verify the expected log call is present
            var source = File.ReadAllText(rimSkySourcePath);

            // The exact log call we expect to be present in the source file
            const string expected = "Log.Message(\"Hello World from RimSky mod!\");";

            StringAssert.Contains(expected, source,
                "Expected RimSky.cs to contain the hello-world log call, but it was not found.");
        }
    }
}
