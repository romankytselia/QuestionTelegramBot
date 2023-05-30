namespace QuestionTelegramBot
{
    public class UserAnswers
    {
        public long ChatId { get; set; }
        public int CurrentIndex { get; set; } = 0;
        public string Answers { get; set; }
    }
}