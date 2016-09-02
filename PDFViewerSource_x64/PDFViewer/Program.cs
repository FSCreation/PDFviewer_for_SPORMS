using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PDFViewer
{
    static class Program
    {
        /// <summary>
        /// Main Method
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length == 0)
            {
                Application.Run(new PDFViewerForm());
            }
            else
            {
                Application.Run(new PDFViewerForm(args));
            }
        }
    }
}
