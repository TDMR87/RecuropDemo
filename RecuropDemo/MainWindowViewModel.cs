using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;
using System.Windows;
using Recurop;

namespace RecuropDemo
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        // Public properties
        public RecurringOperation RecurringTimerOperation { get; }
        public RecurringOperation RecurringVisibilityOperation { get; }
        public ObservableCollection<TimeSpan> LapTimes { get; set; }
        public ObservableCollection<string> ExceptionMessages { get; set; }
        public TimeSpan DisplayTimer
        {
            get => _displayTimer ?? TimeSpan.Zero;
            private set
            {
                _displayTimer = value;
                NotifyPropertyChanged();
            }
        }
        public Visibility TimerVisibility
        {
            get => timerVisibility;
            set
            {
                timerVisibility = value;
                NotifyPropertyChanged();
            }
        }

        // Private fields
        private int elapsedSeconds;
        private bool throwException;
        private TimeSpan? _displayTimer;
        private Visibility timerVisibility;

        // Viewmodel construtor
        public MainWindowViewModel()
        {
            // Initialize collections
            LapTimes = new ObservableCollection<TimeSpan>();
            ExceptionMessages = new ObservableCollection<string>();

            // Initialize the timer operation
            RecurringTimerOperation = new RecurringOperation(name: "TimerOperation");
            RecurringTimerOperation.OperationFaulted += OnOperationFaulted;
            RecurringTimerOperation.StatusChanged += OnOperationStatusChanged;

            // Initialize the blink operation
            RecurringVisibilityOperation = new RecurringOperation(name: "BlinkOperation");
        }

        /// <summary>
        /// Method for handling the OperationFaulted event.
        /// </summary>
        /// <param name="ex"></param>
        private void OnOperationFaulted(Exception ex)
        {
            // Because this exception event handler delegate is invoked on a
            // threadpool thread, we can only modify the list on the
            // thread it was created on (in this case the UI thread).
            App.Current.Dispatcher.Invoke(() =>
            {
                ExceptionMessages.Add(ex.Message);
            });
        }

        /// <summary>
        /// Method for handling the StatusChanged event.
        /// </summary>
        private void OnOperationStatusChanged()
        {
            if (RecurringTimerOperation.Status == RecurringOperationStatus.Cancelled)
            {

            }
        }

        public ICommand StartTimerCommand => new RelayCommand(() =>
        {
            // If operation is already started, do nothing
            //if (!RecurringTimerOperation.CanBeStarted) return;

            // Start a recurring operation using a lambda style Action.
            // Alternatevly, you could call any named method instead.
            RecurringOperationsManager.Instance.StartRecurring(RecurringTimerOperation, TimeSpan.FromSeconds(1), () =>
            {
                // Increment elapsed seconds
                elapsedSeconds++;

                // Set value to the DisplayTimer
                DisplayTimer = TimeSpan.FromSeconds(elapsedSeconds);

                // Simulate a little work
                Thread.Sleep(500);

                // If an exception should be thrown
                if (throwException)
                {
                    // Disable it
                    throwException = false;

                    // Throw an exception inside the recurring operation
                    throw new InvalidOperationException(
                        "An exception was thrown inside the recurring operation.");
                }
            });
        });

        public ICommand PauseTimerCommand => new RelayCommand(() =>
        {
            // Pause the timer operation
            RecurringOperationsManager.Instance.PauseRecurring(RecurringTimerOperation);

            // Start a blink operation
            RecurringOperationsManager.Instance.StartRecurring(RecurringVisibilityOperation, TimeSpan.FromSeconds(0.5), () =>
            {
                switch (TimerVisibility)
                {
                    case Visibility.Visible:
                        TimerVisibility = Visibility.Hidden;
                        break;
                    case Visibility.Hidden:
                        TimerVisibility = Visibility.Visible;
                        break;
                    case Visibility.Collapsed:
                        break;
                    default:
                        break;
                }
            });
        });

        public ICommand ContinueTimerCommand => new RelayCommand(() =>
        {
            // Resume the timer operation
            RecurringOperationsManager.Instance.ResumeRecurring(RecurringTimerOperation);

            // Cancel the blink operation
            RecurringOperationsManager.Instance.CancelRecurring(RecurringVisibilityOperation);

            TimerVisibility = Visibility.Visible;
        });

        public ICommand CancelTimerCommand => new RelayCommand(() =>
        {
            RecurringOperationsManager.Instance.CancelRecurring(RecurringTimerOperation);

            DisplayTimer = TimeSpan.Zero;

            elapsedSeconds = 0;
        });

        public ICommand ThrowCommand => new RelayCommand(() =>
        {
            throwException = true;
        });

        public ICommand LapTimeCommand => new RelayCommand(() =>
        {
            LapTimes.Add(DisplayTimer);
        });

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
