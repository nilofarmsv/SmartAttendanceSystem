using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSanse.Services
{
    using Telegram.Bot;

    namespace WorkSanse.Services
    {
        public class TelegramService
        {
            private readonly TelegramBotClient _bot;

            public TelegramService()
            {
                _bot = new TelegramBotClient("8298311733:AAF8SpWC2uRSp9zfhvx4ZTMmdKGFUolG8R4");
            }

            public async Task SendMessageAsync(long chatId, string message)
            {
                await _bot.SendMessage(chatId, message);
            }
        }
    }
}
