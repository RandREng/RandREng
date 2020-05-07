namespace RandREng.Controls
{
    public class ClipboardUtilities
    {
        public static void SetText( string text )
        {
            try
            {
                System.Windows.Forms.Clipboard.SetText( text );
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show( "The clipboard is currently locked by another application", "Clipboard Copy Failed", System.Windows.Forms.MessageBoxButtons.OK );
            }
        }
    }
}
