using System;
using System.Threading;
using System.Collections;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using RandREng.Utility.Logger;

namespace RandREng.Utility.Processor
{
    public class TimerProcessor
    {
        Thread Thread { get; set; }
        public bool Processing { get; protected set; }
        protected ProcessQueue PQueue { get; set; }
        virtual protected ILogger Logger { get; set; }
        private System.Timers.Timer m_BaseTimer;
        protected int Stagger = 0;
        protected bool SyncOnHour = false;
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
            Init();
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

        virtual protected void Init()
        {
//            WriteEventLog("Initializing Processor...");
        }

        virtual protected void Consumer(object o)
        {
        }

        virtual protected void Producer(DateTime timestamp)
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
                Logger.Log(LogLevel.Information, string.Format("Processor starting delay: {0}.", delay));

                if (delay > 0)
                {
                    Thread.Sleep(delay);
                }
                Producer(DateTime.Now);

                m_BaseTimer = new System.Timers.Timer
                {
                    Interval = Interval
                };
                m_BaseTimer.Elapsed += new System.Timers.ElapsedEventHandler(m_BaseTimer_Elapsed);
                m_BaseTimer.Enabled = true;

                while (Processing)
                {
                    try
                    {
                        object o = PQueue.Consume();
                        Consumer(o);
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

        void m_BaseTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
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
            readonly object queueLock = new object();
            Queue queue = new Queue();

            public void Produce(object o)
            {
                lock (queueLock)
                {
                    queue.Enqueue(o);
                    if (queue.Count == 1)
                    {
                        Monitor.Pulse(queueLock);
                    }
                }
            }

            public object Consume()
            {
                lock (queueLock)
                {
                    while (queue.Count == 0)
                    {
                        Monitor.Wait(queueLock);
                    }
                    return queue.Dequeue();
                }
            }

            public int Count()
            {
                lock (queueLock)
                {
                    return queue.Count;
                }
            }
        }
    }
}
