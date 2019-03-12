using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.IO;

namespace Spotify_Favorite_Song_Tracker.File_Management
{
    internal class FileManager
    {
        public static string FavoriteSongsJsonFile = Path.Combine(Settings.SaveDirectory, "FavoriteSongs.json");
        public static string TimeStampFile = Path.Combine(Settings.SaveDirectory, "TimeStamp.json");
        public static string PlayListsFile = Path.Combine(Settings.SaveDirectory, "Data.json");

        public static List<ExtendedPlayHistory> GetLikedSongs()
        {
            if (!Directory.Exists(Settings.SaveDirectory))
                Directory.CreateDirectory(Settings.SaveDirectory);

            if (!File.Exists(FavoriteSongsJsonFile))
            {
                using (File.Create(FavoriteSongsJsonFile))
                { }
                return new List<ExtendedPlayHistory>();
            }
            return SerializationHelpers.ReadFromJsonFile<List<ExtendedPlayHistory>>(FavoriteSongsJsonFile) ?? new List<ExtendedPlayHistory>();
        }

        public static long GetMostRecentTimeStamp()
        {
            long fiveMinutesAgo = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(5)).ToUnixTimeMillisecondsPoly();
            DataStorage dataStorage = new DataStorage { PreviousTimeStamp = fiveMinutesAgo };

            if (!Directory.Exists(Settings.SaveDirectory))
                Directory.CreateDirectory(Settings.SaveDirectory);

            if (!File.Exists(TimeStampFile))
            {
                File.Create(TimeStampFile).Dispose();
                SerializationHelpers.WriteToJsonFile(TimeStampFile, dataStorage);
                return fiveMinutesAgo;
            }

            return SerializationHelpers.ReadFromJsonFile<DataStorage>(TimeStampFile).PreviousTimeStamp;
        }

        public static List<string> GetPlaylistIDs()
        {
            if (!Directory.Exists(Settings.SaveDirectory))
                Directory.CreateDirectory(Settings.SaveDirectory);

            if (!File.Exists(PlayListsFile))
            {
                using (File.Create(PlayListsFile))
                { }
                return null;
            }
            return SerializationHelpers.ReadFromJsonFile<List<string>>(PlayListsFile);
        }

        public static void SaveSongs(List<ExtendedPlayHistory> distinctHistory)
        {
            SerializationHelpers.WriteToJsonFile(FavoriteSongsJsonFile, distinctHistory);
        }

        public static void SaveTimeStamp()
        {
            DataStorage dataStorage = new DataStorage { PreviousTimeStamp = DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(5)).ToUnixTimeMillisecondsPoly() };

            SerializationHelpers.WriteToJsonFile(TimeStampFile, dataStorage);
        }

        public static void SavePlaylistIDs(List<string> playlistIDs)
        {
            SerializationHelpers.WriteToJsonFile(PlayListsFile, playlistIDs);
        }
    }
}