using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NET02.Entities
{
    public class Book
    {
        private const int NameLength = 1000;
        private readonly string _isbnCode;
        private readonly string _name;
        
        //Regex is necessary for check form of ISBN code
        private static readonly Regex FirstRegex = new (@"[0-9]{3}-[0-9]{1}-[0-9]{2}-[0-9]{6}-[0-9]{1}");
        private static readonly Regex SecondRegex = new ("[0-9]{13}");
        public DateTime? PublicationDate { get; }
        public List<Author> Authors { get; }

        public string ISBNCode
        {
            get => _isbnCode;
            private init => _isbnCode = ISBNCodeСoercion(value);
        }

        public Book(string isbnCode, string name, List<Author> authors, DateTime publicationDate)
        {
            ISBNCode = isbnCode;
            Name = name;
            Authors = authors;
            PublicationDate = publicationDate;
        }

        public Book(string isbnCode)
        {
            ISBNCode = isbnCode;
        }
        
        public string Name
        {
            get => _name;
            private init
            {
                if (value.Length > NameLength && !string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException($"Invalid Name of Author(max length = {NameLength})");
                }
                
                _name = value;
            }
        }

        public override bool Equals(object? obj)
        {
            return (obj as Book)!.ISBNCode.Equals(this.ISBNCode);
        }

        public static string ISBNCodeСoercion(string value)
        {
            if (!FirstRegex.Match(value).Success && !SecondRegex.Match(value).Success)
            {
                throw new ArgumentException("Invalid isbn code");
            }

            return value.Replace("-", string.Empty);
        }
            
    }
}