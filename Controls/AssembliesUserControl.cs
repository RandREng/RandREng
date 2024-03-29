using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace RandREng.Controls
{
    public partial class AssembliesUserControl : UserControl
    {
        public AssembliesUserControl()
        {
            InitializeComponent();
        }
        
        public string GetPasteData()
        {
            if (OrderedAssemblies == null)
            {
                return string.Empty;
            }
            System.Xml.Serialization.XmlSerializer serializer =
                new(typeof(List<string>));
            MemoryStream ms = new();
            serializer.Serialize(ms, this.OrderedAssemblies);

            return Encoding.ASCII.GetString(ms.ToArray());       
        }

        private readonly List<string> OrderedAssemblies = new();
        protected override void OnLoad(EventArgs e)
        {
            Assembly[] ourAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly A in ourAssemblies)
            {
                OrderedAssemblies.Add(A.FullName);
            }
            OrderedAssemblies.Sort();

            foreach (string s in OrderedAssemblies)
            {
                this.listBoxAssemblies.Items.Add(s);
            }
            base.OnLoad(e);
        }
    }
}
