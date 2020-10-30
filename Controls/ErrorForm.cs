using System;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RandREng.Controls
{
    public interface IErrorHelper
    {
        string User { get; }
        Task SendMailAsync(string body);
    }

    public partial class ErrorForm : Form
    {
        private bool bAdvanced;
        private Size oldClientSize;
        private int OldButtonsY;
        private int OldTextBoxY;
        public static IErrorHelper ErrorHelper { get; set; }

        public ErrorForm()
        {
            InitializeComponent();
            oldClientSize = this.ClientSize; 
            OldButtonsY = this.buttonOK.Location.Y;
            OldTextBoxY = this.textBox.Location.Y;

            CriticalError = false;
        }

        public string ErrorMessage
        {
            set
            {
                this.textBox.Text = value;
            }
        }

        public bool CriticalError { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Text = Assembly.GetEntryAssembly().GetName().Name;
            if (CriticalError)
            {
                this.labelHeading1.Text = string.Format("{0} has experienced an error and must be closed. We apologize for the inconvenience", Assembly.GetEntryAssembly().GetName().Name);
            }
            else
            {
                labelHeading1.Text = string.Format("{0} has experienced an error.  Please contact technical support for further assistance.", Assembly.GetEntryAssembly().GetName().Name);
            }            
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            string message = this.GetData();
            ClipboardUtilities.SetText(message);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public static void ConfigureThreadExceptionhandling()
        {
            // setup handler for worker threads
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler( CurrentDomain_UnhandledException );

            // in WinForms the main thread is special and needs a separate handler
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler( Application_ThreadException );
        }

        static void Application_ThreadException( object sender, System.Threading.ThreadExceptionEventArgs e )
        {
            handleThreadException( sender, e.Exception );
        }

        static void CurrentDomain_UnhandledException( object sender, UnhandledExceptionEventArgs e )
        {
            Exception exception = (Exception) e.ExceptionObject;
            handleThreadException( sender, exception );
        }

        private static void handleThreadException( object sender, Exception e )
        {

            if ( !e.StackTrace.Contains( "ErrorForm" ) )
            {
                DisplayException(e);
            }

//            throw e;
        }

        public static void DisplayException( Exception e )
        {
            DisplayException( e, true );
        }

        public static void DisplayException( Exception e, bool showStackAndExceptionType )
        {
            DisplayException( "Exception", e, showStackAndExceptionType );
        }

        public static void DisplayException( string caption, Exception e )
        {
            DisplayException( caption, e, true );
        }

        public static void DisplayException( string caption, Exception e, bool showStackAndExceptionType )
        {
            Exception ex = e;
            string message = String.Format("Build:{0}\r\n\r\n", Assembly.GetEntryAssembly().GetName().Version.ToString());
            string prefix = "";
            while (ex != null)
            {
                if (showStackAndExceptionType)
                {
                    message += prefix + ex.GetType().ToString() + "\r\n" + ex.Message + "\r\n\r\n" + ex.StackTrace + "\r\n\r\n" ;
                }
                else
                {
                    message += prefix + ex.Message + "\r\n\r\n";
                }
                ex = ex.InnerException;
                prefix = "[INNER EXCEPTION] ";
            }
            using (ErrorForm errorForm = new ErrorForm())
            {
                errorForm.Text = caption;
                errorForm.ErrorMessage = message;
                errorForm.ShowDialog();
            }
        }

        public static DialogResult DisplayError(string caption, string message)
        {
            using (ErrorForm errorForm = new ErrorForm())
            {
                errorForm.Text = caption;
                errorForm.ErrorMessage = message;
                return errorForm.ShowDialog();
            }
        }

        private void onLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            const string location = "http://www.custom-installs.com";
            //this.linkLabelLaunch.Links.Add(0, this.linkLabelLaunch.Text, location);
            System.Diagnostics.Process.Start(location);
        }

        private void btnAdvanced_Click(object sender, EventArgs e)
        {
            this.SuspendLayout();
            if(!bAdvanced)
            {
                this.ClientSize = this.panelEx.Size;
                bAdvanced = true;
                this.textBox.Location = new Point(this.textBox.Location.X,OldButtonsY);
                this.btnDetails.Location = new Point(this.btnDetails.Location.X, this.textBox.Bottom+3);
                this.buttonOK.Location = new Point(this.buttonOK.Location.X, this.textBox.Bottom + 3);
                this.buttonCopy.Location = new Point(this.buttonCopy.Location.X, this.textBox.Bottom + 3);
                this.buttonEmail.Location = new Point(this.buttonEmail.Location.X, this.textBox.Bottom + 3);
                this.labelError.Visible = true;


            }else
            {
                this.textBox.Location = new Point(this.textBox.Location.X, OldTextBoxY);
                this.btnDetails.Location = new Point(this.btnDetails.Location.X, OldButtonsY);
                this.buttonOK.Location = new Point(this.buttonOK.Location.X, OldButtonsY);
                this.buttonEmail.Location = new Point(this.buttonEmail.Location.X, OldButtonsY);
                this.ClientSize = this.oldClientSize;
                this.labelError.Visible = false;
                bAdvanced = false;
            }
            this.ResumeLayout(true);
        }

        private void ErrorForm_Load(object sender, EventArgs e)
        {
            CenterToScreen();
        }

        private void buttonEmail_Click(object sender, EventArgs e)
        {
            this.buttonEmail.Enabled = false;
            ErrorHelper.SendMailAsync(GetData());
            this.buttonEmail.Enabled = true;
        }

        private string GetData()
        {
            string message = "";


            message += ErrorHelper.User; 
            message += "\r\n";
            message += DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString();
            message += "\r\n";
            message += Environment.MachineName;
            message += "\r\n\r\n";
            message += this.textBox.Text;

            return message;
        }
    }

}