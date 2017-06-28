using System;

namespace EventHandlerSample
{
    class Program
    {
        //This is the type to be used by EventHandler.
        //This event args will hold information about the event
        class ThresholdEventArgs : EventArgs {
            public DateTime WhenHappened { get; set; }
            public int ValueGreaterThanThreshold { get; set; }
        }

        //a sample class where we'll get an event information
        class Counter
        {
            private int total;
            private const int THRESHOLD = 10;

            //add a number to total, if number is greater than threshold, runs an event.
            public void Add(int i)
            {
                total += i;

                ThresholdEventArgs args = new ThresholdEventArgs();
                args.WhenHappened = DateTime.Now;
                args.ValueGreaterThanThreshold = total;

                if (total > THRESHOLD)
                {
                    //runs event handler statements 
                    this.OnThresholdReachedEventHandler?.Invoke(this, args);//suggested syntax

                }
                else {
                    //runs event handler statements 
                    EventHandler<ThresholdEventArgs> notReachedHandler = this.OnThresholdNotReachedEventHandler;//old syntax
                    if (notReachedHandler != null)
                        notReachedHandler(this, args);
                }
            }
            
            //event handlers must be public
            public event EventHandler<ThresholdEventArgs> OnThresholdReachedEventHandler;
            public event EventHandler<ThresholdEventArgs> OnThresholdNotReachedEventHandler;
        }

        static void Main(string[] args)
        {
            //test event handler
            Counter counter = new Counter();

            //attach your own methods to the counter's event handler for printing warning when threshold is reached
            counter.OnThresholdReachedEventHandler += counter_OnThresholdReachedEventHandler;
            counter.OnThresholdNotReachedEventHandler += counter_OnThresholdNotReachedEventHandler;

            for (int i = 0; i < 10; i++)
                counter.Add(i);

            Console.Read();
        }

        //when threshold is reached by Add method, prints a warning message
        private static void counter_OnThresholdReachedEventHandler(object sender, ThresholdEventArgs e)
        {
            //here we have event handler statements
            Console.WriteLine($"WARNING: Threshold reached at {e.WhenHappened}: Total is {e.ValueGreaterThanThreshold}.");
        }

        //when threshold is not reached, prints a message just for info
        private static void counter_OnThresholdNotReachedEventHandler(object sender, ThresholdEventArgs e)
        {
            //here we have event handler statements
            Console.WriteLine($"INFO: Threshold not reached at {e.WhenHappened}: Total is {e.ValueGreaterThanThreshold}.");
        }

        
    }
}
