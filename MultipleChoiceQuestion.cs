using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

public class MultipleChoiceQuestion
{
    public string QuestionText { get; set; }

    public ReplyKeyboardMarkup KeyboardMarkup { get; set; }

    public MultipleChoiceQuestion(string questionText, ReplyKeyboardMarkup options)
    {
        QuestionText = questionText;
        KeyboardMarkup = options;
    }

   
}