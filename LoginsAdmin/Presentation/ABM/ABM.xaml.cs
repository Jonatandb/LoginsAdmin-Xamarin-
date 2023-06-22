using LoginsAdmin.Presentation.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LoginsAdmin.Presentation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ABM : ContentPage
    {
        public ABM()
        {
            InitializeComponent();
            (BindingContext as ABMViewModel).PerformIconVisualFeedback += OnPerformVisualFeedbackRequested;
        }

        private async void OnPerformVisualFeedbackRequested(object sender, EventArgs e)
        {
            string iconName = (e as IconTouchedEventArgs).IconName;
            var imageButton = FindByName("Copiar" + iconName) as Image;
            if (imageButton != null)
            {
                await imageButton.ScaleTo(1.8, 100);
                await imageButton.ScaleTo(1, 50);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current.MainPage.Navigation.PopAsync(false);
            return true;
        }
    }
}