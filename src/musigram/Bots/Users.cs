using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using musigram.Spotify;
using Telegram.Bot;
using SpotifyAPI.Web;

/*Template for user, contains telegram info for message sending and also spotify authorization*/
namespace musigram.Bots
{
	class User
	{
		//user info
		public string username;
		public string firstName { get; set; }
		public long chatID;
		private SpotifyWebAPI userHandler;

		private CommandList userCommands;

		public User(Telegram.Bot.Types.Message firstMessage)
		{
			username = firstMessage.From.Username;
			chatID = firstMessage.Chat.Id;
			firstName = firstMessage.From.Username;
		}

		//Will check whether a new command is triggered 
		//or we need to continue parsing the input
		public string parseMessage(string message)
		{
			string ret_val;

			if (userCommands != null)
				try
				{
					ret_val = userCommands.execute(message.Split(" "));
				}
				catch (Exception e)
				{
					ret_val ="Exceção aqui mano";
				}
			else ret_val = "Deu ruim cara";
			return ret_val;
		}

		//Gambiarra por agora....
		public async Task UserAuthenticate()
		{ 
			//Prompting for user authentication
			userHandler = await AuthenticationHandler.CredentialsAuthMode();
			userCommands = new CommandList(userHandler);
		}

	}
}
