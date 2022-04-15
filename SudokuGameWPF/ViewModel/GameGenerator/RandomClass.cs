using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGameWPF.ViewModel.GameGenerator
{
    internal class RandomClass
    {
        #region . Variables .

        private static RandomClass instance;
        private static object instanceLock = new object();

        private Random rnd;
        private object rndLock = new object();

        #endregion

        #region . Constructors .

        // Declared private to prevent other people from instantiating this class.
        private RandomClass()
        { }

        #endregion

        #region . Methods .

        #region . Methods: Public .

        /// <summary>
        /// Gets a random integer between zero and the specified number.
        /// </summary>
        /// <param name="max">Upper bound of the random number to generate.</param>
        /// <returns></returns>
        internal static Int32 GetRandomInt(Int32 max)
        {
            return GetRandomInt(0, max);
        }

        /// <summary>
        /// Get a random integer between the min and max specified.
        /// </summary>
        /// <param name="min">Lower bound of the random number to generate.</param>
        /// <param name="max">Upper bound of the random number to generate.</param>
        /// <returns></returns>
        internal static Int32 GetRandomInt(Int32 min, Int32 max)
        {
            CheckInstance();                                    // Check if the singleton is generated.
            return instance.GetNextInt(min, max);              // Return a random integer between min and max
        }

        #endregion

        #region . Methods: Private .

        private static void CheckInstance()
        {
            if (instance == null)                          // Is the instance variable null?
            {
                lock (instanceLock)                        // Yes, obtain a lock on the instance object
                {
                    if (instance == null)                  // Check if the instance variable is null again
                    {
                        instance = new RandomClass();      // Instantiate a Random class
                        instance.InitInstance();           // Initialize the instance
                    }
                }
            }
        }

        private void InitInstance()
        {
            if (rnd == null)                               // Is the random variable null?
            {
                lock (rndLock)                             // Yes, obtain a lock on the random object
                {
                    if (rnd == null)                       // Check if the random object is null again
                    {                                       // It is so create a seed and create a new random class
                        TimeSpan tsp = new TimeSpan(DateTime.Now.Ticks);
                        Int32 seed = (int)(((tsp.TotalMilliseconds * 10000) % Int32.MaxValue) % 10000);
                        Debug.WriteLine("Random seed = {0}", seed);
                        rnd = new Random(seed);
                    }
                }
            }
        }

        private Int32 GetNextInt(Int32 min, Int32 max)
        {
            if (rnd == null)                               // If random object is null
                lock (instance)                            // Lock the instance object because other thread
                { }                                         // is still probably creating the instance
            lock (rndLock)                                 // Obtain a lock on the random object
            {
                return rnd.Next(min, max);                 // Return a random number between min and max
            }
        }

        #endregion

        #endregion

    }
}
