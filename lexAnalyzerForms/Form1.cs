using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Xml.Linq;
using static lexAnalyzerForms.Lexer;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace lexAnalyzerForms
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            Lexer lexer = new Lexer(tbInput.Text);
            List<Lexem> myStorage;
            myStorage = lexer.GetLexemStorage();
            tbOutput.Text = lexer.GetOutputText();
            
        }
    }

}
