using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using LabApi.Loader.Features.Paths;
using Scp999.ApiFeatures;

namespace Scp999.Features;

internal static class AudioSetup
{
    private const string GithubRepo = "MedveMarci/Scp999";
    private const string ZipAssetName = "Scp999Audio.zip";
    private const string FolderName = "Scp999";

    private static readonly string[] ExpectedFiles =
    [
        "circus.ogg", "funnytoy.ogg", "uwu.ogg", "health.ogg",
        "yippee-tbh1.ogg", "yippee-tbh2.ogg",
        "hello.ogg", "hi.ogg"
    ];

    internal static void EnsureAudioFiles()
    {
        Task.Run(() =>
        {
            try
            {
                var audioDir = ResolveAudioDirectory();
                if (audioDir == null)
                {
                    DownloadAndExtract(null, ExpectedFiles);
                    return;
                }

                var missing = ExpectedFiles
                    .Where(f => !File.Exists(Path.Combine(audioDir, f)))
                    .ToArray();

                if (missing.Length == 0)
                {
                    LogManager.Debug("Scp999 audio: all files present.");
                    return;
                }

                LogManager.Info($"Scp999 audio: {missing.Length} file(s) missing — downloading.");
                DownloadAndExtract(audioDir, missing);
            }
            catch (Exception ex)
            {
                LogManager.Error($"Scp999 audio setup failed: {ex.Message}");
            }
        });
    }

    /// <summary>
    ///     Finds the audio folder under PathManager.Configs, handling case differences.
    ///     Returns the path if it exists (renaming if case is wrong), or null if it doesn't exist at all.
    /// </summary>
    private static string ResolveAudioDirectory()
    {
        var configsPath = PathManager.Configs.FullName;
        var expectedPath = Path.Combine(configsPath, FolderName);

        if (Directory.Exists(expectedPath))
            return expectedPath;

        // Case-insensitive search (relevant on Linux)
        var existing = Directory.GetDirectories(configsPath)
            .FirstOrDefault(d => string.Equals(Path.GetFileName(d), FolderName, StringComparison.OrdinalIgnoreCase));

        if (existing == null)
            return null;

        // Rename to correct casing
        Directory.Move(existing, expectedPath);
        LogManager.Info($"Scp999 audio: renamed '{Path.GetFileName(existing)}' → '{FolderName}'.");
        return expectedPath;
    }

    private static void DownloadAndExtract(string targetDir, string[] filesToExtract)
    {
        var downloadUrl = GetLatestAssetUrl();
        if (downloadUrl == null)
        {
            LogManager.Error("Scp999 audio: could not find download URL from GitHub releases.");
            return;
        }

        LogManager.Info($"Scp999 audio: downloading {ZipAssetName}...");

        byte[] zipBytes;
        using (var client = new WebClient())
        {
            client.Headers[HttpRequestHeader.UserAgent] = "Scp999-Plugin";
            zipBytes = client.DownloadData(downloadUrl);
        }

        targetDir ??= Path.Combine(PathManager.Configs.FullName, FolderName);
        Directory.CreateDirectory(targetDir);

        using var zipStream = new MemoryStream(zipBytes);
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);

        var fileSet = new HashSet<string>(filesToExtract, StringComparer.OrdinalIgnoreCase);

        foreach (var entry in archive.Entries)
        {
            var name = Path.GetFileName(entry.FullName);
            if (!fileSet.Contains(name))
                continue;

            var dest = Path.Combine(targetDir, name);
            entry.ExtractToFile(dest, true);
            LogManager.Info($"Scp999 audio: extracted '{name}'.");
        }
    }

    private static string GetLatestAssetUrl()
    {
        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Scp999-Plugin");
            client.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");

            var json = client.GetStringAsync($"https://api.github.com/repos/{GithubRepo}/releases/latest")
                .GetAwaiter().GetResult();

            var root = JsonDocument.Parse(json).RootElement;

            if (!root.TryGetProperty("assets", out var assets) || assets.ValueKind != JsonValueKind.Array)
                return null;

            foreach (var asset in assets.EnumerateArray())
            {
                if (!asset.TryGetProperty("name", out var nameProp) ||
                    !string.Equals(nameProp.GetString(), ZipAssetName, StringComparison.OrdinalIgnoreCase))
                    continue;

                if (asset.TryGetProperty("browser_download_url", out var urlProp))
                    return urlProp.GetString();
            }

            return null;
        }
        catch (Exception ex)
        {
            LogManager.Error($"Scp999 audio: GitHub API error — {ex.Message}");
            return null;
        }
    }
}