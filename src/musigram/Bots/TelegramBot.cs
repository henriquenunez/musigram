using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

using Microsoft.Extensions.Options;
using musigram.Settings;

namespace musigram.Bots
{
	class TelegramBot
	{
		static ITelegramBotClient telegramBotClient;

		private static Dictionary<string, User> userChatPairs = new Dictionary<string, User>();

		private TelegramConfiguration config;

		public TelegramBot(IOptions<TelegramConfiguration> _config)
		{
			config = _config.Value;
		}

		public void StartBot()
		{
			//retrieves token from dependency injection
			telegramBotClient = new TelegramBotClient(config.token);
			telegramBotClient.OnMessage += EvalMessage;
			telegramBotClient.StartReceiving();
			Thread.Sleep(int.MaxValue);
		}

		static async void EvalMessage(object sender, MessageEventArgs messageEvent)
		{
			var _username = messageEvent.Message.From.Username;
			var _msgtext = messageEvent.Message.Text;

			try
			{
				System.Diagnostics.Debug.WriteLine("User first name:" + userChatPairs[_username].firstName);
			}
			catch(KeyNotFoundException KNFe)
			{
				System.Diagnostics.Debug.WriteLine("First time user ?");
				userChatPairs.Add(_username, new User(messageEvent.Message));
				System.Diagnostics.Debug.WriteLine("User added:" + userChatPairs[_username].firstName);
				await userChatPairs[_username].UserAuthenticate();
				System.Diagnostics.Debug.WriteLine("User authenticated");
			}

			//Run commands
			var _returnmsg = userChatPairs[_username].parseMessage(_msgtext);

			SendMessage(userChatPairs[_username], _returnmsg != null ? _returnmsg : "Não entendi.");
		}

		static async void SendMessage(User _user, string payload)
		{
			//sends message to the user with the payload.
			try
			{
				await telegramBotClient.SendTextMessageAsync(
					chatId: _user.chatID, //replace with dictionary entry
					text: payload
				);
			}
			catch (Telegram.Bot.Exceptions.ApiRequestException)
			{
				System.Diagnostics.Debug.WriteLine("Failed delivering");
			}
			catch (KeyNotFoundException)
			{
				System.Diagnostics.Debug.WriteLine("User not registered");
			}
			catch (ArgumentNullException)
			{
				System.Diagnostics.Debug.WriteLine("No user on parameter");
			}
		}
	}
}
