using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace WorkSanse.Services
{
    public class TelegramNotificationService
    {
        private readonly TelegramBotClient _bot;

        public TelegramNotificationService(string botToken)
        {
            _bot = new TelegramBotClient(botToken);
        }

        public async Task SendMessageAsync(long chatId, string message)
        {
            await _bot.SendMessage(
                chatId: chatId,
                text: message,
                parseMode: ParseMode.Html
            );
        }
    }
}