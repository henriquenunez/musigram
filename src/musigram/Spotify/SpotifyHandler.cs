using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.Web.Auth;
using System.Linq;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;

namespace musigram.Spotify
{
	public static class SpotifyHandler
	{
		public static string searchByID(string Id, SpotifyWebAPI _spotifyclient)
		{
			FullTrack track = _spotifyclient.GetTrack(Id);
			return track.Name;
		}

		public static string[] searchAlbums(string name, SpotifyWebAPI _spotifyclient)
		{
			SearchItem foundList = _spotifyclient.SearchItemsEscaped(name, SearchType.Album);
			var _return_list = foundList.Albums;
			if (_return_list != null)
				try
				{
					return _return_list.Items.Select(x => x.Name + " on " + x.ReleaseDate).ToArray();
				}
				catch (NullReferenceException e)
				{
					return null;
				}
			else return new[] { "Found none." };
		}

		public static string[] searchArtists(string name, SpotifyWebAPI _spotifyclient)
		{
			SearchItem foundList = _spotifyclient.SearchItemsEscaped(name, SearchType.Artist);
			var _return_list = foundList.Artists;

			if (_return_list != null)
				try
				{
					return _return_list.Items.Select(x => x.Name).ToArray();
				}
				catch (NullReferenceException e)
				{
					return null;
				}
			else return new[] { "Found none." };
		}

		public static string[] searchSongs(string name, SpotifyWebAPI _spotifyclient)
		{
			SearchItem foundList = _spotifyclient.SearchItemsEscaped(name, SearchType.Track);
			var _return_list = foundList.Tracks;

			if (_return_list != null)
				try
				{
					return _return_list.Items.Select(x => x.Name).ToArray();
				}
				catch (NullReferenceException e)
				{
					return null;
				}
			else return new[] { "Found none." };
		}

	};
}
