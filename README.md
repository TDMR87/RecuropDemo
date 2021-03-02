# RecuropDemo
A WPF timer application created with recurring operations from the Recurop library.

https://www.nuget.org/packages/Recurop/
https://github.com/TDMR87/Recurop

Layout:
The MainWindowViewModel contains the code logic.
The MainWindow.xaml UI elements are bound to the MainWindowViewModel properties. 

Using the Recurop .NET Standard class library you can create easily managed, timer based, recurring background operations. 
You can bind XAML UI elements to the properties of the recurring operations and make the UI elements respond dynamically to the status of
the recurring operation.

```xaml
<Button Command="{Binding StartTimerCommand}" IsEnabled="{Binding RecurringTimerOperation.CanBeStarted}" Content="Start" />
<Button Command="{Binding LapTimeCommand}" IsEnabled="{Binding RecurringTimerOperation.IsRecurring}" Content="Lap" />
<Button Command="{Binding PauseTimerCommand}" IsEnabled="{Binding RecurringTimerOperation.IsRecurring}" Content="Pause" />
<Button Command="{Binding ContinueTimerCommand}" IsEnabled="{Binding RecurringTimerOperation.IsPaused}" Content="Continue" />
<Button Command="{Binding CancelTimerCommand}" IsEnabled="{Binding RecurringTimerOperation.IsRecurring}" Content="Cancel" />
<Button Command="{Binding ThrowCommand}" IsEnabled="{Binding RecurringTimerOperation.IsExecuting}" Content="Throw" />
```

```xaml
<TextBlock Text="{Binding RecurringTimerOperation.Status, StringFormat=Operation status: {0}}"/>
<TextBlock Text="{Binding RecurringTimerOperation.CanBeStarted, StringFormat=Can be started: {0}}"/>
<TextBlock Text="{Binding RecurringTimerOperation.IsRecurring, StringFormat=Is recurring: {0}}"/>
<TextBlock Text="{Binding RecurringTimerOperation.LastRunStart, StringFormat=Last run started: {0:yyyy'-'MM'-'dd'T'HH':'mm':'ss}}"/>
<TextBlock Text="{Binding RecurringTimerOperation.LastRunFinish, StringFormat=Last run finished: {0:yyyy'-'MM'-'dd'T'HH':'mm':'ss}}"/>
```
