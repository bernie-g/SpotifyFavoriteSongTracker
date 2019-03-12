using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Spotify_Favorite_Song_Tracker
{
    public class ExtendedPlayHistory : PlayHistory
    {
        public int TimesPlayed { get; set; }
    }
}