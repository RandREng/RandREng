﻿using System;
using System.Threading;
using System.Collections;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using RandREng.Utility.Logger;
using System.Threading.Tasks;

namespace RandREng.Utility.Processor
{
    public class TimerProcessor
    {
        private Thread Thread { get; set; }
        public bool Processing { get; protected set; }
        protected ProcessQueue PQueue { get; set; }
        protected virtual ILogger Logger { get; set; }
        private System.Timers.Timer _baseTimer;
        protected int Stagger;
        protected bool SyncOnHour;
        protected int Interval = 60000;

        protected string InstanceName { get; private set; }
        protected string ProcessorName { get; private set; }
        protected string EventSource { get { return ProcessorName + " - " + InstanceName; } }

        protected TimerProcessor(string instanceName, string processorName)
            : this(instanceName, processorName, NullLogger.Instance)
        {
        }

        protected TimerProcessor(string instanceName, string processorName, ILogger logger)
        {
            ProcessorName = processorName;
            InstanceName = instanceName;
            Thread = new Thread(DoWork);
            Logger = logger;
            PQueue = new ProcessQueue();
            Processing = true;
        }

        protected void Start()
        {
            InitAsync().Wait();
            if (Processing)
            {
                Logger.Log(LogLevel.Information, "Processor starting.");
                Thread.Start();
            }
            else
            {
                Logger.Log(LogLevel.Information, "Processor DISABLED.");
            }
        }

        /// <summary>
        /// Stop this service.
        /// </summary>
        public void Stop()
        {
            try
            {
                Logger.Log(LogLevel.Information, "Processor stopping.");
                if (Processing)
                {
                    Processing = false;
                    PQueue.Produce("quitthread");
                    Thread.Join(30000);
                }
                Logger.Log(LogLevel.Information, "Processor stopped.");
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex);
            }
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected async virtual Task InitAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
//            WriteEventLog("Initializing Processor...");
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected async virtual Task Consumer(object o)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
        }

        protected virtual void Producer(DateTime timestamp)
        {
            PQueue.Produce(timestamp);
        }

        protected void DoWork()
        {
            try
            {
                int delay = 0;
#if !DEBUG
                if (this.SyncOnHour)
                {
                    DateTime now = DateTime.Now;
                    DateTime last = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
                    delay = (60 * 60 * 1000) - ((Int32)((DateTime.Now - last).TotalMilliseconds));
                    if (delay < 0)
                    {
                        delay = 0;
                    }
                }
                delay += this.Stagger;
#endif
                Logger.Log(LogLevel.Information, "Processor starting delay:", delay);

                if (delay > 0)
                {
                    Thread.Sleep(delay);
                }
                Producer(DateTime.Now);

                _baseTimer = new System.Timers.Timer
                {
                    Interval = Interval
                };
                _baseTimer.Elapsed += new System.Timers.ElapsedEventHandler(baseTimer_Elapsed);
                _baseTimer.Enabled = true;

                while (Processing)
                {
                    try
                    {
                        object o = PQueue.Consume();
                        Consumer(o).Wait();
                    }
                    catch (Exception e)
                    {
                        Logger.LogCritical(e);
                    }
                }
                Logger.Log(LogLevel.Information, "ProcessQueueThread() - Thread procedure stopping.");
            }
            catch (Exception e)
            {
                Logger.LogCritical(e);
            }
        }

        private void baseTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Producer(e.SignalTime);
        }

//        protected void WriteEventLog(string message)
//        {
//#if !DEBUG
//            CFI.Utility.Logging.Logger.WriteEventLog(this.EventSource, message);
//#endif
//        }

        protected class ProcessQueue
        {
            private readonly object _queueLock = new();
            private readonly Queue _queue = new();

            public void Produce(object o)
            {
                lock (_queueLock)
                {
                    _queue.Enqueue(o);
                    if (_queue.Count == 1)
                    {
                        Monitor.Pulse(_queueLock);
                    }
                }
            }

            public object Consume()
            {
                lock (_queueLock)
                {
                    while (_queue.Count == 0)
                    {
                        Monitor.Wait(_queueLock);
                    }
                    return _queue.Dequeue();
                }
            }

            public int Count()
            {
                lock (_queueLock)
                {
                    return _queue.Count;
                }
            }
        }
    }
}
