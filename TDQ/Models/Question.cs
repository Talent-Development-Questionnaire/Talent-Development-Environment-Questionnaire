using System;
using Newtonsoft.Json;
namespace TDQ.Models
{
    public class Question : Questionnaire
    {
        [JsonProperty ("question_no")]
        public string QuestionNo { get; set; }
        public string QuestionText { get; set; }
        [JsonProperty ("score")]
        public string Answer { get; set; }
    }
}
