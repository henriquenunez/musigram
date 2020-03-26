using System;
using System.Collections.Generic;
using System.Text;

using SpotifyAPI.Web;

using Telegram.Bot;


/*This class wraps both spotify and telegram api for commands*/
namespace musigram.Bots
{
    public abstract class Integration : Command
    {
        internal SpotifyWebAPI handler;
        //internal
        public Integration(){}
        public Integration(object parent) : base(parent) {}
        public Integration(SpotifyWebAPI _handler, object parent) : base(parent)
        {
            handler = _handler;
        }
    }
}
