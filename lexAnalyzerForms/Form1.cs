using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace lexAnalyzerForms
{
    //вещественные и целые числа хранятся в нужном формате, не string
    //создать структуру, которая хранит лексемы, тип лексемы и значение
    //служебные слова

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class Lexem
        {
            public LexemType Type { get; set; }
            public string Name { get; set; }
            public object Value { get; set; }

            public void addToStorage(LexemType type, string name, object value)
            {
                Type = type;
                Name = name;
                Value = value;
            }
            public void addType(LexemType type)
            {
                Type = type;
            }
            public void addName(string name)
            {
                Name = name;
            }
            public void addValue(object value)
            {
                Value = value;
            }
            /*public Lexems(string type, string name, object value)
            {
                Type = type;
                Name = name;
                Value = value;
            }*/
            public Lexem()
            {
                Type = LexemType.EMPTY;
                Name = "";
                Value = "";
            }
        }

        public enum State
        {
            S, I, C, D, E,
            R, B, K, O, M,
            L, F
        }
        public enum LexemType
        {
            //DEFAULT = -3,
            EMPTY,
            //ERROR = -1,
            INTEGER,
            DECIMAL,
            PLUS,
            MINUS,
            MULTIPLY,
            DIVIDE,
            LPAREN,
            RPAREN,
            SEMICOLON,
            COMMA,
            LBRACE,
            RBRACE,
            LSQUARE,
            RSQUARE,
            NOT,
            LESS,
            GREATER,
            LESS_OR_EQUAL,
            GREATER_OR_EQUAL,
            EQUAL,
            NOT_EQUALS,
            ASSIGN,
            INT_DECLARE,
            FLOAT_DECLARE,
            ARRAY_DECLARE,
            INPUT,
            OUTPUT,
            IF,
            ELSE,
            WHILE,
            AND,
            OR,
            NAME,
            EOL,
            END
        }

        string inputText, outputText;

        public List<string> KeywordsList = new List<string> { "if", "else", "while", "arr", "input", "output", "int", "decimal" };
        private string name = "";
        int chislo;
        int pos;
        bool skipLexemFlag = false;
        public List<Lexem> LexemStorage = new List<Lexem>();

        public bool IsKeyword(string word)
        {
            if (KeywordsList.Contains(word)) return true;
            return false;
        }
        bool IsLetter(char symbol)
        {
            if (symbol >= 'a' && symbol <= 'z') return true;
            return false;
        }
        bool IsNumber(char symbol)
        {
            if (symbol >= '0' && symbol <= '9') return true;
            return false;
        }
        public void AddToStorage(string name)
        {
            Lexem lex = new Lexem();
            lex.addName(name);
            if (name == "+") lex.addType(LexemType.PLUS);
            else if (name == "-") lex.addType(LexemType.MINUS);
            else if (name == "*") lex.addType(LexemType.MULTIPLY);
            else if (name == "/") lex.addType(LexemType.DIVIDE);
            else if (name == "(") lex.addType(LexemType.LPAREN);
            else if (name == ")") lex.addType(LexemType.RPAREN);
            else if (name == ";") lex.addType(LexemType.SEMICOLON);
            else if (name == ",") lex.addType(LexemType.COMMA);
            else if (name == "{") lex.addType(LexemType.LBRACE);
            else if (name == "}") lex.addType(LexemType.RBRACE);

            else if (name == "=") lex.addType(LexemType.EQUAL);
            else if (name == "[") lex.addType(LexemType.LSQUARE);
            else if (name == "]") lex.addType(LexemType.RSQUARE);
            else if (name == "<") lex.addType(LexemType.LESS);
            else if (name == ">") lex.addType(LexemType.GREATER);
            else if (name == "<=") lex.addType(LexemType.LESS_OR_EQUAL);
            else if (name == ">=") lex.addType(LexemType.GREATER_OR_EQUAL);
            else if (name == "!=") lex.addType(LexemType.NOT_EQUALS);
            else if (name == ":=") lex.addType(LexemType.ASSIGN);

            //else if (name == " ") lex.addType(LexemType.DIVIDE);
            //else if (name == "\n") lex.addType(LexemType.DIVIDE);
            else if (name == "^") lex.addType(LexemType.AND);
            else if (name == "|") lex.addType(LexemType.OR);
            else if (IsNumber(name[0]))
            {
                if (name.Contains(".")) lex.addType(LexemType.DECIMAL);
                else lex.addType(LexemType.INTEGER);
            }
            else if (IsLetter(name[0]))
            {
                if (name == "if") lex.addType(LexemType.IF);
                else if (name == "else") lex.addType(LexemType.ELSE);
                else if (name == "while") lex.addType(LexemType.WHILE);
                else if (name == "arr") lex.addType(LexemType.ELSE);
                else if (name == "input") lex.addType(LexemType.INPUT);
                else if (name == "output") lex.addType(LexemType.OUTPUT);
                else if (name == "int") lex.addType(LexemType.INT_DECLARE);
                else if (name == "decimal") lex.addType(LexemType.FLOAT_DECLARE);
                else if (name == "arr") lex.addType(LexemType.ARRAY_DECLARE);

                else lex.addType(LexemType.NAME);
            }
            LexemStorage.Add(lex);
            //else ()

            //END

        }
        
        public bool IsSkipLexem()
        {
            if (skipLexemFlag)
            {
                skipLexemFlag = false;
                return true;
            }
            return false;
        }

        public bool IsSingleCharLexem(char symbol)
        {
            if (symbol == '+' || symbol == '-' || symbol == '*' || symbol == '[' || symbol == ']' || symbol == '{' || symbol == '}' || symbol == '(' || symbol == ')' || symbol == '|' || symbol == '^' || symbol == ';' || symbol == ',') return true;
            return false;
        }
        public bool IsPossiblyDoubleCharLexem(char symbol)
        {
            if ( symbol == '/' || symbol == '<' || symbol == '>' || symbol == ':' || symbol == '!') return true; 
            return false;
        }
        public char GetSecondCharOfLexem(char symbol)
        {
            if (symbol == '/') return '*';
            else return '=';
        }

        public string scanWord()
        {
            string wordd = "";
            wordd = inputText[pos].ToString();
            if (pos + 1 < inputText.Length)
            {
                char nextSymbol = inputText[pos + 1];
                while (IsLetter(nextSymbol) || IsNumber(nextSymbol))
                {
                    wordd += nextSymbol;
                    pos++;
                    if (pos + 1 < inputText.Length)
                        nextSymbol = inputText[pos + 1];
                    else break;
                }
            }

            return wordd;
        }

        string scanLex()
        {
            string scannedLexem = "";
            char currSymbol = inputText[pos];

            if (IsSingleCharLexem(currSymbol))
            {
                return currSymbol.ToString();
            }

            if (IsPossiblyDoubleCharLexem(currSymbol))
            {
                if(pos + 1 < inputText.Length)
                {
                    char secondSymbol = inputText[pos + 1]; 
                    if (secondSymbol == GetSecondCharOfLexem(currSymbol))
                    {
                        pos++;
                        return currSymbol.ToString() + secondSymbol.ToString();
                    }
                }
                return currSymbol.ToString();
            }

            if (IsLetter(currSymbol))
            {
                name = scanWord();
                return name;
            }

            if(currSymbol == ' ' || currSymbol == '\n')
            {
                skipLexemFlag = true;
            }

            return "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LexemStorage.Clear();
            pos = 0;
            inputText = tbInput.Text;
            string scannedLexem = "";

            while (pos < inputText.Length)
            {
                tbOutput.Text += "позиция  " + pos.ToString() + '\n';
                scannedLexem = scanLex();
                if (scannedLexem != "")
                {
                    AddToStorage(scannedLexem);
                    tbOutput.Text += "лексема  " + scannedLexem + '\n' + '\n';
                }
                else
                {
                    if(!IsSkipLexem())
                        tbOutput.Text += "ошибка" + '\n' + '\n';

                    else tbOutput.Text += "пробел или enter" + '\n' + '\n';
                }
                pos++;
            }

            tbOutput.Text += "СПИСОК ЛЕКСЕМ: \n";
            foreach (Lexem lex in LexemStorage)
            {
                tbOutput.Text += "лексема: " + lex.Name + '\n';
                tbOutput.Text += "тип лексемы: " + lex.Type.ToString() + '\n' + '\n';
            }
        }
    }
}
