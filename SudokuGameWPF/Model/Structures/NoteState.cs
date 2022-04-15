using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using SudokuGameWPF.ViewModel;

namespace SudokuGameWPF.Model.Structures
{
    public class NoteState : INotifyPropertyChanged
    {
        #region . Variables, constants, and other declarations .

        #region . Variables .

        private readonly string stateValue;                            // Stores the string value of the note.
        private bool state;                                            // Stores the state value of the note.  True = display it.  False = hide it.

        #endregion

        #region . Other declarations .

        public event PropertyChangedEventHandler PropertyChanged;       // Interface definition

        #endregion

        #endregion

        #region . Constructors .

        /// <summary>
        /// Initializes a new instance of the NoteState class.
        /// </summary>
        /// <param name="value">Value for this note instance.</param>
        internal NoteState(Int32 value)
        {
            if (Common.IsValidAnswer(value))                                // Check that the input value is valid.
            {
                stateValue = string.Format(" {0} ", value.ToString());     // Yes, then save it.
                State = false;                                              // Default state to False.
            }
            else
                throw new Exception("Invalid input.");                      // No, raise an exception.
        }

        #endregion

        #region . Properties .

        #region . Properties: Public Read-Only .

        /// <summary>
        /// Gets the string value to display.
        /// </summary>
        public string Value
        {
            get
            {
                if (State)                                                  // If the state is True.
                    return stateValue;                                     // Return the value to display
                return "   ";                                               // Otherwise, return a blank string.
            }
        }

        #endregion

        #region . Properties: Public Read/Write.

        /// <summary>
        /// Gets or set the display state for this instance.  True = display the value.  False = hide the value.
        /// </summary>
        public bool State
        {
            get
            {
                return state;                                              // Return the current state.
            }
            set
            {
                state = value;                                             // Save the state.
                OnPropertyChanged("Value");                                 // Raise the event on the Value property.
            }
        }

        #endregion

        #endregion

        #region . Interface Implementation .

        // This routine is normally called from the Set accessor of each property
        // that is bound to the a WPF control.
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}

