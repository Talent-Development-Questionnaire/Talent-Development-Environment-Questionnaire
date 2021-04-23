using System;
using System.Net;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Newtonsoft.Json;

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

        //Method to check if inputted email already exists within the database
        public static bool CheckEmailExists(string email)
        {
            string url = $"{Constants.ip}coach/checkEmail/{email}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

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
            string url = $"{Constants.ip}coach/addCoach/{email}/{password}/{name}/{otp}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

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
            string url = $"{Constants.ip}coach/checkAccountExists/{email}/{password}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                string responseString = reader.ReadToEnd();
                return Convert.ToBoolean(responseString);
            }
        }

        public static Models.CoachUser GetUserDetails(string email)
        {
            string url = $"{Constants.ip}coach/getUser/{email}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                string responseString = reader.ReadToEnd();
                responseString = responseString.Replace("(", string.Empty);
                responseString = responseString.Replace(")", string.Empty);
                responseString = responseString.TrimEnd(',');
                var user = JsonConvert.DeserializeObject<Models.CoachUser>(responseString);

                if (user != null)
                    return user;
                else
                    return null;
            }
        }

        public static void EditAccountDetails(string id, string email, string name, string gender, string dob)
        {
            string url = $"{Constants.ip}coach/editUser/{id}/{email}/{name}/{gender}/{dob}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        }

        public static List<Models.Question> GenerateQuestions(string email, string otp)
        {
            var questionnnaire = VerifyAthlete(email, otp);
            if (questionnnaire != null)
            {
                string url = $"{Constants.ip}player/getQuestions/{questionnnaire.ID}";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream);
                    string responseString = reader.ReadToEnd();
                    string[] questions = responseString.Split(new string[] { "\n" }, StringSplitOptions.None);
                    List<Models.Question> questionsList = new List<Models.Question>();

                    foreach (var item in questions)
                    {
                        questionsList.Add(new Models.Question
                        {
                            ID = questionnnaire.ID,
                            QuestionText = item
                        });
                    }
                    return questionsList;
                }
            }
            return null;
        }

        public static Models.Questionnaire VerifyAthlete(string email, string otp)
        {
            string url = $"{Constants.ip}player/verifyAthlete/{email}/{otp}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                string responseString = reader.ReadToEnd();
                responseString = responseString.Replace("(", string.Empty);
                responseString = responseString.Replace(")", string.Empty);
                responseString = responseString.TrimEnd(',');
                var questionnaire = JsonConvert.DeserializeObject<Models.Questionnaire>(responseString);

                if (responseString != "false")
                    return questionnaire;
            }
            return null;
        }

        public static bool VerifyCoachAccount(string email, string otp)
        {
            string url = $"{Constants.ip}coach/verifyAccount/{email}/{otp}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                string responseString = reader.ReadToEnd();

                return Convert.ToBoolean(responseString);
            }
        }

        public static Models.Questionnaire AssignAthletesQuestionnaires(string name, string type, string email, string athlete, string otp, int flag)
        {
            string url = $"{Constants.ip}coach/createQuestionnaire/{name}/{type}/{email}/{athlete}/{otp}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (flag == 1)
            {
                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream);
                    string responseString = reader.ReadToEnd();
                    responseString = responseString.Replace("(", string.Empty);
                    responseString = responseString.Replace(")", string.Empty);
                    responseString = responseString.TrimEnd(',');
                    var questionnaire = JsonConvert.DeserializeObject<Models.Questionnaire>(responseString);

                    if (questionnaire != null)
                        return questionnaire;
                    else
                        return null;
                }
            }

            return null;
        }

        public static bool DeleteUser(string email, string password)
        {
            password = HashPassword(password);

            string url = $"{Constants.ip}/coach/deleteCoach/{email}/{password}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                string responseString = reader.ReadToEnd();
                return Convert.ToBoolean(responseString);
            }
        }

        public static void SendCompletedQuestionnaire(Models.Question question, int questionNumber)
        {
            string url = $"{Constants.ip}/player/submitQuestion/{question.ID}/{questionNumber}/{question.Answer}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            request.Abort();
        }

        public static void UpdateQuestionnaireCompletions(Models.Question question)
        {
            string url = $"{Constants.ip}/player/updateCompletionCount/{question.ID}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            request.Abort();
        }

        public static List<Models.Questionnaire> GetQuestionnaires(string email)
        {
            List<Models.Questionnaire> oldQuestionnaireList = new List<Models.Questionnaire>();
            List<Models.Questionnaire> newQuestionnaireList = new List<Models.Questionnaire>();
            var user = GetUserDetails(email);

            string url = $"{Constants.ip}coach/getQuestionnaires/{user.ID}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);

                string responseString = reader.ReadToEnd();

                responseString = responseString.Replace("(", string.Empty);
                responseString = responseString.Replace(")", string.Empty);
                responseString = responseString.TrimEnd(',');
                var list = Regex.Split(responseString, @"({.*?})");

                if (responseString != "false")
                {
                    foreach (var item in list)
                        if (!string.IsNullOrEmpty(item) && item != ", ")
                            oldQuestionnaireList.Add(JsonConvert.DeserializeObject<Models.Questionnaire>(item));
                    foreach (var item in oldQuestionnaireList)
                        newQuestionnaireList.Add(GetQuestions(item));
                    return newQuestionnaireList;
                }
            }
            return null;
        }

        private static Models.Questionnaire GetQuestions(Models.Questionnaire questionnaire)
        {
            string url = $"{Constants.ip}coach/getQuestionnaireQuestions/{questionnaire.ID}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);

                string responseString = reader.ReadToEnd();

                responseString = responseString.Replace("(", string.Empty);
                responseString = responseString.Replace(")", string.Empty);
                responseString = responseString.TrimEnd(',');
                var list = Regex.Split(responseString, @"({.*?})");

                if (responseString != "false")
                {
                    questionnaire.Questions = new List<Models.Question>();

                    foreach (var item in list)
                        if (!string.IsNullOrEmpty(item) && item != ", ")
                            questionnaire.Questions.Add(JsonConvert.DeserializeObject<Models.Question>(item));
                }
                questionnaire.Questions = GetQuestionText(questionnaire);
                return questionnaire;
            }

        }

        private static List<Models.Question> GetQuestionText(Models.Questionnaire questionnaire)
        {

            List<Models.Question> newQuestions = new List<Models.Question>();
            string url = $"{Constants.ip}coach/getQuestions/{questionnaire.ID}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                string responseString = reader.ReadToEnd();
                string[] questions = responseString.Split(new string[] { "\n" }, StringSplitOptions.None);

                int index = 0;
                foreach (var item in questions)
                    if (!string.IsNullOrEmpty(item) && questionnaire.Questions != null)
                    { 
                        var question = questionnaire.Questions[index];
                        if (!newQuestions.Contains(question))
                        {
                            index++;
                            double score = Convert.ToDouble(question.Answer) / Convert.ToDouble(questionnaire.Completions);
                            newQuestions.Add(new Models.Question
                            {
                                QuestionNo = index.ToString(),
                                QuestionText = item,
                                Answer = score.ToString()
                            });
                        }
                            
                    }

                return newQuestions;
            }
        }
    }
}