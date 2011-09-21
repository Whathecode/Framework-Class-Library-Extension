using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;


namespace Whathecode.System.Diagnostics
{
	public partial class StatisticsStopwatch
	{
		/// <summary>
		///   Measured results from StatisticsStopwatch.
		/// </summary>
		/// <author>Steven Jeuris</author>
		public class Measurement
		{
			readonly DateTime _start;
			readonly DateTime _stop;

			readonly string _label;

			/// <summary>
			///   The total amount of time during which measurements were made.
			/// </summary>
			public TimeSpan TotalInterval { get; private set; }

			/// <summary>
			///   The amount of measurements made.
			/// </summary>
			public int MeasurementCount { get; private set; }

			/// <summary>
			///   The average time of the measurements.
			/// </summary>
			public TimeSpan AverageTime { get; private set; }

			/// <summary>
			///   The average time of the measurements, not including the first measurement.
			///   This is useful when the first call takes more time than sequential calls. (E.g. caching)
			/// </summary>
			public TimeSpan AverageTimeFirstExcluded { get; private set; }

			/// <summary>
			///   The total time of all the measurements.
			/// </summary>
			public TimeSpan TotalTime { get; private set; }


			/// <summary>
			///   Specify a new measurement.
			/// </summary>
			/// <param name = "label">The label to use for this measurement when ToString() is used.</param>
			/// <param name = "start">Start time of the measurement.</param>
			/// <param name = "stop">End time of the measurement.</param>
			/// <param name = "intervals">The different measured intervals.</param>
			public Measurement( string label, DateTime start, DateTime stop, List<TimeSpan> intervals )
			{
				Contract.Requires( stop >= start );

				_start = start;
				_stop = stop;
				_label = label;

				TotalInterval = _stop - _start;
				MeasurementCount = intervals.Count;

				// Get total time.
				long totalTicks = intervals.Sum( interval => interval.Ticks );
				TotalTime = new TimeSpan( totalTicks );

				// Get average times.
				TimeSpan noTime = new TimeSpan( 0 );
				if ( intervals.Count > 0 )
				{
					AverageTime = new TimeSpan( totalTicks / intervals.Count );
					AverageTimeFirstExcluded = intervals.Count > 1
						? new TimeSpan( (TotalTime.Ticks - intervals[ 0 ].Ticks) / (intervals.Count - 1) )
						: noTime;
				}
				else
				{
					AverageTime = noTime;
					AverageTimeFirstExcluded = noTime;
				}
			}


			public override string ToString()
			{
				string output = _label + ": " + AverageTime.TotalMilliseconds + "ms";

				if ( MeasurementCount > 1 )
				{
					output += " average time of " + MeasurementCount + " executions.";

					// Check for big difference when not including first measurement.
					if ( Math.Abs( AverageTime.Ticks - AverageTimeFirstExcluded.Ticks ) > AverageTimeFirstExcluded.Ticks )
					{
						output += " (" + AverageTimeFirstExcluded.TotalMilliseconds + "ms when excluding first call)";
					}
				}

				return output;
			}
		}
	}
}