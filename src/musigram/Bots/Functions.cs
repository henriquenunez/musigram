using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using SpotifyAPI.Web;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;

using Telegram.Bot;

using musigram.Spotify;

namespace musigram.Bots
{
	public class CommandList
	{
		CommandMap commands = new CommandMap();
		internal CommandMap _commandState;

		public CommandList(SpotifyWebAPI _handler)
		{
			commands.Add("/start",	new WelcomeMessage());
			commands.Add("/browse",	new Search(_handler, this));
			commands.Add("/playlist",	new Search(_handler, this));
		}

		public string execute(string[] message)
		{
			if (_commandState != null)
				return _commandState.try_exec(message);
			else
				return		commands.try_exec(message);
		}

		/*COMMANDS*/
		public class WelcomeMessage : Command
		{
			public override string Run(string[] message)
			{
				_message = "Welcome to the bot!";
				return _message;
			}
		}

		public class Search : Integration
		{
			public Search(SpotifyWebAPI _handler, object _parent) : base(_handler, _parent)
			{
				subcommands = new CommandMap();

				//Handler variable here is from abstract class integration
				//subcommands.Add("song", );
				subcommands.Add("album", new SearchAlbum(handler, _parent));
				subcommands.Add("song", new SearchSong(handler, _parent));
				subcommands.Add("cancel", new SearchSong(handler, _parent));
				//subcommands.Add("artist", );
			}

			public override string Run(string[] message)
			{
				//If command unsuccesful, saves this command as current state.
				((CommandList)parent)._commandState = this.subcommands;

				_message =  "Type album or song."; //TODO use json for localization
				return _message;
			}

			class SearchSong : Integration
			{
				public SearchSong(SpotifyWebAPI _handler, object _parent) : base(_handler, _parent)
				{
				}
				public override string Run(string[] message)
				{
					//Spotifysearch song name
					_message = null;

					var songs = SpotifyHandler.searchSongs(string.Join(" ", message), handler);
					foreach (string song in songs)
					{
						_message += song + "\n";
					}

					/*END OF COMMAND*/
					((CommandList)parent)._commandState = null;
				
					return _message;
				}
			}

			class SearchAlbum : Integration
			{ 
				public SearchAlbum(SpotifyWebAPI _handler, object _parent) : base(_handler, _parent)
				{
				}
				public override string Run(string[] message)
				{
					//Spotifysearch song name

					_message = null;
					var albums = SpotifyHandler.searchAlbums(string.Join(" ", message), handler);
					foreach (string album in albums)
					{
						_message += album + "\n";
					}
					//_message = ;

					/*END OF COMMAND*/
					((CommandList)parent)._commandState = null;

					return _message;
				}
			}
		}
	}
}

/*
 * 
 CommandMap {
    Map<String, Command> m

    insert(String name, Command cmd) { m.put(name.toLowerCase(), cmd) }

    try_exec(String[] message) {
        Command c = m.get(message[0].toLowerCase())
        if(c != null) {
            c.exec(message[1:])
            return true
        }
        return false
    }
}


Command {
    CommandMap subcommands

    exec(String[] message) {
        if(subcommands.try_exec(message)) return;
        run(message);
    }

    run(String[] message)
}


A extends Command {
    constructor() { subcommands.insert("b", new B()) }

    run(String[] message) { print("A running") }

    B extends Command {
        run(String[] message) { print("A.B running") }
    }
}

main() {
    CommandMap commands

    commands.insert("a", new A())

    commands.try_exec(
        "A B 123 456".split(" ")
    ) // A.B running

    commands.try_exec(
        "A 123 456 789".split(" ")
    ) // A running
}
*/



