using System;
using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
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
    class SpeechDetector
    {
        CoreDispatcher dispatcher;
        public Func<Task> SwapPageCallback;
        public Action<string> PickedFood;

        public Windows.Media.SpeechRecognition.SpeechRecognizer speechRecognizer;
        List<string> constraints;


        public SpeechDetector(List<string> constraintItems, CoreDispatcher newDispacher)
        {
            constraints = constraintItems;
            dispatcher = newDispacher;
        }

        public async Task Initialise()
        {
            speechRecognizer = new Windows.Media.SpeechRecognition.SpeechRecognizer();

            await OnSearchStart();
        }
        

        public async Task OnSearchStart()
        {
            await this.StartMessageListener();
        }
        
        public async Task OnSearchStop()
        {
            this.speechRecognizer.ContinuousRecognitionSession.ResultGenerated -= OnSpeechResult;
                
            speechRecognizer.Dispose();
            speechRecognizer = null;
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
            this.speechRecognizer.ContinuousRecognitionSession.ResultGenerated += OnSpeechResult;

            speechRecognizer.Constraints.Clear();
            speechRecognizer.Constraints.Add(constraint);
            await speechRecognizer.CompileConstraintsAsync() ;
            await speechRecognizer.ContinuousRecognitionSession.StartAsync();
            
        }

        async void OnSpeechResult(
          Windows.Media.SpeechRecognition.SpeechContinuousRecognitionSession sender,
          Windows.Media.SpeechRecognition.SpeechContinuousRecognitionResultGeneratedEventArgs args)
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
             {
                 var a = constraints;

                 if (args.Result.Constraint == null)
                     return;

                 if ((args.Result.Confidence == Windows.Media.SpeechRecognition.SpeechRecognitionConfidence.High) ||
                     (args.Result.Confidence == Windows.Media.SpeechRecognition.SpeechRecognitionConfidence.Medium))
                 {
                    //var messageDialog = new Windows.UI.Popups.MessageDialog(args.Result.Text, "Message Recieved");
                    //await messageDialog.ShowAsync();

                    if (constraints.Contains(args.Result.Text.ToLower()))
                     {
                         if (SwapPageCallback != null)
                         {
                             await SwapPageCallback();
                         }
                         else
                         {
                             PickedFood(args.Result.Text);
                         }

                        //AddVote(speechRecogniztionResult.Text);
                    }
                 }
             });
            
        }

    }
}
