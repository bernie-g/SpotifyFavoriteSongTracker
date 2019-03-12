using MiscUtil.Reflection;
using MoreLinq.Extensions;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Timers;

namespace Spotify_Favorite_Song_Tracker
{
    internal class SpotifyTrackerCli
    {
        private static void Main(string[] args)
        {
            AuthorizeUser();
        }

        private static void AuthorizeUser()
        {
            ImplictGrantAuth auth =
                new ImplictGrantAuth(Settings.ClientID, "http://localhost:4002", "http://localhost:4002", Scope.UserReadRecentlyPlayed | Scope.PlaylistModifyPrivate | Scope.PlaylistReadPrivate | Scope.PlaylistModifyPublic);

            auth.AuthReceived += OnAuthReceived;
            auth.Start();
            auth.OpenBrowser();
            Thread.Sleep(-1);
        }

        private static void OnAuthReceived(object sender, Token payload)
        {
            var auth = sender as ImplictGrantAuth;
            auth.Stop();
            SpotifyWebAPI api = new SpotifyWebAPI() { TokenType = payload.TokenType, AccessToken = payload.AccessToken };
            while (true)
            {
                SpotifyTracker.Scheduler_Elapsed(api);
                Thread.Sleep(TimeSpan.FromSeconds(Settings.ScheduleDelay));
            }
        }
    }
}