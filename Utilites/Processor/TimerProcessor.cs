using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;
using CFI.Utility.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CFI.Utility.Processor
{
    public class TimerProcessor
    {
        Thread Thread {get; set;}
        public bool Processing  {get; protected set;}
        protected ProcessQueue PQueue { get; set; }
        virtual protected ILogger Logger {get; set;}
        private System.Timers.Timer m_BaseTimer;
        protected int Stagger = 0;
        protected bool SyncOnHour = false;
        protected int Interval = 60000;

        protected string InstanceName { get; private set; }
        protected string ProcessorName { get; private set; }
        protected string EventSource { get { return this.ProcessorName + " - " + this.InstanceName; } }

        protected TimerProcessor(string instanceName, string processorName)
            : this(instanceName, processorName, NullLogger.Instance)
        {
        }

        protected TimerProcessor(string instanceName, string processorName, ILogger logger)
        {
            this.ProcessorName = processorName;
            this.InstanceName = instanceName;
            this.Thread = new Thread(this.DoWork);
            this.Logger = logger;
            this.PQueue = new ProcessQueue();
            this.Processing = true;
        }

        protected void Start()
        {
            this.Init();
            if (this.Processing)
            {
                Logger.Log(LogLevel.Information, "Processor starting.");
                this.Thread.Start();
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
                if (this.Processing)
                {
                    this.Processing = false;
                    this.PQueue.Produce("quitthread");
                    this.Thread.Join(30000);
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
            WriteEventLog("Initializing Processor...");
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
                Int32 delay = 0;
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
                    System.Threading.Thread.Sleep((Int32)delay);
                }
                this.Producer(DateTime.Now);

                this.m_BaseTimer = new System.Timers.Timer();
                this.m_BaseTimer.Interval = this.Interval;
                this.m_BaseTimer.Elapsed += new System.Timers.ElapsedEventHandler(m_BaseTimer_Elapsed);
                this.m_BaseTimer.Enabled = true;

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
            this.Producer(e.SignalTime);
        }

        protected void WriteEventLog(string message)
        {
            #if !DEBUG
            CFI.Utility.Logging.Logger.WriteEventLog(this.EventSource, message);
            #endif
        }

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
