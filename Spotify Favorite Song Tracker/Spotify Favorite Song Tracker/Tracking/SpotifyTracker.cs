using MiscUtil.Reflection;
using Spotify_Favorite_Song_Tracker;
using Spotify_Favorite_Song_Tracker.File_Management;
using Spotify_Favorite_Song_Tracker.Playlist_Creation;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Spotify_Favorite_Song_Tracker
{
    internal class SpotifyTracker
    {
        public static void Scheduler_Elapsed(SpotifyWebAPI api)
        {
            long previousTimeStamp = FileManager.GetMostRecentTimeStamp();
            DateTime timeStamp = DateTimeOffset.FromUnixTimeMilliseconds(previousTimeStamp).UtcDateTime;
            CursorPaging<PlayHistory> recentData = api.GetUsersRecentlyPlayedTracks(50, timeStamp);
            RecordSongs(recentData, api);
        }

        public static void RecordSongs(CursorPaging<PlayHistory> recentData, SpotifyWebAPI api)
        {
            var mostRecentHistory = ConvertToExtendedPlayHistory(recentData).ToList();
            var groupedHistory = mostRecentHistory.GroupBy(x => x.Track.Id);
            var distinctHistory = new List<ExtendedPlayHistory>();

            foreach (var group in groupedHistory)
            {
                var song = group.ToList()[0];
                song.TimesPlayed = group.Count();
                distinctHistory.Add(song);
            }

            distinctHistory = distinctHistory.OrderByDescending(x => x.TimesPlayed).ToList();
            var previousDistinctHistory = FileManager.GetLikedSongs();
            AddTogetherSongs(ref distinctHistory, previousDistinctHistory);
            //PlaylistCreator.CreatePlaylist(distinctHistory, api, 2);
            FileManager.SaveSongs(distinctHistory);
            FileManager.SaveTimeStamp();
        }

        private static void AddTogetherSongs(ref List<ExtendedPlayHistory> distinctHistory,
            List<ExtendedPlayHistory> previousDistinctHistory)
        {
            foreach (var song in previousDistinctHistory)
            {
                if (distinctHistory.Any(x => x.Track.Id == song.Track.Id))
                {
                    distinctHistory.First(x => x.Track.Id == song.Track.Id).TimesPlayed++;
                }
                else
                {
                    distinctHistory.Add(song);
                }
            }

            distinctHistory = distinctHistory.OrderByDescending(x => x.TimesPlayed).ToList();
        }

        public static IEnumerable<ExtendedPlayHistory> ConvertToExtendedPlayHistory(CursorPaging<PlayHistory> history)
        {
            foreach (var page in history.Items)
            {
                yield return PropertyCopy<ExtendedPlayHistory>.CopyFrom(page);
            }
        }
    }
}