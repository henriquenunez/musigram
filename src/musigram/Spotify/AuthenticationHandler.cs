using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;

namespace musigram.Spotify
{
	static class AuthenticationHandler
	{
		static private AuthorizationCodeAuth auth;

		public static string clientId { private get; set; }
		public static string secretId { private get; set; }

		/*
		AuthenticationHandler()
		{
			auth = new AuthorizationCodeAuth(
			   _clientId,
			   _secretId,
			   "http://localhost:4002",
			   "http://localhost:4002",
			   Scope.PlaylistReadPrivate | Scope.PlaylistReadCollaborative
			   );

			auth.AuthReceived += async (sender, payload) =>
			{
				auth.Stop();
				Token token = await auth.ExchangeCode(payload.Code);
				SpotifyWebAPI api = new SpotifyWebAPI()
				{
					TokenType = token.TokenType,
					AccessToken = token.AccessToken
				};
				// Do requests with API client
				auth.Start(); // Starts an internal HTTP Server
				auth.OpenBrowser();
			};
			
		}*/

		public static async Task<SpotifyWebAPI> CredentialsAuthMode()
		{
			CredentialsAuth auth = new CredentialsAuth(clientId, secretId);
			Token token = await auth.GetToken();

			return new SpotifyWebAPI()
			{
				AccessToken = token.AccessToken,
				TokenType = token.TokenType
			};
		}
	};
}
