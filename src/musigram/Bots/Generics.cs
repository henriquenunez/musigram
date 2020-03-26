using System;
using System.Collections.Generic;
using System.Text;
using musigram.Spotify;

/*Classes for handling commands
 * i.e.:
 *  /search artist Arcade Fire
 *  /search album Serasa do Amor
 *  /create playlist COVID-19
	 */
namespace musigram.Bots
{
	/*
	 * Dont actually know why ive put an interface here, im still figuring things out
	public interface IFunction
	{
		String Run(SpotifyHandler handler);
	}
	*/
	public abstract class Command
	{
		internal CommandMap subcommands;
		internal string _message;
		internal object parent;
		public Command()
		{
			subcommands = new CommandMap();
		}

		public Command(object _parent)
		{
			subcommands = new CommandMap();
			parent = _parent;
		}

		public string Exec(string[] message)
		{
			string retMessage = subcommands.try_exec(message);
			if (retMessage != null) return retMessage;

			Console.WriteLine("tried to process command...");

			return Run(message);
		}

		public string Exec(string _message)
		{
			var message = _message.Split(" ");
			string retMessage = subcommands.try_exec(message);
			if (retMessage != null) return retMessage;

			Console.WriteLine("tried to process command...");

			return Run(message);
		}

		public abstract string Run(string[] message);
	}

	class CommandMap
	{
		Dictionary<string, Command> commandTable;

		public CommandMap()
		{
			commandTable = new Dictionary<string, Command>();
		}

		public void Add(string name, Command cmd) { commandTable.Add(name.ToLower(), cmd); }

		public string try_exec(string[] message)
		{
			try
			{
				Command c = commandTable[message[0].ToLower()];

				if (c != null)
				{
					//This is only available on C#8.0 or greater
					return c.Exec(message[1..^0]);
				}
				return null;

			}
			catch (IndexOutOfRangeException e)
			{
				return null;
			}
			catch (KeyNotFoundException e)
			{
				return null;
			}
		}
	}
}
