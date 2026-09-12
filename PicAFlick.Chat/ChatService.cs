namespace PicAFlick.Chat
{
    public class ChatService
    {
        private readonly IChatResponder _chatResponder;

        public ChatService(IChatResponder chatResponder)
        {
            _chatResponder = chatResponder;
        }

        public async Task<string> ProcessIncomingMessageAsync(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException();

            var response = await _chatResponder.GenerateResponseAsync(message);
                
            return response;
        }
    }
}