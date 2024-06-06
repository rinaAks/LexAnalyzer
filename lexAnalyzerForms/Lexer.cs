using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace lexAnalyzerForms
{
    internal class Lexer
    {
        private string textForOutput = "";
        public Lexer(string program) 
        {
            ScanProgram(program);
        }

        public List<Lexem> GetLexemStorage()
        {
            return LexemStorage;
        }

        public string GetOutputText()
        {
            return textForOutput;
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
            DECIMAL_DECLARE,
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
        int chislo;
        int pos;
        bool skipLexemFlag = false;
        public List<Lexem> LexemStorage = new List<Lexem>();
        public List<Lexem> VariableStorage = new List<Lexem>();

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
        public void AddToLexemStorage(string name)
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
                else if (name == "decimal") lex.addType(LexemType.DECIMAL_DECLARE);
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
            if (symbol == '/' || symbol == '<' || symbol == '>' || symbol == ':' || symbol == '!') return true;
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


        public string scanNumber()
        {
            string numberr = "";

            numberr = inputText[pos].ToString();
            if (pos + 1 < inputText.Length)
            {
                char nextSymbol = inputText[pos + 1];
                while (IsNumber(nextSymbol) || nextSymbol == '.')
                {
                    numberr += nextSymbol;
                    pos++;
                    if (pos + 1 < inputText.Length)
                        nextSymbol = inputText[pos + 1];
                    else break;
                }
                if (numberr.Contains("."))
                {
                    if (numberr.IndexOf(".") + 1 < inputText.Length)
                    {
                        if (!IsNumber(numberr[numberr.IndexOf(".") + 1]))
                            numberr = "";
                    }
                    else numberr = "";
                }
            }

            return numberr;
        }

        string scanLex()
        {
            char currSymbol = inputText[pos];

            if (IsSingleCharLexem(currSymbol))
            {
                return currSymbol.ToString();
            }

            if (IsPossiblyDoubleCharLexem(currSymbol))
            {
                if (pos + 1 < inputText.Length)
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
                return scanWord();
            }

            if (currSymbol == ' ' || currSymbol == '\n')
            {
                skipLexemFlag = true;
            }

            if (IsNumber(currSymbol))
            {
                return scanNumber();
            }

            return "";
        }



        private void ScanProgram(string program)
        {
            LexemStorage.Clear();
            VariableStorage.Clear();
            pos = 0;
            inputText = program;
            string scannedLexem = "";

            while (pos < inputText.Length)
            {
                textForOutput += "position  " + pos.ToString() + '\n';
                scannedLexem = scanLex();
                if (scannedLexem != "")
                {
                    AddToLexemStorage(scannedLexem);
                    textForOutput += "lexem  " + scannedLexem + '\n' + '\n';
                }
                else
                {
                    if (!IsSkipLexem())
                        textForOutput += "error" + '\n' + '\n';

                    else textForOutput += "space or enter" + '\n' + '\n';
                }
                pos++;
            }

            textForOutput += "СПИСОК ЛЕКСЕМ: \n";
            foreach (Lexem lex in LexemStorage)
            {
                textForOutput += "lexem: " + lex.Name + '\n';
                textForOutput += "lexem type: " + lex.Type.ToString() + '\n' + '\n';
            }

            
            // ищем среди лексем переменные и сохраняем в VariablesStorage
            for(int i = 0; i < LexemStorage.Count; i++)
            {
                Lexem lex = LexemStorage[i];
                if (lex.Type == LexemType.INTEGER || lex.Type == LexemType.DECIMAL)
                {
                    if (i >= 2)
                    {
                        if (LexemStorage[i - 1].Type == LexemType.ASSIGN && LexemStorage[i - 2].Type == LexemType.NAME)
                        {
                            lex.Value = LexemStorage[i].Name;
                            lex.Name = LexemStorage[i - 2].Name;
                            VariableStorage.Add(lex);
                        }
                    }
                }
            }

            textForOutput += "СПИСОК ПЕРЕМЕННЫХ: \n";
            foreach (Lexem lex in VariableStorage)
            {
                textForOutput += "lexem: " + lex.Name + '\n';
                textForOutput += "lexem type: " + lex.Type.ToString() + '\n';
                textForOutput += "lexem value: " + lex.Value.ToString() + '\n' + '\n';
            }

        }

    }


}
