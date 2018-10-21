using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BalancedParentheses
{
    public class Program
    {
        private Storage _openParen;
        private Storage _closeParen;

        public void Initialize()
        {
            Console.WriteLine("Creating knowledge base of key value pairs");
            Console.WriteLine("(), {}, [], <>, ui");
            IEnumerable<Pair> pairs = new List<Pair>
            {
                new Pair('(', ')'),
                new Pair('[', ']'),
                new Pair('{', '}'),
                new Pair('<', '>'),
                new Pair('u', 'i')
            };

            // creating a key value pair list to match each of them to each 
            // value of '(' will be ')'
            // value of ')' will be '('
            // creating this will help with speedy look ups
            IEnumerable<KeyValuePair<char, char>> openParenPairs =
                pairs.Select(x => new KeyValuePair<char, char>(x.OpenParen, x.CloseParen));
            IEnumerable<KeyValuePair<char, char>> closedParenPairs =
                pairs.Select(x => new KeyValuePair<char, char>(x.CloseParen, x.OpenParen));

            _openParen = new Storage(openParenPairs);
            _closeParen = new Storage(closedParenPairs);
        }

        public bool IsValid(Parentheses p)
        {
            Stack stack = new Stack();
            string parentheses = p.ParenthesesString;

            // check to verify input is of proper type
            if (!Validate(parentheses)) return false;

            foreach (char c in parentheses)
            {
                if (_openParen.KeyValuePairs.ContainsKey(c)) // check if key exists
                {
                    stack.Push(c);
                }
                else // if the key does not exists, then char is probably is a closed paren
                {
                    char poppedValue = Convert.ToChar(stack.Pop());
                    if (!c.Equals(_openParen.KeyValuePairs[poppedValue]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool Validate(string potentialBrokenParenthesis)
        {
            // null check
            if (potentialBrokenParenthesis == null) return false;

            // checks to verify first character is not of type closing parenthesis 
            if (_closeParen.KeyValuePairs.ContainsKey(potentialBrokenParenthesis[0])) return false;

            // at this point, we can assume string is of valid type
            return true;
        }

        static void Main(string[] args)
        {
            Program p = new Program();
            p.Initialize();
            IEnumerable<Parentheses> list = new List<Parentheses>
            {
                new Parentheses("()()"),
                new Parentheses("(())"),
                new Parentheses("{([]())}"),
                new Parentheses(")"),
                new Parentheses("(})"),
                new Parentheses(null),
                new Parentheses("u()i")
            };

            List<string> results = list.Select(paren => $"{paren.ParenthesesString} : {p.IsValid(paren)}").ToList(); 
            results.ForEach(paren => Console.WriteLine(paren));

            Console.ReadLine();
        }
    }

    public class Storage
    {
        public IDictionary<char, char> KeyValuePairs{ get; }

        public Storage(IEnumerable<KeyValuePair<char, char>> pairs)
        {
            KeyValuePairs = new Dictionary<char, char>();
            foreach (var pair in pairs)
            {
                KeyValuePairs.Add(pair);
            }
        }
    }

    public class Parentheses
    {
        public string ParenthesesString { get;}

        public Parentheses(string parentheses)
        {
            ParenthesesString = parentheses;
        }
    }

    public class Pair
    {
        public char OpenParen { get;}
        public char CloseParen { get;}

        public Pair(char openParen, char closeParen)
        {
            OpenParen = openParen;
            CloseParen = closeParen;
        }
    }
}
