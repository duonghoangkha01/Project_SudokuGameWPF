using SudokuGameWPF.ViewModel.CustomEventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SudokuGameWPF.ViewModel
{
    internal class GameTimer
    {
        #region . Variables, Constants, And other Declarations .

        #region . Constants .

        private string initialValue = "00:00:00";
        private string timeFormat = "hh\\:mm\\:ss";
        private Int32 interval = 1000;

        #endregion

        #region . Variables .

        private DateTime startTime;
        private Timer timer;

        #endregion

        #region . Other Declarations .

        internal event EventHandler<GameTimerEventArgs> GameTimerEvent;

        #endregion

        #endregion

        #region . Constructors .

        /// <summary>
        /// Initializes a new instance of the GameTimer class.
        /// </summary>
        internal GameTimer()
        {
            ElapsedTime = initialValue;                    // Initialize the elapsed time value
        }

        #endregion

        #region . Properties: Public Read-only .

        /// <summary>
        /// Gets the elapsed time.
        /// </summary>
        internal string ElapsedTime { get; private set; }

        #endregion

        #region . Event Handlers .

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ComputeElapsedTime();                   // Compute elapsed time
            RaiseEvent(ElapsedTime);                // Raise an event with the elapsed time
        }

        #endregion

        #region . Methods .

        #region . Methods: Public .

        /// <summary>
        /// Start the timer.
        /// </summary>
        internal void StartTimer()
        {
            startTime = DateTime.Now;                      // Save the start time to now
            if (timer == null)                             // Is the timer variable null?
                timer = new Timer(interval);              // Yes, then instantiate a new timer instance and initialize the interval to 1 second
            timer.Elapsed += timer_Elapsed;               // Set the timer event handler
            timer.AutoReset = true;                        // Set the autoreset property to true
            timer.Enabled = true;                          // Start the timer
            RaiseEvent(initialValue);                      // Raise the event
        }

        /// <summary>
        /// Stop the timer.
        /// </summary>
        internal void StopTimer()
        {
            try
            {
                if ((timer != null) && (timer.Enabled))   // Is the timer running?
                {
                    timer.Enabled = false;                 // Stop it
                    ComputeElapsedTime();                   // Compute the elapsed time
                    RaiseEvent("");                         // Raise an event
                }
            }
            catch (Exception)
            {
                // TODO: What to do here?
            }
            finally
            {
                timer = null;                              // Set the timer variable to null
            }
        }

        /// <summary>
        /// Pause the timer.
        /// </summary>
        internal void PauseTimer()
        {
            if ((timer != null) && (timer.Enabled))       // Is the timer running?
                timer.Enabled = false;                     // Stop it
        }

        /// <summary>
        /// Resume the timer.
        /// </summary>
        internal void ResumeTimer()
        {
            if (timer != null)                             // Is the timer variable null?
            {
                LoadPreviousTime();                         // No, then load the previously saved time
                timer.Enabled = true;                      // Start the timer
            }
        }

        internal void ContinuePreviousTimer()
        {
            if (timer == null)                             // Is the timer variable null?
                timer = new Timer(interval);              // Yes, then instantiate a new timer instance and initialize the interval to 1 second
            timer.Elapsed += timer_Elapsed;               // Set the timer event handler
            timer.AutoReset = true;                        // Set the autoreset property to true
            RaiseEvent(initialValue);                      // Raise the event
            ResumeTimer();
        }

        /// <summary>
        /// Load the previously saved time.
        /// </summary>
        internal void LoadPreviousTime()
        {
            TimeSpan diff = Properties.Settings.Default.ElapsedTime;    // Load the previously saved time
            startTime = DateTime.Now - diff;                           // Compute the difference
        }

        /// <summary>
        /// Reset the timer.
        /// </summary>
        internal void ResetTimer()
        {
            if (timer != null)                             // Is the timer variable null?
            {
                timer.Enabled = false;                     // No, stop the timer
                startTime = DateTime.Now;                  // Reset the start time to now
                timer.Enabled = true;                      // Start the timer again
                RaiseEvent(initialValue);                  // Raise an event
            }
        }

        #endregion

        #region . Methods: Private .

        private void ComputeElapsedTime()
        {
            try
            {
                TimeSpan diff = DateTime.Now - startTime;      // Compute the difference between the start time and now.
                Properties.Settings.Default.ElapsedTime = diff; // Save it to the application configuration
                ElapsedTime = diff.ToString(timeFormat);       // Save the elapsed time
            }
            catch (Exception)
            {
                ElapsedTime = initialValue;                    // Error, initialize the elapsed time
            }
        }

        protected virtual void RaiseEvent(string value)
        {
            EventHandler<GameTimerEventArgs> handler = GameTimerEvent;
            if (handler != null)
            {
                GameTimerEventArgs e = new GameTimerEventArgs(value);
                handler(this, e);
            }
        }

        #endregion

        #endregion
    }
}
