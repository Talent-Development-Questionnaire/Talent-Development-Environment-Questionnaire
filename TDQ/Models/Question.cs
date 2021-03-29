using System;
namespace TDQ.Models
{
    public class Question : Questionnaire
    {
        public string QuestionText { get; set; }
        public string Answer { get; set; }
    }
}
