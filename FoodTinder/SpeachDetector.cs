using System;
using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
//using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace FoodTinder
{
    class SpeachDetector
    {
        public Func<Task> SwapPageCallback;
        public Action<string> PickedFood;

        Windows.Media.SpeechRecognition.SpeechRecognizer speachRecognizer;
        List<string> constraints;


        public SpeachDetector(List<string> constraintItems)
        {
            constraints = constraintItems;
        }

        public async Task Initialise()
        {

            await OnSearchStart();
        }

        public async Task OnSearchStart()
        {
            await this.StartMessageListener();
        }
        
        public async Task OnSearchStop()
        {
            if (this.speachRecognizer.State == Windows.Media.SpeechRecognition.SpeechRecognizerState.Capturing) { 
                await this.speachRecognizer.StopRecognitionAsync();
            }
        }

        public async Task StartMessageListener()
        {
            //   var messageDialog = new Windows.UI.Popups.MessageDialog("LISTENING", "Message Recieved");
            //   await messageDialog.ShowAsync();
            await this.StartListeningForConstraintAsync(
                new Windows.Media.SpeechRecognition.SpeechRecognitionListConstraint(constraints));
        }

        public async Task StartListeningForConstraintAsync(Windows.Media.SpeechRecognition.ISpeechRecognitionConstraint constraint)
        {
            if (speachRecognizer == null)
            {
                this.speachRecognizer = new Windows.Media.SpeechRecognition.SpeechRecognizer();
                this.speachRecognizer.ContinuousRecognitionSession.ResultGenerated += OnSpeechResult;
            }
            else
            {
                await this.speachRecognizer.ContinuousRecognitionSession.StopAsync();
            }
            speachRecognizer.Constraints.Clear();
            speachRecognizer.Constraints.Add(constraint);
            await speachRecognizer.CompileConstraintsAsync() ;
            await speachRecognizer.ContinuousRecognitionSession.StartAsync() ;
        }

        async void OnSpeechResult(
          Windows.Media.SpeechRecognition.SpeechContinuousRecognitionSession sender,
          Windows.Media.SpeechRecognition.SpeechContinuousRecognitionResultGeneratedEventArgs args)
        {
            if ((args.Result.Confidence == Windows.Media.SpeechRecognition.SpeechRecognitionConfidence.High) ||
                (args.Result.Confidence == Windows.Media.SpeechRecognition.SpeechRecognitionConfidence.Medium))
            {
                //var messageDialog = new Windows.UI.Popups.MessageDialog(args.Result.Text, "Message Recieved");
                //await messageDialog.ShowAsync();

                if (constraints.Contains(args.Result.Text.ToLower()))
                {
                    if(SwapPageCallback != null)
                    {
                        await SwapPageCallback();
                    }
                    else
                    {
                        PickedFood(args.Result.Text);
                    }
                    
                    //AddVote(speachRecogniztionResult.Text);
                }
            }
        }

    }
}
