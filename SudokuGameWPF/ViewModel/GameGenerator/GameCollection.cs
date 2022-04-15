using SudokuGameWPF.Model.Enums;
using SudokuGameWPF.Model.Structures;
using SudokuGameWPF.ViewModel.CustomEventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SudokuGameWPF.ViewModel.GameGenerator
{
    internal class GameCollection
    {
        #region . Variables, Constants, And other Declarations .

        #region . Constants .

        private const Int32 cDepth = 5;    // Number of games to keep around

        #endregion

        #region . Variables .

        private DifficultyLevels level;
        private Queue<CellClass[,]> games;
        private object qLock;
        private bool stop;
        private AutoResetEvent makeMoreGames;
        private GameGenerator cGameGenerator;

        #endregion

        #region . Other Declarations .

        internal event EventHandler<GameManagerEventArgs> GameManagerEvent;

        #endregion

        #endregion

        #region . Constructors .

        /// <summary>
        /// Initializes a new instance of the GameCollection class.
        /// </summary>
        /// <param name="level">The level of difficulty for this instance.</param>
        internal GameCollection(DifficultyLevels level)
        {
            InitializeVariables(level);
        }

        #endregion

        #region . Properties: Public .

        /// <summary>
        /// Gets the number of games in the queue.
        /// </summary>
        internal Int32 GameCount
        {
            get
            {
                lock (qLock)                                   // Obtain a lock on the game queue.
                {
                    if (games == null)                         // Is the queue object null?
                        games = new Queue<CellClass[,]>();     // Yes, then instantiate a new queue object.
                    return games.Count;                        // Return the count in the queue.
                }
            }
        }

        /// <summary>
        /// Gets a game off the queue.
        /// </summary>
        internal CellClass[,] GetGame
        {
            get
            {
                try
                {
                    lock (qLock)                                       // Obtain a lock on the queue.
                    {
                        if (games == null)                             // Is the queue object null?
                            games = new Queue<CellClass[,]>();         // Yes, then instantiate a new queue object.
                        if (games.Count > 0)                           // Any games in the queue?
                        {
                            CellClass[,] cells = games.Dequeue();      // Yes, pop a game off the queue
                            RaiseEvent(games.Count);                   // Raise a new event with the new count
                            makeMoreGames.Set();                       // Tell the background thread to create another game
                            return cells;                               // Return the game that was just removed.
                        }
                    }
                }
                catch (Exception)
                {
                    // TODO: What to do here?
                    // Maybe log the error in the Application.Event log?
                }
                return null;                                            // Error ... just return null.
            }
        }

        #endregion

        #region . Event Handlers: Other Events .

        private void GameGeneratorEventHandler(object sender, GameGeneratorEventArgs e)
        {
            Int32 count = 0;                                    // Init the counter to zero.
            lock (qLock)                                       // Obtain a lock on the queue
            {
                if (games == null)                             // Is the queue object null?
                    games = new Queue<CellClass[,]>();         // Yes, then instantiate a new queue object
                games.Enqueue(e.Cells);                        // Add the new game to the queue
                count = games.Count;
            }
            RaiseEvent(count);                                  // Raise an event with the new count
            makeMoreGames.Set();                               // Tell the background thread to make more games
        }

        #endregion

        #region . Methods .

        #region . Methods: Public .

        /// <summary>
        /// Starts a thread to manage this queue.
        /// </summary>
        internal void StartThread()
        {
            CreateGames();                                              // Start the background thread to create games.
        }

        /// <summary>
        /// Stop the thread that is managing this queue.
        /// </summary>
        internal void StopThread()
        {
            stop = true;                                               // Raise the stop flag
            makeMoreGames.Set();                                       // Unblock the background thread if it is blocked
        }

        /// <summary>
        /// Converts the games in the queue into a string that can be saved.
        /// </summary>
        /// <returns>A string that represents the games in the queue.</returns>
        internal string SaveGames()
        {
            try
            {
                lock (qLock)                                               // Obtain a lock on the game queue
                {
                    if (games != null)                                     // Is the queue object null?
                    {
                        StringBuilder sTemp = new StringBuilder();          // No, then instantiate a string builder object
                        foreach (CellClass[,] item in games)               // For each game in the queue
                            if (item != null)                               // If it's not null
                                sTemp.Append(ConvertGameToString(item));    // Convert it to a string
                        return sTemp.ToString();                            // Return the string of games
                    }
                    else
                        games = new Queue<CellClass[,]>();                 // Game object is null, so instantiate a new queue object
                }
            }
            catch (Exception)
            {
                // TODO: What to do here?
                // Maybe log error into Application.Event log?
            }
            return null;                                                    // Error somewhere, just return a null string
        }

        /// <summary>
        /// Converts a string of saved games into the proper format.
        /// </summary>
        /// <param name="sGames">A string representing the saved games.</param>
        internal void LoadGames(string sGames)
        {
            if (!string.IsNullOrWhiteSpace(sGames))                         // Is the input parameter null?
            {
                lock (qLock)                                               // No, obtain a lock on the queue object
                {
                    if (games == null)                                     // Is the queue object null?
                        games = new Queue<CellClass[,]>();                 // Yes, then instantiate a new queue objectc
                    Int32 iPtr = 0;                                         // Initialize the pointer to zero
                    while (sGames.Length > iPtr)                            // While there are more games to process
                    {
                        string sTemp = sGames.Substring(iPtr, 162);         // Extract the game starting at the pointer
                        CellClass[,] cells = ConvertStringToGame(sTemp);    // Convert the string to a game
                        if (cells != null)                                  // Is the return value null?
                            games.Enqueue(cells);                          // No, then save it to the queue
                        iPtr += 162;                                        // Increment the pointer to the next game
                    }
                    RaiseEvent(games.Count);                               // Done, raise an event with the new game count
                }
            }
        }

        #endregion

        #region . Methods: Private .

        private void InitializeVariables(DifficultyLevels level)
        {
            this.level = level;                                                     // Save the difficulty level that we are managing
            stop = false;                                                      // Default the stop flag to false
            qLock = new object();                                              // Instantiate a new lock object
            makeMoreGames = new AutoResetEvent(false);                         // Instantiate a new AutoResetEven object
            cGameGenerator = new GameGenerator(level);                         // Instantiate the game creator object
            cGameGenerator.GameGeneratorEvent += GameGeneratorEventHandler;    // Add the event handler
            lock (qLock)                                                       // Obtain a lock on the queue object
            {
                games = new Queue<CellClass[,]>();                             // Instantiate a new queue object
            }
        }

        private void CreateGames()
        {
            Thread t = new Thread(new ThreadStart(GameMaker));      // Create a new thread
            t.IsBackground = true;                                  // Set the thread to background mode
            t.Start();                                              // Start it
        }

        private void GameMaker()
        {
            do                                                      // Enter a loop
            {
                try
                {
                    lock (qLock)                                   // Obtain a lock on the queue object
                    {
                        if (games == null)                         // Is the queue object null?
                            games = new Queue<CellClass[,]>();     // Yes, then instantiate a new queue object
                        if ((!cGameGenerator.Busy) && (games.Count < cDepth))   // Game creator not busy and we need more games?
                            cGameGenerator.CreateNewGame();        // Yes, then create another game
                    }
                    makeMoreGames.WaitOne();                       // Wait for a signal to continue
                }
                catch (Exception)
                {
                    // TODO: What to do here?
                    // Maybe log the error to the Application.Event log?
                }
            } while (!stop);                                       // Keep looping until the stop flag is raised
        }

        private string ConvertGameToString(CellClass[,] cells)
        {
            StringBuilder sTemp = new StringBuilder();              // Instantiate a new string builder object
            for (Int32 col = 0; col < 9; col++)                     // Loop through the columns
                for (Int32 row = 0; row < 9; row++)                 // Loop through the rows
                    sTemp.Append(cells[col, row].ToString());       // Append to the string builder object
            return sTemp.ToString();                                // Return the whole string
        }

        private CellClass[,] ConvertStringToGame(string sInput)
        {
            if (sInput.Length >= 162)                                       // Is the input string the right length?
            {
                CellClass[,] cells = new CellClass[9, 9];                   // Yes, the instantiate a new 2D array to hold the new game
                Int32 iPtr = 0;                                             // Initialize the pointer variable
                for (Int32 col = 0; col < 9; col++)                         // Loop through the columns
                    for (Int32 row = 0; row < 9; row++)                     // Loop through the rows
                    {
                        string sTemp = sInput.Substring(iPtr, 2);           // Extract a 2 character string at the pointer location
                        cells[col, row] = new CellClass(col, row, sTemp);   // Instantiate a new CellClass object with that string
                        if (cells[col, row].InvalidState)                   // Was the conversion valid?
                            return null;                                    // No, then abort and return null
                        iPtr += 2;                                          // Yes, the increment pointer by 2
                    }
                return cells;                                               // Return the game that was restored
            }
            return null;                                                    // Problems, return null instead
        }

        protected virtual void RaiseEvent(Int32 count)
        {
            EventHandler<GameManagerEventArgs> handler = GameManagerEvent;
            if (handler != null)
            {
                GameManagerEventArgs e = new GameManagerEventArgs(level, count);
                handler(this, e);
            }
        }

        #endregion

        #endregion
    }
}
