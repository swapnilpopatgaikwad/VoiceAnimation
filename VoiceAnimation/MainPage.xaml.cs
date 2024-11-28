namespace VoiceAnimation
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void AmplitudeSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            VoiceWeave.UpdateAmplitude(e.NewValue);
        }

        private void AmplitudeSlider_ValueChanged1(object sender, ValueChangedEventArgs e)
        {
            // Map slider value to amplitude
            VoiceRecorder.UpdateAmplitude(e.NewValue);
        }

        private void StartAnimation_Clicked(object sender, EventArgs e)
        {
            BarAnimationView.StartAnimation();
        }

        private void StopAnimation_Clicked(object sender, EventArgs e)
        {
            BarAnimationView.StopAnimation();
        }
    }

}
