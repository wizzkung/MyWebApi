namespace BmwFullProjectJquery.Model.DeepSeek
{
    public class ChatRequest
    {
        public string model { get; set; } = "deepseek-chat";
        public List<ChatMessage> messages { get; set; } = new();
        public double temperature { get; set; } = 0.7;
        public int max_tokens { get; set; } = 1000;
    }
}
