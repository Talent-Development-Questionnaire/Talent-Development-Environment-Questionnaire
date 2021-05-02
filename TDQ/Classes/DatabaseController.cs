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
        //Encrypt user password before sending to database
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
        //Generate a One Time Password
        public static string GenerateOTP()
        {
            //Set the characters and numbers to be used within the password
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = numbers;
            //Append preset characters into single string
            characters += alphabets + small_alphabets + numbers;
            int length = 12; //set max length of OTP
            string otp = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    //Get random character from characters string
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                //appened character to otp as long as index is not -1
                otp += character;
            }
            return otp;
        }
        //Base function to connect to server through HTTP
        private static HttpWebResponse ConnectToClient(string prefix)
        {
            //Set url to ip + passed in prefix
            string url = $"{Constants.ip}{prefix}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);//create the request to the server
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();//get the response from the server
            //return the response to the calling function
            return response;
        }
        //Format the response to a string
        private static string FormatResponse(HttpWebResponse response)
        {
            //Use stream from respnse of the server
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                //read from stream and set string to value
                string responseString = reader.ReadToEnd();
                //Remove unecessary characters from string
                responseString = responseString.Replace("(", string.Empty);
                responseString = responseString.Replace(")", string.Empty);
                responseString = responseString.TrimEnd(',');

                return responseString;
            }
        }
        //Get a list of the questionnaire questions
        private static string[] FormatFileResponse(HttpWebResponse response)
        {
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                string responseString = reader.ReadToEnd();
                //Create array of questions
                string[] questions = responseString.Split(new string[] { "\n" }, StringSplitOptions.None);

                return questions;
            }
        }
        //Check if inputted email already exists within the database
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
        //Insert new user data into the database 
        public static bool InsertNewUser(string email, string password, string name, string otp)
        {
            //Encrypt user password
            password = HashPassword(password);
            HttpWebResponse response = ConnectToClient($"coach/addCoach/{email}/{password}/{name}/{otp}");

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                string responseString = reader.ReadToEnd();

                return Convert.ToBoolean(responseString);
            }
        }
        //Check if account details exist in database, return true if it doesn't exist
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
        //Retrive user details from the database
        public static CoachUser GetUserDetails(string email)
        {
            HttpWebResponse response = ConnectToClient($"coach/getUser/{email}");

            //format the response into a string
            string responseString = FormatResponse(response);
            responseString = responseString.Replace("\\x02\\x03", " ");
            //Deserialise the json format string into the coach user object
            var user = JsonConvert.DeserializeObject<CoachUser>(responseString);
            //Check if user is not null
            if (user != null)
                return user;

            return null;
        }
        //Send edited user details to the database
        public static void EditAccountDetails(string id, string email, string name, string gender, string dob)
        {
            ConnectToClient($"coach/editUser/{id}/{email}/{name}/{gender}/{dob}");
        }
        //Generate the specific questionnaire questions 
        public static List<Question> GenerateQuestions(string email, string otp)
        {
            //Gets questionnaire linked to the inputted athlete details
            var questionnnaire = VerifyAthlete(email, otp);
            if (questionnnaire != null)
            {
                HttpWebResponse response = ConnectToClient($"player/getQuestions/{questionnnaire.ID}");
                List<Question> questionsList = new List<Question>();
                var questions = FormatFileResponse(response);
                //Add each question to list with ID and Question Text
                foreach (var item in questions)
                {
                    questionsList.Add(new Question
                    {
                        Type = questionnnaire.Type,
                        ID = questionnnaire.ID,
                        QuestionText = item
                    });
                }
                //return the questionnaire
                return questionsList;
            }
            return null;
        }
        //Verify the inputted details match the database and return the linked questionnaire
        public static Questionnaire VerifyAthlete(string email, string otp)
        {
            HttpWebResponse response = ConnectToClient($"player/verifyAthlete/{email}/{otp}");

            string responseString = FormatResponse(response);
            //return questio nnaire if user details match what is in the database
            if (responseString != "false")
            {
                var questionnaire = JsonConvert.DeserializeObject<Questionnaire>(responseString);
                return questionnaire;
            }

            return null;
        }
        //Verify coach email using a One Time Password
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
        //Create new row in table for each athlete and assign them to the newly created questionnaire
        public static void AssignAthletesQuestionnaires(string name, string type, string email, string athlete, string otp)
        {
            HttpWebResponse response = ConnectToClient($"coach/createQuestionnaire/{name}/{type}/{email}/{athlete}/{otp}");
            response.Close();
        }
        //Delete User account details from the database
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
        //Send the completed questionnaire to be stored in the database
        public static void SendCompletedQuestionnaire(Question question, int questionNumber, string email)
        {
            var response = ConnectToClient($"player/submitQuestion/{question.ID}/{questionNumber}/{question.Answer}");
            response.Close();

            //Delete athlete from database when questionnaire is completed
            switch (question.Type)
            {
                case "59":
                    if(questionNumber == 59)
                        response = ConnectToClient($"player/deleteAthleteRow/{email}/{question.ID}");
                    break;
                case "28":
                    if (questionNumber == 28)
                        response = ConnectToClient($"player/deleteAthleteRow/{email}/{question.ID}");
                    break;
            }
            response.Close();
        }
        //Update the number of times the questionnaire has been completed
        public static void UpdateQuestionnaireCompletions(Question question)
        {
            var response = ConnectToClient($"player/updateCompletionCount/{question.ID}");
            response.Close();
        }
        //Get all the questionnaires linked to the coach account
        public static List<Questionnaire> GetQuestionnaires(string email)
        {
            //Initialise 2 lists, copy one to another to avoid crashing
            List<Questionnaire> oldQuestionnaireList = new List<Questionnaire>();
            List<Questionnaire> newQuestionnaireList = new List<Questionnaire>();

            //get the account details associated to the email logged into the app
            var user = GetUserDetails(email);
            if (user == null)
                return null;
            //Get all questionnaires linked to the user's ID
            HttpWebResponse response = ConnectToClient($"coach/getQuestionnaires/{user.ID}");

            string responseString = FormatResponse(response);
            //Split the string into a list by items within curly braces
            var list = Regex.Split(responseString, @"({.*?})");
            // Run if there are questionnaires linked to the user account
            if (responseString != "false")
            {
                foreach (var item in list)
                    //Check that item is not empty or is an unwanted value found in the string
                    if (!string.IsNullOrEmpty(item) && item != ", ")
                        //Add questionnaire to list
                        oldQuestionnaireList.Add(JsonConvert.DeserializeObject<Questionnaire>(item));

                foreach (var item in oldQuestionnaireList)
                    //Add questionnaires from original list populated with the questions
                    newQuestionnaireList.Add(GetQuestions(item));

                return newQuestionnaireList;
            }

            return null;
        }
        //Populate Questionnaire with questions and their answers
        private static Questionnaire GetQuestions(Questionnaire questionnaire)
        {
            HttpWebResponse response = ConnectToClient($"coach/getQuestionnaireQuestions/{questionnaire.ID}");

            string responseString = FormatResponse(response);
            var list = Regex.Split(responseString, @"({.*?})");

            if (responseString != "false")
            {
                //Initialise question list for questionnaire
                questionnaire.Questions = new List<Question>();

                //Add question to list if values are correct
                foreach (var item in list)
                    if (!string.IsNullOrEmpty(item) && item != ", ")
                        questionnaire.Questions.Add(JsonConvert.DeserializeObject<Question>(item));
            }

            //Assign questions text to each of the questions in the list
            questionnaire.Questions = GetQuestionText(questionnaire);
            return questionnaire;
        }
        //Get the text for each question and add it to the question object
        private static List<Question> GetQuestionText(Questionnaire questionnaire)
        {
            List<Question> newQuestions = new List<Question>();
            HttpWebResponse response = ConnectToClient($"coach/getQuestions/{questionnaire.ID}");

            var questions = FormatFileResponse(response);

            int index = 0;
            foreach (var item in questions)
                if (!string.IsNullOrEmpty(item) && questionnaire.Questions != null)
                {
                    //Loop through each question in list 
                    var question = questionnaire.Questions[index];
                    //Run if question does not already exist in list
                    if (!newQuestions.Contains(question))
                    {
                        //Increment index
                        index++;
                        //Assign the average based on overall score / the amount of completions
                        double score = Convert.ToDouble(question.Answer) / Convert.ToDouble(questionnaire.Completions);
                        //Add new values to the question object
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
        //Send the inputted athlete info to the database
        public static bool SendUserDetails(string name, string sport, string academy, string age, string gender)
        {
            var response = ConnectToClient($"player/sendAthleteInfo/{academy}/{sport}/{name}/{age}/{gender}");

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                string responseString = reader.ReadToEnd();
                //Return true or false to check if inputting data went okay
                return Convert.ToBoolean(responseString);
            }
        }
    }
}