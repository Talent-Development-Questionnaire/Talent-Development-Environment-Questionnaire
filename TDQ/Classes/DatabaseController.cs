using System;
using System.Net;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Newtonsoft.Json;
using TDQ.Models;

namespace TDQ.Classes
{
    public class DatabaseController
    {
        private static string HashPassword(string pass)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(pass));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public static string GenerateOTP()
        {
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = numbers;

            characters += alphabets + small_alphabets + numbers;
            int length = 12;
            string otp = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            return otp;
        }
        private static HttpWebResponse ConnectToClient(string prefix)
        {
            string url = $"{Constants.ip}{prefix}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            return response;
        }
        private static string FormatResponse(HttpWebResponse response)
        {
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);

                string responseString = reader.ReadToEnd();

                responseString = responseString.Replace("(", string.Empty);
                responseString = responseString.Replace(")", string.Empty);
                responseString = responseString.TrimEnd(',');

                return responseString;
            }
        }
        private static string[] FormatFileResponse(HttpWebResponse response)
        {
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                string responseString = reader.ReadToEnd();
                string[] questions = responseString.Split(new string[] { "\n" }, StringSplitOptions.None);

                return questions;
            }
        }
        //Method to check if inputted email already exists within the database
        public static bool CheckEmailExists(string email)
        {
            var response = ConnectToClient($"coach/checkEmail/{email}");

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                string responseString = reader.ReadToEnd();
                return Convert.ToBoolean(responseString);
            }
        }
        //Method to insert new user data into the database 
        public static bool InsertNewUser(string email, string password, string name, string otp)
        {
            password = HashPassword(password);
            HttpWebResponse response = ConnectToClient($"coach/addCoach/{email}/{password}/{name}/{otp}");

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                string responseString = reader.ReadToEnd();
                return Convert.ToBoolean(responseString);
            }
        }
        //Method to check if account details exist in database
        public static bool AccountCheck(string email, string password)
        {
            password = HashPassword(password);
            HttpWebResponse response = ConnectToClient($"coach/checkAccountExists/{email}/{password}");

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                string responseString = reader.ReadToEnd();
                return Convert.ToBoolean(responseString);
            }
        }
        public static CoachUser GetUserDetails(string email)
        {
            HttpWebResponse response = ConnectToClient($"coach/getUser/{email}");

            string responseString = FormatResponse(response);
            var user = JsonConvert.DeserializeObject<CoachUser>(responseString);

            if (user != null)
                return user;

            return null;
        }
        public static void EditAccountDetails(string id, string email, string name, string gender, string dob)
        {
            ConnectToClient($"coach/editUser/{id}/{email}/{name}/{gender}/{dob}");
        }
        public static List<Question> GenerateQuestions(string email, string otp)
        {
            var questionnnaire = VerifyAthlete(email, otp);
            if (questionnnaire != null)
            {
                HttpWebResponse response = ConnectToClient($"player/getQuestions/{questionnnaire.ID}");
                List<Question> questionsList = new List<Question>();
                var questions = FormatFileResponse(response);

                foreach (var item in questions)
                {
                    questionsList.Add(new Question
                    {
                        ID = questionnnaire.ID,
                        QuestionText = item
                    });
                }
                return questionsList;
            }
            return null;
        }
        public static Questionnaire VerifyAthlete(string email, string otp)
        {
            HttpWebResponse response = ConnectToClient($"player/verifyAthlete/{email}/{otp}");

            string responseString = FormatResponse(response);
            var questionnaire = JsonConvert.DeserializeObject<Questionnaire>(responseString);

            if (responseString != "false")
                return questionnaire;

            return null;
        }
        public static bool VerifyCoachAccount(string email, string otp)
        {
            HttpWebResponse response = ConnectToClient($"coach/verifyOTP/{email}/{otp}");

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                string responseString = reader.ReadToEnd();

                return Convert.ToBoolean(responseString);
            }
        }
        public static void AssignAthletesQuestionnaires(string name, string type, string email, string athlete, string otp)
        {
            HttpWebResponse response = ConnectToClient($"coach/createQuestionnaire/{name}/{type}/{email}/{athlete}/{otp}");
            response.Close();
        }
        public static bool DeleteUser(string email, string password)
        {
            password = HashPassword(password);
            HttpWebResponse response = ConnectToClient($"coach/deleteCoach/{email}/{password}");

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                string responseString = reader.ReadToEnd();
                return Convert.ToBoolean(responseString);
            }
        }
        public static void SendCompletedQuestionnaire(Question question, int questionNumber)
        {
            var response = ConnectToClient($"player/submitQuestion/{question.ID}/{questionNumber}/{question.Answer}");
            response.Close();
        }
        public static void UpdateQuestionnaireCompletions(Question question)
        {
            var response = ConnectToClient($"player/updateCompletionCount/{question.ID}");
            response.Close();
        }
        public static List<Questionnaire> GetQuestionnaires(string email)
        {
            List<Questionnaire> oldQuestionnaireList = new List<Questionnaire>();
            List<Questionnaire> newQuestionnaireList = new List<Questionnaire>();

            var user = GetUserDetails(email);
            if (user == null)
                return null;

            HttpWebResponse response = ConnectToClient($"coach/getQuestionnaires/{user.ID}");

            string responseString = FormatResponse(response);
            var list = Regex.Split(responseString, @"({.*?})");

            if (responseString != "false")
            {
                foreach (var item in list)
                    if (!string.IsNullOrEmpty(item) && item != ", ")
                        oldQuestionnaireList.Add(JsonConvert.DeserializeObject<Questionnaire>(item));

                foreach (var item in oldQuestionnaireList)
                    newQuestionnaireList.Add(GetQuestions(item));

                return newQuestionnaireList;
            }

            return null;
        }
        private static Questionnaire GetQuestions(Questionnaire questionnaire)
        {
            HttpWebResponse response = ConnectToClient($"coach/getQuestionnaireQuestions/{questionnaire.ID}");

            string responseString = FormatResponse(response);
            var list = Regex.Split(responseString, @"({.*?})");

            if (responseString != "false")
            {
                questionnaire.Questions = new List<Question>();

                foreach (var item in list)
                    if (!string.IsNullOrEmpty(item) && item != ", ")
                        questionnaire.Questions.Add(JsonConvert.DeserializeObject<Question>(item));
            }

            questionnaire.Questions = GetQuestionText(questionnaire);
            return questionnaire;
        }
        private static List<Question> GetQuestionText(Questionnaire questionnaire)
        {
            List<Question> newQuestions = new List<Question>();
            HttpWebResponse response = ConnectToClient($"coach/getQuestions/{questionnaire.ID}");

            var questions = FormatFileResponse(response);

            int index = 0;
            foreach (var item in questions)
                if (!string.IsNullOrEmpty(item) && questionnaire.Questions != null)
                {
                    var question = questionnaire.Questions[index];
                    if (!newQuestions.Contains(question))
                    {
                        index++;
                        double score = Convert.ToDouble(question.Answer) / Convert.ToDouble(questionnaire.Completions);
                        newQuestions.Add(new Question
                        {
                            QuestionNo = index.ToString(),
                            QuestionText = item,
                            Answer = score.ToString("0.#")
                        });
                    }
                }

            return newQuestions;
        }
        public static bool SendUserDetails(string name, string sport, string academy, string age, string gender)
        {
            var response = ConnectToClient($"player/sendAthleteInfo/{academy}/{sport}/{name}/{age}/{gender}");

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                string responseString = reader.ReadToEnd();

                return Convert.ToBoolean(responseString);
            }
        }
    }
}