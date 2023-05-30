using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using QuestionTelegramBot;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;


class Program
{
    const string TOKEN = "5909956706:AAHr5J1yyHpmyj8EIQyWPRXXWog-ypOIzBA";

    private static TelegramBotClient botClient;
    private static List<MultipleChoiceQuestion> options;
    private static List<UserAnswers> _answersList = new List<UserAnswers>();

    static void Main()
    {
        const string fileName = "C:\\Users\\L-91 5590\\Downloads\\QuestionTelegramBot 2\\QuestionTelegramBot\\questions.txt";
        options = ReadQuestionsFromFile(fileName);
        botClient = new TelegramBotClient(TOKEN);
        botClient.OnMessage += Bot_OnMessage;
        botClient.StartReceiving();

        Console.WriteLine("Bot started. Press any key to exit.");
        Console.ReadKey();

        botClient.StopReceiving();
    }

    private static async void Bot_OnMessage(object sender, MessageEventArgs e)
    {
        var user = _answersList.FirstOrDefault(i => i.ChatId == e.Message.Chat.Id);
        if (user is null)
        {
            user = new UserAnswers { ChatId = e.Message.Chat.Id, CurrentIndex = -1 };
            _answersList.Add(user);
        }

        if (e.Message.Text != null)
        {
            user.CurrentIndex += 1;
            int questionIndex = user.CurrentIndex;
            string userAnswer = e.Message.Text;

            if (options.Count > 0)
            {
                if (questionIndex < options.Count)
                {
                    string question = options[questionIndex].QuestionText;
                    await botClient.SendTextMessageAsync(e.Message.Chat.Id, question, replyMarkup:options[questionIndex].KeyboardMarkup );
                }
                else
                {
                    await botClient.SendTextMessageAsync(e.Message.Chat.Id, "No more questions.");
                    await botClient.SendTextMessageAsync(e.Message.Chat.Id, user.Answers);
                }
            }
            else
            {
                await botClient.SendTextMessageAsync(e.Message.Chat.Id, "No questions found.");
            }

            if (questionIndex - 1 >= 0)
            {
                user.Answers += "\n" + options[questionIndex - 1].QuestionText + ": " + e.Message.Text;
            }
            Console.WriteLine($"User: {e.Message.From.Username} | Answer: {userAnswer}");
        }
    }

    

    private static ReplyKeyboardMarkup GetAnswerOptions(List<string> options)
    {
        var keyboard = new ReplyKeyboardMarkup(new[]
        {
            new[]
            {
                new KeyboardButton(options[0]),
                new KeyboardButton(options[1]),
            },
            new[]
            {
                new KeyboardButton(options[2]),
                new KeyboardButton(options[3]),
            }
        });
        return keyboard;
    }
    
    
    public static List<MultipleChoiceQuestion> ReadQuestionsFromFile(string fileName)
    {
        List<MultipleChoiceQuestion> questions = new List<MultipleChoiceQuestion>();

        try
        {
            string[] lines = File.ReadAllLines(fileName);

            for (int i = 0; i < lines.Length; i += 6)
            {
                string questionText = lines[i];

                var options = new List<string>();
                options.Add(lines[i + 1].Substring(3)); // Remove the leading 'a) ' from the option
                options.Add(lines[i + 2].Substring(3));
                options.Add(lines[i + 3].Substring(3));
                options.Add(lines[i + 4].Substring(3));

                MultipleChoiceQuestion question = new MultipleChoiceQuestion(questionText, GetAnswerOptions(options));
                questions.Add(question);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading questions from file: {ex.Message}");
        }

        return questions;
    }

}