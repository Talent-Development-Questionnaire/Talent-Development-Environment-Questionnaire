using System;
using System.Text.RegularExpressions;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TDQ.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditAccountPage : ContentPage
    {
        private Models.CoachUser user;
        private Regex dobRegex = new Regex(@"^([0]?[1-9]|[1|2][0-9]|[3][0|1])[.\-|\/-]([0]?[1-9]|[1][0-2])[.\-|\/-]([0-9]{4}|[0-9]{2})$"); 

        public EditAccountPage()
        {
            InitializeComponent();

            // If the application is being run on iOS then display the cancel button, otherwise do not display this
            if (Device.RuntimePlatform == "iOS")
                BtnCancel.IsVisible = true;
            else
                BtnCancel.IsVisible = false;
        }

        public EditAccountPage(Models.CoachUser coach)
        {
            InitializeComponent();
            user = coach;

            if (user.Dob == " ")
                user.Dob = null; 

            if (Device.RuntimePlatform == "iOS")
                BtnCancel.IsVisible = true;
            else
                BtnCancel.IsVisible = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // If there is a profile image stored in the saved settings then retreive it, if not then set the image as the default account photo

            if (Utils.SavedSettings.AccountImageSettings == null)
                AccountImage.Source = ImageSource.FromResource("ic_account.png");
            else
                AccountImage.Source = Utils.SavedSettings.AccountImageSettings;
        }

        // When the profile image is pressed allow the user to select a new profile photo
        private async void AccountImage_Tapped(object sender, EventArgs e)
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Pick a photo"
            });

            //if user doesn't pick image stop executing button press
            if (result == null)
                return;

            AccountImage.Source = ImageSource.FromFile(result.FullPath);

            try
            {
                Utils.SavedSettings.AccountImageSettings = result.FullPath;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Debug", ex.Message, "OK");
            }
        }

        // If the save button is pressed then save the current details and send this to the database
        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            //Check email and name aren't left empty and email is valid
            if(!string.IsNullOrEmpty(entryName.Text) && !string.IsNullOrEmpty(entryEmail.Text) && Classes.GlobalFunctions.IsValidEmail(entryEmail.Text))
            {
                //Set user object name and email to entry text
                user.Name = entryName.Text;
                user.Email = entryEmail.Text;

                //If user doesn't select an item or select 'N/A' then set value to 'null'
                if (pickerGender.SelectedIndex == -1 || pickerGender.SelectedIndex == 3)
                    user.Gender = "%02%03";
                else
                    user.Gender = pickerGender.SelectedItem.ToString();

                //Check date of birth field is empty
                if (!string.IsNullOrEmpty(entryDOB.Text) && entryDOB.Text != "(null)")
                {
                    //Check the dob is in a valid format
                    if (dobRegex.IsMatch(entryDOB.Text))
                    {
                        //replace '/' with '-', '/' doesn't work with url
                        if (entryDOB.Text.Contains("/"))
                        {
                            entryDOB.Text = entryDOB.Text.Replace('/', '-');
                        }
                        user.Dob = entryDOB.Text;
                    }
                    else
                        await DisplayAlert("Error", "Dob format should either be dd-mm-yyyy or dd/mm/yyyy, please try again!", "OK");
                }
                else
                    user.Dob = "%02%03";
                //Send the editied details
                Classes.DatabaseController.EditAccountDetails(user.ID, user.Email, user.Name, user.Gender, user.Dob);
                Navigation.PopModalAsync();
                return;

            }

            await DisplayAlert("Error", "The email and name entry fields must not be left empty. Email may also be invalid, please fill them out to continue!", "OK");
        }

        // If the cancel button is pressed then go back to the previous page
        void BtnCancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}