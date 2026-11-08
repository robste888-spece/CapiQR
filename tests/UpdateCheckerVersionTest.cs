using System;

namespace CapiQR
{
    internal static class UpdateCheckerVersionTest
    {
        private static int Main()
        {
            AssertAvailable("v1.1.0", new Version(1, 0, 0, 0), true);
            AssertAvailable("1.1.0", new Version(1, 1, 0, 0), false);
            AssertAvailable("v1.0.9", new Version(1, 1, 0, 0), false);

            UpdateCheckResult invalid = UpdateChecker.EvaluateRelease("latest", "https://github.com/robste888-spece/CapiQR/releases/latest", new Version(1, 0, 0, 0));
            if (invalid.IsSuccess)
                throw new InvalidOperationException("Invalid release tags must fail validation.");

            UpdateCheckResult unsafeUrl = UpdateChecker.EvaluateRelease("v1.2.0", "http://example.com/update.exe", new Version(1, 1, 0, 0));
            if (!unsafeUrl.IsSuccess || unsafeUrl.ReleaseUrl != "https://github.com/robste888-spece/CapiQR/releases/latest")
                throw new InvalidOperationException("Unsafe release URLs must fall back to the official GitHub Releases page.");

            Console.WriteLine("UpdateChecker version tests passed.");
            return 0;
        }

        private static void AssertAvailable(string tag, Version currentVersion, bool expected)
        {
            UpdateCheckResult result = UpdateChecker.EvaluateRelease(tag, "https://github.com/robste888-spece/CapiQR/releases/tag/" + tag, currentVersion);
            if (!result.IsSuccess || result.IsUpdateAvailable != expected)
            {
                throw new InvalidOperationException(
                    "Unexpected comparison result for " + tag + " versus " + currentVersion + ". Expected update=" + expected + ".");
            }
        }
    }
}
