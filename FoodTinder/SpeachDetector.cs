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
        public Action SwapPageCallback;
        public Action<string> PickedFood;

        Windows.Media.SpeechRecognition.SpeechRecognizer speachRecognizer;
        List<string> constraints;


        public SpeachDetector(List<string> constraintItems)
        {
            constraints = constraintItems;
            OnSearchStart();
        }

        public async Task OnSearchStart()
        {
            await this.StartMessageListener();
        }
        
        public async Task OnSearchStop()
        {
            await this.speachRecognizer.StopRecognitionAsync();
        }

        async Task StartMessageListener()
        {
            //   var messageDialog = new Windows.UI.Popups.MessageDialog("LISTENING", "Message Recieved");
            //   await messageDialog.ShowAsync();
            await this.StartListeningForConstraintAsync(
                new Windows.Media.SpeechRecognition.SpeechRecognitionListConstraint(constraints));
        }

        async Task StartListeningForConstraintAsync(Windows.Media.SpeechRecognition.ISpeechRecognitionConstraint constraint)
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
            await speachRecognizer.CompileConstraintsAsync();
            await speachRecognizer.ContinuousRecognitionSession.StartAsync();
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
                        SwapPageCallback();
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
