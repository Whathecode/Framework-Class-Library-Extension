using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;


namespace Whathecode.System.Diagnostics
{
    /// <summary>
    ///   A stopwatch which can give additional statistics for timed intervals.
    /// </summary>
    /// <author>Steven Jeuris</author>
    public partial class StatisticsStopwatch
    {
        readonly Stopwatch _stopwatch = new Stopwatch();
        readonly DateTime _timingStart;
        readonly List<TimeSpan> _intervals = new List<TimeSpan>();

        readonly string _label;


        StatisticsStopwatch( DateTime start, string label )
        {
            _stopwatch.Start();
            _timingStart = start;

            _label = label;
        }


        public static StatisticsStopwatch Start()
        {
            return Start( "" );
        }

        public static StatisticsStopwatch Start( string label )
        {
            Contract.Requires( label != null );

            return new StatisticsStopwatch( DateTime.Now, label );
        }

        public void StartNextMeasurement()
        {
            // Make sure no previous measurement is running.
            StopCurrentMeasurement();

            // Start new measurement.
            _stopwatch.Restart();
        }

        void StopCurrentMeasurement()
        {
            if ( _stopwatch.IsRunning )
            {
                _stopwatch.Stop();

                // Add new interval.
                _intervals.Add( new TimeSpan( _stopwatch.ElapsedTicks ) );
            }
        }

        public Measurement Stop()
        {
            DateTime timingStop = DateTime.Now;

            // When a previous measurement was still running, stop it first and add interval.
            StopCurrentMeasurement();

            // Return latest updated measurement.            
            return new Measurement( _label, _timingStart, timingStop, _intervals );
        }
    }
}