using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Spotify_Favorite_Song_Tracker
{
    internal static class Settings
    {
        /// <summary>
        ///  Client ID taken from Spotify.
        /// </summary>
        public static string ClientID = "6b7bc9d9983645f2b99814052322d36b";

        /// <summary>
        ///  How often to log Spotify songs in minutes.
        /// </summary>
        public static int ScheduleDelay = 5;

        /// <summary>
        ///  Directory name to save program's files to.
        /// </summary>
        public static string SaveDirectory = Path.Combine(Directory.GetCurrentDirectory(), "SpotifyTracker");

        /// <summary>
        ///  Threshold for number of listens for the Favorite Songs playlist.
        /// </summary>
        public static int FavoriteThreshold = 3;

        /// <summary>
        ///  Threshold for number of listens for the Loved Songs playlist.
        /// </summary>
        public static int LovedThreshold = 5;

        /// <summary>
        ///  Threshold for number of listens for the First-Picks playlist.
        /// </summary>
        public static int FirstPicksThreshold = 8;
    }
}