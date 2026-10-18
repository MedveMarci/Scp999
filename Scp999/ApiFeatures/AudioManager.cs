using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using RoleAPI.API.Audio;

namespace Scp999.ApiFeatures;

internal static class AudioManager
{
    private const string ResourcePrefix = "Scp999.Audio.";

    private static readonly Assembly Assembly = typeof(AudioManager).Assembly;

    private static readonly Dictionary<string, string> Resources = Assembly.GetManifestResourceNames().Where(n => n.StartsWith(ResourcePrefix, StringComparison.Ordinal)).ToDictionary(n => n.Substring(ResourcePrefix.Length), n => n, StringComparer.OrdinalIgnoreCase);

    private static string Directory => Scp999.Singleton?.Config.AudioDirectoryPath;

    public static AbilityAudio Get(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
            return null;

        string filePath = null;
        try
        {
            if (!string.IsNullOrEmpty(Directory))
                filePath = Path.Combine(Directory, fileName);
        }
        catch (Exception e)
        {
            LogManager.Warn($"Invalid audio file name {fileName}.\n{e.Message}");
            return null;
        }

        if (filePath != null && File.Exists(filePath))
            return AbilityAudio.File(filePath);

        if (Resources.TryGetValue(fileName, out string resourceName))
            return AbilityAudio.Embedded(resourceName, Assembly);

        LogManager.Warn($"The audio file {fileName} does not exist at path {filePath} and is not a built-in sound.");
        return null;
    }

    public static void ExtractMissing(string directory)
    {
        if (string.IsNullOrEmpty(directory) || Resources.Count == 0)
            return;

        try
        {
            System.IO.Directory.CreateDirectory(directory);
        }
        catch (Exception e)
        {
            LogManager.Warn($"Could not create the audio folder, the built-in sounds will be played from Scp999.dll.\nPath: {directory}\n{e.Message}");
            return;
        }

        int extracted = 0;
        foreach (KeyValuePair<string, string> resource in Resources)
        {
            string filePath = Path.Combine(directory, resource.Key);
            if (File.Exists(filePath))
                continue;

            bool created = false;
            try
            {
                using Stream source = Assembly.GetManifestResourceStream(resource.Value);
                if (source is null)
                    continue;

                using (FileStream target = new(filePath, FileMode.CreateNew, FileAccess.Write))
                {
                    created = true;
                    source.CopyTo(target);
                }

                extracted++;
            }
            catch (Exception e)
            {
                LogManager.Warn($"Could not save the audio file {resource.Key}, it will be played from Scp999.dll.\n{e.Message}");

                if (created)
                    try
                    {
                        File.Delete(filePath);
                    }
                    catch
                    {
                        // ignored
                    }
            }
        }

        if (extracted > 0)
            LogManager.Info($"Saved {extracted} built-in audio file(s) to {directory}.");
    }
}
