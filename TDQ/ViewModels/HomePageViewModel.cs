using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TDQ.Models;

namespace TDQ.ViewModels
{
    public class HomePageViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName]string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public HomePageViewModel()
        {
            UpdateQuestionnareList();
        }

        bool messageText = false;
        ObservableCollection<Questionnaire> questionnaireList;

        public bool MessageTextVisibility
        {
            get { return messageText; }
            set
            {
                messageText = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Questionnaire> QuestionnaireList
        {
            get { return questionnaireList; }
            set
            {
                questionnaireList = value;
                OnPropertyChanged();
            }
        }

        public void UpdateQuestionnareList()
        {
            if (!string.IsNullOrEmpty(Utils.SavedSettings.LoginSettings))
            {
                List<Questionnaire> tempList = Classes.DatabaseController.GetQuestionnaires(Utils.SavedSettings.LoginSettings);
                if (tempList != null)
                {
                    messageText = false;
                    questionnaireList = new ObservableCollection<Questionnaire>(tempList);
                    return;
                }
                messageText = true;
            }
        }
    }
}