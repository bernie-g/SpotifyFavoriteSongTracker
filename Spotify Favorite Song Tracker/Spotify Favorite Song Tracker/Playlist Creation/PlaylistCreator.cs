using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spotify_Favorite_Song_Tracker.Playlist_Creation
{
    internal class PlaylistCreator
    {
        public static void CreateFavoritePlaylist(IEnumerable<ExtendedPlayHistory> songs, SpotifyWebAPI api, int minimumTimesPlayed)
        {
            string userId = api.GetPrivateProfile().Id;
            var playlistId = api.CreatePlaylist(userId, "Favorite Songs (" + minimumTimesPlayed + "+ listens)", false).Id;
            //var tracks = songs.Where(x => x.TimesPlayed >= minimumTimesPlayed).Select(x => x.Track.Uri);
            //api.AddPlaylistTracks(userId, playlistId, tracks.ToList());
        }
    }
}