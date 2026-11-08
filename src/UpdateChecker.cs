using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace CapiQR
{
    internal sealed class UpdateCheckResult
    {
        public bool IsSuccess { get; private set; }
        public bool IsUpdateAvailable { get; private set; }
        public string LatestVersion { get; private set; }
        public string ReleaseUrl { get; private set; }
        public string ErrorMessage { get; private set; }

        public static UpdateCheckResult Success(bool isUpdateAvailable, string latestVersion, string releaseUrl)
        {
            return new UpdateCheckResult
            {
                IsSuccess = true,
                IsUpdateAvailable = isUpdateAvailable,
                LatestVersion = latestVersion,
                ReleaseUrl = releaseUrl
            };
        }

        public static UpdateCheckResult Failure(string errorMessage)
        {
            return new UpdateCheckResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage
            };
        }
    }

    internal static class UpdateChecker
    {
        private const string LatestReleaseApi = "https://api.github.com/repos/robste888-spece/CapiQR/releases/latest";
        private const string ReleasesPage = "https://github.com/robste888-spece/CapiQR/releases/latest";
        private const int TimeoutMilliseconds = 8000;

        public static async Task<UpdateCheckResult> CheckAsync(Version currentVersion)
        {
            HttpWebRequest request = null;
            try
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
                request = (HttpWebRequest)WebRequest.Create(LatestReleaseApi);
                request.Method = "GET";
                request.Accept = "application/vnd.github+json";
                request.UserAgent = "CapiQR/" + Normalize(currentVersion);
                request.Headers["X-GitHub-Api-Version"] = "2026-03-10";
                request.ReadWriteTimeout = TimeoutMilliseconds;

                Task<WebResponse> responseTask = request.GetResponseAsync();
                if (await Task.WhenAny(responseTask, Task.Delay(TimeoutMilliseconds)) != responseTask)
                {
                    request.Abort();
                    return UpdateCheckResult.Failure("หมดเวลารอการตอบกลับจาก GitHub");
                }

                using (var response = (HttpWebResponse)await responseTask)
                using (var stream = response.GetResponseStream())
                {
                    var serializer = new DataContractJsonSerializer(typeof(GitHubRelease));
                    var release = serializer.ReadObject(stream) as GitHubRelease;
                    return EvaluateRelease(release == null ? null : release.TagName, release == null ? null : release.HtmlUrl, currentVersion);
                }
            }
            catch (WebException ex)
            {
                return UpdateCheckResult.Failure(GetFriendlyNetworkError(ex));
            }
            catch (Exception ex)
            {
                return UpdateCheckResult.Failure(ex.Message);
            }
        }

        internal static UpdateCheckResult EvaluateRelease(string tagName, string releaseUrl, Version currentVersion)
        {
            Version latestVersion;
            string cleanVersion;
            if (!TryParseTag(tagName, out latestVersion, out cleanVersion))
                return UpdateCheckResult.Failure("GitHub ส่งเลขเวอร์ชันที่อ่านไม่ได้");

            Uri safeUrl;
            if (!Uri.TryCreate(releaseUrl, UriKind.Absolute, out safeUrl) ||
                safeUrl.Scheme != Uri.UriSchemeHttps ||
                !string.Equals(safeUrl.Host, "github.com", StringComparison.OrdinalIgnoreCase))
            {
                releaseUrl = ReleasesPage;
            }

            return UpdateCheckResult.Success(latestVersion > Normalize(currentVersion), cleanVersion, releaseUrl);
        }

        private static bool TryParseTag(string tagName, out Version version, out string cleanVersion)
        {
            version = null;
            cleanVersion = string.Empty;
            if (string.IsNullOrWhiteSpace(tagName))
                return false;

            cleanVersion = tagName.Trim().TrimStart('v', 'V');
            Version parsed;
            if (!Version.TryParse(cleanVersion, out parsed) || parsed.Major < 0 || parsed.Minor < 0 || parsed.Build < 0)
                return false;

            version = new Version(parsed.Major, parsed.Minor, parsed.Build, 0);
            cleanVersion = parsed.Major + "." + parsed.Minor + "." + parsed.Build;
            return true;
        }

        private static Version Normalize(Version version)
        {
            if (version == null)
                return new Version(0, 0, 0, 0);
            return new Version(
                Math.Max(0, version.Major),
                Math.Max(0, version.Minor),
                Math.Max(0, version.Build),
                0);
        }

        private static string GetFriendlyNetworkError(WebException exception)
        {
            var response = exception.Response as HttpWebResponse;
            if (response != null)
                return "GitHub ตอบกลับ HTTP " + (int)response.StatusCode;
            return exception.Status == WebExceptionStatus.Timeout
                ? "หมดเวลารอการตอบกลับจาก GitHub"
                : "เชื่อมต่อ GitHub ไม่สำเร็จ";
        }

        [DataContract]
        private sealed class GitHubRelease
        {
            [DataMember(Name = "tag_name")]
            public string TagName { get; set; }

            [DataMember(Name = "html_url")]
            public string HtmlUrl { get; set; }
        }
    }
}
